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
	/// Represents a header section for a PDF, supporting customizable logo and title rendering using a model-driven
	/// template.
	/// </summary>
	/// <remarks>Use this class to define and render a report header in a PDF document, with support for binding logo
	/// and title values from the provided model. The header section can be styled and positioned according to template
	/// settings. This class is typically used as part of a larger PDF generation workflow.</remarks>
	/// <typeparam name="TModel">The type of model used to bind data for the header section. Must implement the IPdfModel interface.</typeparam>
	public class PdfReportHeaderSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the logo text associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> Logo { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the title associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> Title { get; set; } = string.Empty;

		/// <summary>
		/// Renders the header section of the PDF grid page asynchronously, including the logo image and title text if
		/// available.
		/// </summary>
		/// <remarks>The logo image is rendered left-aligned and vertically centered with a margin, if a valid image
		/// path is provided. The title text is rendered using the resolved style and alignment. The method does not throw
		/// exceptions for missing or invalid logo paths.</remarks>
		/// <param name="g">The PDF grid page on which the header will be rendered.</param>
		/// <param name="m">The data model used to resolve header content and styles.</param>
		/// <param name="bounds">The bounds within which the header content should be rendered.</param>
		/// <returns>A task that represents the asynchronous render operation. The result is <see langword="true"/> if rendering was
		/// successful; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the first style for this section
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the default style for this section
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);

			//
			// Draw the image left aligned and vertically centered in the
			// header leaving a 1 row margin above and below it. Also 
			// leave a one column margin on the left.
			//
			string path = this.Logo.Resolve(g, m);

			if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
			{
				g.DrawImageWithFixedHeight(path, bounds.LeftColumn + padding.Left, bounds.TopRow + padding.Top, bounds.Rows - (padding.Top + padding.Bottom));
			}

			if (this.Title != null)
			{
				string title = this.Title.Resolve(g, m);
				PdfBounds textBounds = bounds.SubtractSpacing(g, m, padding);
				g.DrawText(title, style.Font.Resolve(g, m), textBounds, style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));
			}

			return Task.FromResult(returnValue);
		}
	}
}
