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
	/// Defines a contract for managing named PDF styles associated with a model type. Provides methods to retrieve and
	/// replace styles by name.
	/// </summary>
	/// <remarks>Implementations of this interface allow for organized storage and retrieval of PDF styles by name.
	/// The interface extends IDictionary, enabling standard collection operations alongside specialized style management
	/// methods.</remarks>
	/// <typeparam name="TModel">The model type associated with each PDF style. Must implement the IPdfModel interface.</typeparam>
	public interface IPdfStyleManager<TModel> : IDictionary<string, PdfStyle<TModel>>
			where TModel : IPdfModel
	{
		/// <summary>
		/// Retrieves the style associated with the specified name.
		/// </summary>
		/// <param name="name">The name of the style to retrieve. Cannot be null or empty.</param>
		/// <returns>A <see cref="PdfStyle{TModel}"/> instance representing the style with the specified name, or null if no matching
		/// style is found.</returns>
		PdfStyle<TModel> GetStyle(string name);

		/// <summary>
		/// Replaces the style associated with the specified name with the provided style.
		/// </summary>
		/// <param name="name">The name of the style to replace. Cannot be null or empty.</param>
		/// <param name="style">The new style to associate with the specified name. Cannot be null.</param>
		void Replace(string name, PdfStyle<TModel> style);
	}
}
