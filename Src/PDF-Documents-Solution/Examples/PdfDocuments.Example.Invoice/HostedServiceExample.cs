/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PdfDocuments.Example.Invoice
{
	public class HostedServiceExample : HostedServiceTemplate
	{
		public HostedServiceExample(IHostApplicationLifetime hostApplicationLifetime, ILogger<HostedServiceExample> logger, IServiceScopeFactory serviceScopeFactory)
			: base(hostApplicationLifetime, logger, serviceScopeFactory)
		{
		}

		protected override async void OnStarted()
		{
			//
			// Create a sample Invoice document.
			//
			Invoice model = new()
			{
				Id = "12345678",
				PaymentMethod = "ACH",
				CheckNumber = "123481819",
				JobNumber = "123456",
				Terms = "NET30",
				DueDate = DateTime.Now.AddDays(20),
				InvoiceDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
				BillTo = new Address()
				{
					Name = "GROVER INDUSTRIES",
					AddressLine = "123 SESAME ST STE 11",
					CityStateZip = "YELLOWTOWN, NY 10015",
					Phone = "(800)555-0101"
				},
				BillFrom = new Address()
				{
					Name = "ACME WIDGETS",
					AddressLine = "456 MAROON AVE STE 1947",
					CityStateZip = "TOONTOWN, CA 98901",
					Phone = "(888)333-1010"
				},
				Items = new InvoiceItem[]
				{
					new () { Id = 10112, Quantity = 2321, UnitPrice = 1.43M },
					new () { Id = 10115, Quantity = 1211, UnitPrice = 5.411M },
					new () { Id = 10711, Quantity = 54, UnitPrice = 2.21M },
					new () { Id = 10112, Quantity = 6531, UnitPrice = 3.15M },
					new () { Id = 10912, Quantity = 252, UnitPrice = 10.82M },
					new () { Id = 10132, Quantity = 4321, UnitPrice = 11.45M }
				}
			};

			await this.CreatePdfAsync(model);
		}

		protected async Task<int> CreatePdfAsync<TModel>(TModel model)
			where TModel : IPdfModel
		{
			int returnValue = 0;

			//
			// Create a scope.
			//
			using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
			{
				//
				// Register an encoding provider.
				//
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

				//
				// Get the PDF Generator factory.
				//
				IPdfGeneratorFactory factory = scope.ServiceProvider.GetRequiredService<IPdfGeneratorFactory>();

				//
				// Get the generator the PDF
				//
				IPdfGenerator<TModel> generator = await factory.GetAsync<TModel>();

#if DEBUG
				//
				// Use the various option to debug the PDF layout.
				//
				generator.DebugMode = generator.DebugMode
									.SetFlag(DebugMode.RevealGrid, false)
									.SetFlag(DebugMode.RevealLayout, false)
									.SetFlag(DebugMode.HideDetails, false)
									.SetFlag(DebugMode.RevealFontDetails, false)
									.SetFlag(DebugMode.OutlineText, false);
#endif

				//
				// Save and open the PDF.
				//
				await generator.SaveAndOpenPdfAsync(model);
			}

			return returnValue;
		}
	}
}
