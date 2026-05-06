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
	/// Represents an image element that can be rendered within a PDF grid, using a specified model type for data binding
	/// and styling.
	/// </summary>
	/// <remarks>Use this class to add images to a PDF document as part of a grid layout. The image is loaded from
	/// the specified file path and rendered according to the provided style and layout information. Ensure that the file
	/// path points to a valid image file accessible at runtime.</remarks>
	/// <typeparam name="TModel">The type of the model used for data binding and style resolution. Must implement the IPdfModel interface.</typeparam>
	public class PdfImageElement<TModel> : IPdfElement<TModel> where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfImageElement class with the specified image file path.
		/// </summary>
		/// <param name="path">The file system path to the image to be embedded in the PDF. Cannot be null or empty.</param>
		public PdfImageElement(string path)
		{
			this.Path = path;
		}

		/// <summary>
		/// Gets or sets the file system path associated with this instance.
		/// </summary>
		public virtual string Path { get; set; }

		/// <summary>
		/// Asynchronously measures the rendered size of the text content, including margins and padding, using the specified
		/// style and model on the given PDF grid page.
		/// </summary>
		/// <remarks>The measured size accounts for the text, as well as any margin, padding, and cell padding defined
		/// in the style. This method is useful for determining layout requirements before rendering content to a PDF
		/// grid.</remarks>
		/// <param name="g">The PDF grid page context used for measurement calculations.</param>
		/// <param name="m">The data model that provides context for resolving style properties.</param>
		/// <param name="style">The style to apply when measuring the text, including font, margin, and padding settings.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a PdfSize structure indicating the
		/// total measured size, including all applicable margins and padding.</returns>
		public virtual Task<PdfSize> MeasureAsync(PdfGridPage g, TModel m, PdfStyle<TModel> style)
		{
			PdfSize returnValue = new();

			//
			// Add the cell padding
			//
			PdfSpacing cellPaddng = style.CellPadding.Resolve(g, m);
			returnValue.Columns += cellPaddng.Left + cellPaddng.Right;
			returnValue.Rows += cellPaddng.Top + cellPaddng.Bottom;

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Asynchronously renders an image onto the specified PDF grid page using the provided model, bounds, and style.
		/// </summary>
		/// <remarks>If a valid image path is specified and the file exists, the image is drawn centered within the
		/// specified bounds. No action is taken if the path is null, empty, or the file does not exist.</remarks>
		/// <param name="g">The PDF grid page on which the image will be rendered.</param>
		/// <param name="m">The model containing data relevant to the rendering operation.</param>
		/// <param name="bounds">The bounds within which the image will be drawn on the page.</param>
		/// <param name="style">The style information to apply when rendering the image.</param>
		/// <param name="state">An optional user-defined object that contains state information for the rendering operation. May be null.</param>
		/// <returns>A task that represents the asynchronous rendering operation.</returns>
		public virtual Task RenderAsync(PdfGridPage g, TModel m, PdfBounds bounds, PdfStyle<TModel> style, object state = null)
		{
			if (!string.IsNullOrWhiteSpace(this.Path) && File.Exists(this.Path))
			{
				//
				// Determine the horizontal and vertical alignment for the image based on
				// the style settings, resolving any bindings
				//
				PdfHorizontalAlignment horizontalAlignment = style.HorizontalImageAlignment.Resolve(g, m);
				PdfVerticalAlignment verticalAlignment = style.VerticalImageAlignment.Resolve(g, m);

				//
				// Get the image opacity.
				//
				float opacity = style.ImageOpacity.Resolve(g, m);

				//
				// Add the cell padding
				//
				PdfSpacing cellPaddng = style.CellPadding.Resolve(g, m);
				PdfBounds imageBounds = bounds.SubtractSpacing(g, m, cellPaddng);

				//
				// Get the image scale factor based on the style settings, resolving any bindings. This determines how the image
				// will be scaled during rendering.
				//
				float scale = style.ImageScale.Resolve(g, m);

				//
				//Determine if the image should be clipped.
				//
				bool clipDrawing = style.ClipDrawing.Resolve(g, m, state);

				using (PdfTransparentImage image = PdfTransparentImage.FromFile(this.Path, opacity))
				{
					//
					// Draw the image within the calculated bounds, applying the specified alignments.
					//
					g.DrawImage(image.XImage, imageBounds, horizontalAlignment, verticalAlignment, scale, clipDrawing);
				}
			}

			return Task.CompletedTask;
		}
	}
}
