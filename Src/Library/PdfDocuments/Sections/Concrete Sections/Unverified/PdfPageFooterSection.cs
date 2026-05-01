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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	/// <summary>
	/// Represents a footer section template for a PDF page, allowing customizable text content in each corner of the
	/// footer.
	/// </summary>
	/// <remarks>Use this class to define footer content for PDF pages, with support for binding text to the
	/// top-left, top-right, bottom-left, and bottom-right corners. Footer content and styles can be dynamically resolved
	/// based on the provided model. This section is typically rendered at the bottom of each PDF page and can be
	/// customized for different document layouts.</remarks>
	/// <typeparam name="TModel">The model type used to bind footer content and styles. Must implement the IPdfModel interface.</typeparam>
	public class PdfPageFooterSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the text displayed in the top-left area of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> TopLeftText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the top-right corner of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> TopRightText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the bottom-left area of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> BottomLeftText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the bottom-right corner of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> BottomRightText { get; set; } = string.Empty;

		/// <summary>
		/// Renders text content in each corner of the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>Text is rendered in the top-left, top-right, bottom-left, and bottom-right corners of the
		/// specified bounds. The style and padding are resolved from the model and applied to the rendering
		/// operation.</remarks>
		/// <param name="g">The PDF grid page on which the text will be rendered.</param>
		/// <param name="m">The model containing data used to resolve text and style information for rendering.</param>
		/// <param name="bounds">The bounds within which the text will be rendered on the page.</param>
		/// <returns>A task that represents the asynchronous render operation. The task result is <see langword="true"/> if rendering
		/// was successful; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfSpacing padding = style.Padding.Resolve(g, m);
			XFont font = style.Font.Resolve(g, m);
			PdfBounds textBounds = bounds.SubtractSpacing(g, m, padding);

			//
			// Top left.
			//
			g.DrawText(this.TopLeftText.Resolve(g, m), font, textBounds, XStringFormats.TopLeft, style.ForegroundColor.Resolve(g, m));

			//
			// Top right.
			//
			g.DrawText(this.TopRightText.Resolve(g, m), font, textBounds, XStringFormats.TopRight, style.ForegroundColor.Resolve(g, m));

			//
			// Bottom left
			//
			g.DrawText(this.BottomLeftText.Resolve(g, m), font, textBounds, XStringFormats.BottomLeft, style.ForegroundColor.Resolve(g, m));

			//
			// Bottom right.
			//
			g.DrawText(this.BottomRightText.Resolve(g, m), font, textBounds, XStringFormats.BottomRight, style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
