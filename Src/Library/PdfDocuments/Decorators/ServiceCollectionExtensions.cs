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
using Microsoft.Extensions.DependencyInjection;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for registering PDF-related services with an IServiceCollection.
	/// </summary>
	/// <remarks>These extension methods simplify the configuration of PDF generation and styling services in
	/// dependency injection containers. They are intended to be used during application startup to add required PDF
	/// services to the service collection.</remarks>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds PDF document generation services to the specified service collection.
		/// </summary>
		/// <remarks>Registers the <see cref="IPdfGeneratorFactory"/> service with a scoped lifetime. Call this method
		/// during application startup to enable PDF generation features.</remarks>
		/// <param name="services">The service collection to which the PDF document generation services will be added. Cannot be null.</param>
		/// <returns>The same instance of <see cref="IServiceCollection"/> that was provided, to support method chaining.</returns>
		public static IServiceCollection AddPdfDocuments(this IServiceCollection services)
		{
			services.AddTransient<IPdfGeneratorFactory, PdfGeneratorFactory>();
			return services;
		}

		/// <summary>
		/// Adds a scoped PDF style manager service for the specified model type to the service collection.
		/// </summary>
		/// <remarks>Registers IPdfStyleManager<![CDATA[<TModel>]]>; with its default implementation PdfStyleManager<![CDATA[<TModel>]]>; using
		/// scoped lifetime. Call this method during application startup to enable PDF style management for the specified
		/// model type.</remarks>
		/// <typeparam name="TModel">The type of PDF model for which the style manager is registered. Must implement the IPdfModel interface.</typeparam>
		/// <param name="services">The service collection to which the PDF style manager will be added.</param>
		/// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
		public static IServiceCollection AddPdfStyleManager<TModel>(this IServiceCollection services)
				where TModel : IPdfModel
		{
			services.AddTransient<IPdfStyleManager<TModel>, PdfStyleManager<TModel>>();
			return services;
		}

		/// <summary>
		/// Registers the specified PDF style manager as a scoped service for the given model type.
		/// </summary>
		/// <remarks>This method allows you to provide a custom PDF style manager instance for a specific model type,
		/// making it available for dependency injection within the application's scope.</remarks>
		/// <typeparam name="TModel">The type of the PDF model for which the style manager is registered. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="services">The service collection to which the PDF style manager will be added.</param>
		/// <param name="styleManager">The instance of <see cref="PdfStyleManager{TModel}"/> to register for dependency injection.</param>
		/// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
		public static IServiceCollection AddPdfStyleManager<TModel>(this IServiceCollection services, PdfStyleManager<TModel> styleManager)
				where TModel : IPdfModel
		{
			services.AddTransient<IPdfStyleManager<TModel>>((s) => styleManager);
			return services;
		}
	}
}
