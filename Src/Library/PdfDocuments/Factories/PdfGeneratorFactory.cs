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
	/// Provides a factory for creating PDF generator instances for specific document model types.
	/// </summary>
	/// <remarks>Use this factory to obtain an appropriate PDF generator implementation for a given document model
	/// type. The factory relies on dependency injection to resolve available PDF generators. If no generator is registered
	/// for the requested model type, an exception is thrown.</remarks>
	public class PdfGeneratorFactory : IPdfGeneratorFactory
	{
		/// <summary>
		/// Initializes a new instance of the PdfGeneratorFactory class using the specified service provider.
		/// </summary>
		/// <param name="serviceProvider">The service provider used to resolve dependencies required by the factory. Cannot be null.</param>
		public PdfGeneratorFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Gets or sets the service provider used to resolve service dependencies within the class.
		/// </summary>
		/// <remarks>Assigning a custom service provider allows for advanced dependency injection scenarios. The
		/// property is typically used to access services required by the class or its derived types.</remarks>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Asynchronously retrieves a PDF generator instance for the specified document model type.
		/// </summary>
		/// <remarks>This method uses dependency injection to locate a PDF generator registered for the given model
		/// type. Ensure that a generator for the desired type is registered in the service provider before calling this
		/// method.</remarks>
		/// <typeparam name="TModel">The type of document model for which to obtain a PDF generator. Must implement IPdfModel.</typeparam>
		/// <returns>A task that represents the asynchronous operation. The task result contains an IPdfGenerator instance for the
		/// specified model type.</returns>
		/// <exception cref="Exception">Thrown if a PDF generator for the specified document model type is not found.</exception>
		public virtual Task<IPdfGenerator<TModel>> GetAsync<TModel>()
			where TModel : IPdfModel
		{
			IPdfGenerator<TModel> returnValue = null;

			IEnumerable<IPdfGenerator> items = this.ServiceProvider.GetRequiredService<IEnumerable<IPdfGenerator>>();
			IPdfGenerator item = items.Where(t => t.DocumentModelType == typeof(TModel)).SingleOrDefault();

			if (item != null)
			{
				returnValue = item as IPdfGenerator<TModel>;
			}
			else
			{
				throw new Exception($"A PDF generator for document type '{typeof(TModel).Name}' was not found.");
			}

			return Task.FromResult(returnValue);
		}
	}
}
