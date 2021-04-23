/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfDocuments.Barcode;
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public static class PdfGridPageImageExtensions
	{
		public static void DrawImageWithFixedWidth(this IPdfGridPage source, string imageFile, int leftColumn, int topRow, int columns)
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

		public static void DrawImageWithFixedWidth(this IPdfGridPage source, XImage image, int leftColumn, int topRow)
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

		public static void DrawImageWithFixedHeight(this IPdfGridPage source, string imageFile, int leftColumn, int topRow, int rows)
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

		public static void DrawImage(this IPdfGridPage source, string imageFile, IPdfBounds bounds, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
		{
			using (XImage image = XImage.FromFile(imageFile))
			{
				source.DrawImage(image, bounds, horizontalAlignment, verticalAlignment);
			}
		}

		public static void DrawImage(this IPdfGridPage source, XImage image, IPdfBounds bounds, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
		{
			IPdfBounds imageBounds = new PdfBounds(0, 0, (int)(image.PointWidth / source.Grid.ColumnWidth), (int)(image.PointHeight / source.Grid.RowHeight));
			IPdfPoint hPoint = bounds.AlignHorizontally(imageBounds, horizontalAlignment);
			IPdfPoint vPoint = bounds.AlignVertically(imageBounds, verticalAlignment);

			imageBounds.LeftColumn = hPoint.Column;
			imageBounds.TopRow = vPoint.Row;

			double x = source.Grid.Left(hPoint.Column);
			double y = source.Grid.Top(vPoint.Row);
			XPoint xp = new XPoint() { X = x, Y = y };

			source.Graphics.DrawImage(image, xp);
		}

		public static void DrawBarCode(this IPdfGridPage source, BarCodeType type, IPdfBounds bounds, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, XColor color, XColor backColor, string data)
		{
			//
			// Get the height and width of the target image.
			//
			int pixelWidth = (int)source.Grid.ColumnsWidth(bounds.Columns);
			int pixelHeight = (int)source.Grid.RowsHeight(bounds.Rows);

			using (Image barcodeImage = source.BarcodeGenerator.Create(data, pixelWidth, pixelHeight, type, color.ToGdiColor(), backColor.ToGdiColor()))
			{
				using (MemoryStream stream = new MemoryStream())
				{
					barcodeImage.Save(stream, ImageFormat.Png);

					using (XImage pdfImage = XImage.FromStream(stream))
					{
						source.DrawImage(pdfImage, bounds, horizontalAlignment, verticalAlignment);
					}
				}
			}
		}
	}
}
