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
namespace PdfDocuments
{
	/// <summary>
	/// Defines a factory for creating PDF generator instances for a specified model type.
	/// </summary>
	/// <remarks>Use this interface to obtain PDF generators that support custom model types implementing the
	/// IPdfModel interface. The factory abstracts the creation logic, allowing for flexible generator instantiation and
	/// dependency management.</remarks>
	public interface IPdfGeneratorFactory
	{
		/// <summary>
		/// Asynchronously retrieves a PDF generator instance for the specified model type.
		/// </summary>
		/// <typeparam name="TModel">The type of model for which the PDF generator is created. Must implement the IPdfModel interface.</typeparam>
		/// <returns>A task that represents the asynchronous operation. The task result contains an IPdfGenerator instance for the
		/// specified model type.</returns>
		Task<IPdfGenerator<TModel>> GetAsync<TModel>() where TModel : IPdfModel;
	}
}
