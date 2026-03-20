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
	///	Defines a contract for PDF elements that can be measured and rendered using a specific model type.
	/// </summary>
	/// <remarks>Implementations of this interface are responsible for determining their size and rendering
	/// themselves onto a PDF grid page. The interface allows customization of appearance and layout through the provided
	/// style and model parameters.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for the PDF element. Must implement the IPdfModel interface.</typeparam>
	public interface IPdfElement<TModel> where TModel : IPdfModel
	{
		/// <summary>
		/// Calculates the required size for rendering the specified model within a PDF grid page using the provided style.
		/// </summary>
		/// <param name="g">The PDF grid page context in which the model will be measured. Determines layout constraints and available space.</param>
		/// <param name="m">The model to be measured. Represents the content whose size is to be calculated.</param>
		/// <param name="style">The style to apply when measuring the model. Influences formatting, spacing, and appearance.</param>
		/// <returns>A PdfSize object representing the width and height needed to render the model with the specified style on the
		/// given grid page.</returns>
		PdfSize Measure(PdfGridPage g, TModel m, PdfStyle<TModel> style);

		/// <summary>
		/// Renders the specified model onto a PDF grid page within the given bounds and using the provided style.
		/// </summary>
		/// <param name="g">The PDF grid page on which the model will be rendered.</param>
		/// <param name="m">The model data to be rendered onto the page.</param>
		/// <param name="bounds">The bounds within which the model will be rendered. Defines the area on the page for rendering.</param>
		/// <param name="style">The style to apply when rendering the model. Determines visual appearance such as formatting and layout.</param>
		/// <param name="state">An optional state object that can be used to pass additional information or context required for rendering.</param>
		void Render(PdfGridPage g, TModel m, PdfBounds bounds, PdfStyle<TModel> style, object state);
	}
}