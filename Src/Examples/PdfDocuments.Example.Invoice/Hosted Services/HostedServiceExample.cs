/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
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
using System.Text;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PdfSharp.Fonts;

namespace PdfDocuments.Example.Invoice
{
	/// <summary>
	/// Provides a sample implementation of a hosted service that demonstrates PDF generation using a model and manages
	/// application lifetime events.
	/// </summary>
	/// <remarks>This class is intended as an example or template for creating custom hosted services that perform
	/// background tasks during application startup. It demonstrates how to generate a PDF document asynchronously and
	/// signal application shutdown upon completion. To implement custom startup logic, derive from this class and override
	/// the OnStarted method. The class is not intended for production use without modification.</remarks>
	public class HostedServiceExample : HostedServiceTemplate
	{
		/// <summary>
		/// Initializes a new instance of the HostedServiceExample class with the specified application lifetime, logger, and
		/// service scope factory.
		/// </summary>
		/// <param name="hostApplicationLifetime">The application lifetime object that manages application startup and shutdown events.</param>
		/// <param name="logger">The logger used to record diagnostic messages for the hosted service.</param>
		/// <param name="serviceScopeFactory">The factory used to create service scopes for resolving scoped dependencies within the hosted service.</param>
		public HostedServiceExample(IHostApplicationLifetime hostApplicationLifetime, ILogger<HostedServiceExample> logger, IServiceScopeFactory serviceScopeFactory)
			: base(hostApplicationLifetime, logger, serviceScopeFactory)
		{
		}

		/// <summary>
		/// Handles initialization logic when the service starts by creating a sample invoice document and generating its PDF
		/// representation asynchronously.
		/// </summary>
		/// <remarks>This method is called automatically as part of the service startup sequence. It is intended for
		/// demonstration or testing purposes and should be overridden to implement custom startup behavior in derived
		/// classes. The method executes asynchronously and does not block the calling thread.</remarks>
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
				Items =
				[
					new () { Id = 10112, Quantity = 2321, UnitPrice = 1.43M },
					new () { Id = 10115, Quantity = 1211, UnitPrice = 5.411M },
					new () { Id = 10711, Quantity = 54, UnitPrice = 2.21M },
					new () { Id = 10112, Quantity = 6531, UnitPrice = 3.15M },
					new () { Id = 10912, Quantity = 252, UnitPrice = 10.82M },
					new () { Id = 10132, Quantity = 4321, UnitPrice = 11.45M }
				]
			};

			await this.CreatePdfAsync(model);
		}

		/// <summary>
		/// Generates a PDF document using the specified model and opens it for viewing.
		/// </summary>
		/// <remarks>This method configures font settings and encoding providers required for PDF generation. After
		/// creating and opening the PDF, the application is signaled to stop. The method should be called from an environment
		/// where application shutdown is appropriate after PDF generation.</remarks>
		/// <typeparam name="TModel">The type of the model used to generate the PDF document. Must implement the IPdfModel interface.</typeparam>
		/// <param name="model">The model containing the data to be rendered in the PDF document. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The result is always 0.</returns>
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
				// Set the font resolver. This is required to resolve the fonts used in the PDF document.
				//
				IFontResolver fontResolver = scope.ServiceProvider.GetService<IFontResolver>();
				GlobalFontSettings.FontResolver = fontResolver;

				//
				// Set the default font.
				//
				GlobalPdfDocumentsSettings.DefaultFontName = "Open Sans";

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

				this.HostApplicationLifetime.StopApplication();
			}

			return returnValue;
		}
	}
}
