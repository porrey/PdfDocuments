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
	/// Represents a PDF section that renders an image, using a data-bound path from the model.
	/// </summary>
	/// <remarks>This section draws an image left-aligned and vertically centered within the specified bounds, using
	/// the resolved image path from the model. The image is only rendered if the resolved path is not null, empty, or
	/// whitespace, and the file exists at the specified location.</remarks>
	/// <typeparam name="TModel">The type of the model used to provide data for the section. Must implement the IPdfModel interface.</typeparam>
	public class PdfImageSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the image source associated with the model.
		/// </summary>
		/// <remarks>The image source is typically a URL or file path that identifies the image to display for the
		/// model. The exact usage depends on the binding context and how the property is consumed in the
		/// application.</remarks>
		public virtual BindProperty<string, TModel> Image { get; set; } = string.Empty;

		/// <summary>
		/// Renders an image onto the specified PDF grid page using the provided model and layout bounds.
		/// </summary>
		/// <remarks>The image is rendered left-aligned and vertically centered within the specified bounds, using
		/// padding and style information resolved from the model. If the resolved image path is null, empty, or the file does
		/// not exist, no image is rendered.</remarks>
		/// <param name="g">The PDF grid page on which the image will be rendered.</param>
		/// <param name="m">The data model used to resolve image path and styling information.</param>
		/// <param name="bounds">The layout bounds that define the area within which the image is rendered.</param>
		/// <returns>A task that represents the asynchronous render operation. The result is true if rendering was attempted.</returns>
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
			// Draw the image left aligned and vertically centered.
			//
			string path = this.Image.Resolve(g, m);

			if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
			{
				g.DrawImageWithFixedHeight(path, bounds.LeftColumn + padding.Left, bounds.TopRow + padding.Top, bounds.Rows - (padding.Top + padding.Bottom));
			}

			return Task.FromResult(returnValue);
		}
	}
}
