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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfDocuments.Example.Theme;
using PdfDocuments.IronBarcode;
using Serilog;

namespace PdfDocuments.Example
{
	public class ConsoleStartup : IStartupConfigureServices, IStartupAppConfiguration
	{
		public void ConfigureAppConfiguration(IConfigurationBuilder builder)
		{
			//
			// Build the configuration so Serilog can read from it.
			//
			IConfigurationRoot configuration = builder.Build();

			//
			// Create a logger from the configuration.
			//
			Log.Logger = new LoggerConfiguration()
					  .ReadFrom.Configuration(configuration)
					  .CreateLogger();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddExampleTheme()
					.AddPdfDocuments()
					.AddIronBarcodeSupport()
					.AddScoped<IPdfGenerator, InvoicePdf>()
					.AddHostedService<HostedServiceExample>();
		}
	}
}
