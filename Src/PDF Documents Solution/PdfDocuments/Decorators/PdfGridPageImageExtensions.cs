/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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
	public static class PdfGridPageImageExtensions
	{
		public static void DrawImageWithFixedWidth(this PdfGridPage source, string imageFile, int leftColumn, int topRow, int columns)
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
				double targetedImageWidth = source.Grid.ColumnsWidth(columns);
				double targetedImageHeight = (targetedImageWidth / actualImageWidth) * actualImageHeight;

				//
				// Draw the image.
				//
				source.Graphics.DrawImage(image, source.Grid.Left(leftColumn), source.Grid.Top(topRow), targetedImageWidth, targetedImageHeight);
			}
		}

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

		public static void DrawImage(this PdfGridPage source, string imageFile, PdfBounds bounds, PdfHorizontalAlignment horizontalAlignment, PdfVerticalAlignment verticalAlignment)
		{
			using (XImage image = XImage.FromFile(imageFile))
			{
				source.DrawImage(image, bounds, horizontalAlignment, verticalAlignment);
			}
		}

		public static void DrawImage(this PdfGridPage source, XImage image, PdfBounds bounds, PdfHorizontalAlignment horizontalAlignment, PdfVerticalAlignment verticalAlignment)
		{
			PdfBounds imageBounds = new PdfBounds(0, 0, (int)(image.PointWidth / source.Grid.ColumnWidth), (int)(image.PointHeight / source.Grid.RowHeight));
			PdfPoint hPoint = bounds.AlignHorizontally(imageBounds, horizontalAlignment);
			PdfPoint vPoint = bounds.AlignVertically(imageBounds, verticalAlignment);

			imageBounds.LeftColumn = hPoint.Column;
			imageBounds.TopRow = vPoint.Row;

			double x = source.Grid.Left(hPoint.Column);
			double y = source.Grid.Top(vPoint.Row);
			XPoint xp = new XPoint() { X = x, Y = y };

			source.Graphics.DrawImage(image, xp);
		}
	}
}
