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
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfDocuments.FontResolver.Folder;
using Serilog;

namespace PdfDocuments.Example.Invoice
{
	/// <summary>
	/// Provides startup configuration for a console application, including application configuration and service
	/// registration.
	/// </summary>
	/// <remarks>Implements both IStartupConfigureServices and IStartupAppConfiguration to set up configuration
	/// sources and register services required for the application's operation. This class is typically used to initialize
	/// logging, dependency injection, and other infrastructure components at application startup.</remarks>
	public class ConsoleStartup : IStartupConfigureServices, IStartupAppConfiguration
	{
		/// <summary>
		/// Configures the application's configuration sources and initializes the Serilog logger using the provided
		/// configuration builder.
		/// </summary>
		/// <remarks>This method builds the configuration from the specified builder and sets up Serilog to read its
		/// settings from the resulting configuration. Call this method during application startup to ensure logging is
		/// configured according to the application's configuration sources.</remarks>
		/// <param name="builder">The configuration builder used to construct the application's configuration. Must not be null.</param>
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

		/// <summary>
		/// Configures application services required for PDF generation and font resolution.
		/// </summary>
		/// <remarks>This method registers services for PDF document generation, font resolution from a specified
		/// folder, and related hosted services. Call this method during application startup to ensure all required
		/// dependencies are available for PDF generation features.</remarks>
		/// <param name="services">The service collection to which application services are added. Must not be null.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			//
			// Add the Folder Font Resolver to the service collection. This will allow
			// the PdfGenerator to use the fonts installed on the system.
			//
			services.AddFolderFontResolver("./Fonts");

			//
			// Add the PdfDocuments services to the service collection. This will allow
			// us to generate PDF documents using the IPdfGenerator interface.
			//
			services.AddPdfDocuments()
					.AddScoped<IPdfGenerator, InvoicePdf>()
					.AddPdfStyleManager<Invoice>()
					.AddHostedService<HostedServiceExample>();
		}
	}
}
