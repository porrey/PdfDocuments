﻿/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PdfDocuments.Example
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
			Invoice model = new() { Id = "12345678" };
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
				//
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
									//.SetFlag(DebugMode.RevealGrid)
									//.SetFlag(DebugMode.RevealLayout)
									//.SetFlag(DebugMode.HideDetails)
									//.SetFlag(DebugMode.RevealFontDetails)
									;
#endif

				//
				// Save and open the PDF.
				//
				this.SaveAndOpenPdfAsync(generator, model);
			}

			return returnValue;
		}

		protected async void SaveAndOpenPdfAsync<TModel>(IPdfGenerator<TModel> generator, TModel model)
			where TModel : IPdfModel
		{
			this.Logger.LogInformation("Creating PDF.");

			(bool result, byte[] fileData) = await generator.CreatePdfAsync(model);

			if (result)
			{
				string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\{model.GetType().Name}[{model.Id}].pdf";
				File.WriteAllBytes(fileName, fileData);
				this.Logger.LogInformation($"Saved PDF to '{fileName}'.");

				try
				{
					this.Logger.LogInformation($"Opening PDF.");

					using (Process.Start(new ProcessStartInfo()
					{
						UseShellExecute = true,
						FileName = fileName
					}))
					{
						this.Logger.LogInformation($"PDF opened successfully.");
						this.HostApplicationLifetime.StopApplication();
					}
				}
				catch (Exception ex)
				{
					this.Logger.LogError(ex, $"Exception");
				}
			}
		}
	}
}