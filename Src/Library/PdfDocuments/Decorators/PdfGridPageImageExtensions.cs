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
	/// Provides extension methods for drawing images onto a PDF grid page with fixed dimensions or alignment options.
	/// </summary>
	/// <remarks>These extension methods enable rendering images within a grid-based layout on PDF pages, supporting
	/// both fixed width and height scaling, as well as alignment within specified bounds. Methods accept either image file
	/// paths or preloaded image objects, allowing flexibility in image sourcing. All methods require a valid grid page
	/// context and appropriate grid coordinates or bounds for placement. Thread safety is not guaranteed; ensure proper
	/// synchronization if used in multi-threaded scenarios.</remarks>
	public static class PdfGridPageImageExtensions
	{
		/// <summary>
		/// Draws an image onto the PDF grid page, scaling it to fit a specified number of columns starting at the given grid
		/// position.
		/// </summary>
		/// <remarks>The image is scaled proportionally to fit the specified column width, preserving its aspect
		/// ratio. The top-left corner of the image aligns with the specified grid cell. If the image file is invalid or
		/// cannot be loaded, an exception may be thrown.</remarks>
		/// <param name="source">The PDF grid page on which the image will be drawn.</param>
		/// <param name="imageFile">The file path of the image to be rendered. Must reference a valid image file.</param>
		/// <param name="leftColumn">The index of the leftmost column where the image will be placed. Must be within the grid's column range.</param>
		/// <param name="topRow">The index of the top row where the image will be placed. Must be within the grid's row range.</param>
		/// <param name="columns">The number of columns the image should span horizontally. Must be positive and not exceed the grid's column count.</param>
		public static void DrawImageWithFixedWidth(this PdfGridPage source, string imageFile, int leftColumn, int topRow, int columns)
		{
			//
			// Draw the image.
			//
			using (XImage image = XImage.FromFile(imageFile))
			{
				XSize resolution = source.Page.GetPageResolution();
				double actualImageWidth = (image.PixelWidth * resolution.Width / image.HorizontalResolution);
				double actualImageHeight = (image.PixelHeight * resolution.Height / image.VerticalResolution);

				//
				// Resize the image to fit into the top three grid units.
				//
				double targetedImageWidth = source.Grid.ColumnsWidth(columns);
				double targetedImageHeight = (targetedImageWidth / actualImageWidth) * actualImageHeight;

				//
				// Draw the image.
				//
				source.Graphics.DrawImage(image, source.Grid.Left(leftColumn), source.Grid.Top(topRow), targetedImageWidth, targetedImageHeight);
			}
		}

		/// <summary>
		/// Draws the specified image onto the PDF grid page at the given column and row, scaling the image to match the
		/// page's resolution and maintaining its original width.
		/// </summary>
		/// <remarks>The image is scaled based on the page's resolution to ensure its width remains consistent with
		/// its original pixel width. The height is adjusted proportionally to preserve the image's aspect ratio.</remarks>
		/// <param name="source">The PDF grid page on which the image will be drawn.</param>
		/// <param name="image">The image to draw on the grid page.</param>
		/// <param name="leftColumn">The column index in the grid where the left edge of the image will be placed.</param>
		/// <param name="topRow">The row index in the grid where the top edge of the image will be placed.</param>
		public static void DrawImageWithFixedWidth(this PdfGridPage source, XImage image, int leftColumn, int topRow)
		{
			//
			// Draw the image.
			//
			XSize resolution = source.Page.GetPageResolution();
			double actualImageWidth = (image.PixelWidth * resolution.Width / image.HorizontalResolution);
			double actualImageHeight = (image.PixelHeight * resolution.Height / image.VerticalResolution);

			//
			// Draw the image.
			//
			source.Graphics.DrawImage(image, source.Grid.Left(leftColumn), source.Grid.Top(topRow), actualImageWidth, actualImageHeight);
		}

		/// <summary>
		/// Draws an image onto the PDF grid page, scaling it to fit a specified number of grid rows while maintaining its
		/// aspect ratio.
		/// </summary>
		/// <remarks>The image is scaled proportionally so its height matches the specified number of grid rows. The
		/// width is adjusted to preserve the original aspect ratio. The image is placed at the specified grid location. If
		/// the image file is invalid or cannot be loaded, an exception may be thrown.</remarks>
		/// <param name="source">The PDF grid page on which the image will be drawn.</param>
		/// <param name="imageFile">The file path of the image to be rendered. Must reference a valid image file.</param>
		/// <param name="leftColumn">The index of the leftmost grid column where the image will be positioned.</param>
		/// <param name="topRow">The index of the topmost grid row where the image will be positioned.</param>
		/// <param name="rows">The number of grid rows the image's height should span. Must be positive.</param>
		public static void DrawImageWithFixedHeight(this PdfGridPage source, string imageFile, int leftColumn, int topRow, int rows)
		{
			//
			// Draw the logo.
			//
			using (XImage image = XImage.FromFile(imageFile))
			{
				XSize resolution = source.Page.GetPageResolution();
				double actualImageWidth = (image.PixelWidth * resolution.Width / image.HorizontalResolution);
				double actualImageHeight = (image.PixelHeight * resolution.Height / image.VerticalResolution);

				//
				// Resize the image to fit into the top three grid units.
				//
				double targetedImageHeight = source.Grid.RowsHeight(rows);
				double targetedImageWidth = (targetedImageHeight / actualImageHeight) * actualImageWidth;

				//
				// Draw the image.
				//
				source.Graphics.DrawImage(image, source.Grid.Left(leftColumn), source.Grid.Top(topRow), targetedImageWidth, targetedImageHeight);
			}
		}

		/// <summary>
		/// Draws an image onto the specified PDF page within the given bounds, aligning it according to the provided
		/// horizontal and vertical alignment options.
		/// </summary>
		/// <param name="source">The PDF page on which the image will be drawn.</param>
		/// <param name="imageFile">The path to the image file to be rendered on the PDF page. Must be a valid file path to an image supported by the
		/// library.</param>
		/// <param name="bounds">The rectangular area, in PDF coordinates, where the image will be placed.</param>
		/// <param name="horizontalAlignment">Specifies how the image is aligned horizontally within the bounds.</param>
		/// <param name="verticalAlignment">Specifies how the image is aligned vertically within the bounds.</param>
		/// <param name="clipDrawing">Indicates whether the drawing should be clipped to the specified bounds.</param>
		public static void DrawImage(this PdfGridPage source, string imageFile, PdfBounds bounds, PdfHorizontalAlignment horizontalAlignment, PdfVerticalAlignment verticalAlignment, bool clipDrawing)
		{
			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				if (clipDrawing)
				{
					source.ClipDrawing(bounds);
				}

				using (XImage image = XImage.FromFile(imageFile))
				{
					source.DrawImage(image, bounds, horizontalAlignment, verticalAlignment);
				}
			}
			finally
			{
				source.Graphics.Restore(state);
			}
		}

		/// <summary>
		/// Draws the specified image onto the PDF grid page, aligning it within the given bounds according to the specified
		/// horizontal and vertical alignment.
		/// </summary>
		/// <remarks>The image is scaled to fit the grid cell dimensions and positioned based on the alignment
		/// parameters. Use this method to place images precisely within a grid layout on a PDF page.</remarks>
		/// <param name="source">The PDF grid page on which the image will be drawn.</param>
		/// <param name="image">The image to render onto the grid page.</param>
		/// <param name="bounds">The bounds within the grid page that define the area for image placement and alignment.</param>
		/// <param name="horizontalAlignment">Specifies how the image is aligned horizontally within the provided bounds.</param>
		/// <param name="verticalAlignment">Specifies how the image is aligned vertically within the provided bounds.</param>
		/// <param name="scale">An optional scaling factor to apply to the image size. Default is 1.0 (no scaling).</param>
		/// <param name="clipDrawing">Indicates whether the drawing should be clipped to the specified bounds.</param>
		public static void DrawImage(this PdfGridPage source, XImage image, PdfBounds bounds, PdfHorizontalAlignment horizontalAlignment, PdfVerticalAlignment verticalAlignment, float scale = 1.0f, bool clipDrawing = true)
		{
			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				int imageWidthInColumns = (int)((image.PointWidth * scale) / source.Grid.ColumnWidth);
				int imageHeightInRows = (int)((image.PointHeight * scale) / source.Grid.RowHeight);

				PdfBounds imageBounds = new(bounds.LeftColumn, bounds.TopRow, imageWidthInColumns, imageHeightInRows);

				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				if (clipDrawing)
				{
					source.ClipDrawing(imageBounds);
				}

				PdfPoint hPoint = bounds.AlignHorizontally(imageBounds, horizontalAlignment);
				PdfPoint vPoint = bounds.AlignVertically(imageBounds, verticalAlignment);
				XPoint xp = source.Grid.GetXPoint(hPoint.Column, vPoint.Row);

				source.Graphics.DrawImage(image, xp.X, xp.Y, image.PointWidth * scale, image.PointHeight * scale);
			}
			finally
			{
				source.Graphics.Restore(state);
			}
		}
	}
}
