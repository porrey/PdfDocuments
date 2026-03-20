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
	/// Represents a PDF section that renders a block of text using a specified model.
	/// </summary>
	/// <remarks>Use PdfTextBlockSection<![CDATA[<TModel>]]> to display formatted text within a PDF document section. This class
	/// is intended for scenarios where text content is dynamically resolved from the provided model and rendered with
	/// customizable styles. Background and border drawing are not supported for this section.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for the text block. Must implement the IPdfModel interface.</typeparam>
	public class PdfTextBlockSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Asynchronously renders the text element onto the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <param name="g">The PDF grid page on which the text element will be rendered.</param>
		/// <param name="m">The model containing data used to resolve and render the text element.</param>
		/// <param name="bounds">The bounds within the page that define the area for rendering the text element.</param>
		/// <returns>A task that represents the asynchronous render operation. The result is <see langword="true"/> if rendering was
		/// successful; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfTextElement<TModel> textElement = new PdfTextElement<TModel>(this.Text.Resolve(g, m));
			textElement.Render(g, m, bounds, style);

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Determines whether the background should be drawn for the current element.
		/// </summary>
		/// <returns>Always returns <see langword="false"/>, indicating that the background will not be drawn.</returns>
		protected override bool OnShouldDrawBackground()
		{
			return false;
		}

		/// <summary>
		/// Determines whether a border should be drawn for the control.
		/// </summary>
		/// <remarks>Override this method in a derived class to enable border drawing if required.</remarks>
		/// <returns>Always returns <see langword="false"/>, indicating that no border will be drawn.</returns>
		protected override bool OnShouldDrawBorder()
		{
			return false;
		}
	}
}
