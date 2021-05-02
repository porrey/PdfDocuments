using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PdfDocuments.Barcode
{
	public static class Extensions
	{
		public static void DrawBarCode(this PdfGridPage source, BarCodeType type, PdfBounds bounds, PdfHorizontalAlignment horizontalAlignment, PdfVerticalAlignment verticalAlignment, XColor color, XColor backColor, string data)
		{
			//
			// Get the height and width of the target image.
			//
			int pixelWidth = (int)source.Grid.ColumnsWidth(bounds.Columns);
			int pixelHeight = (int)source.Grid.RowsHeight(bounds.Rows);

			//using (Image barcodeImage = source.BarcodeGenerator.Create(data, pixelWidth, pixelHeight, type, color.ToGdiColor(), backColor.ToGdiColor()))
			//{
			//	using (MemoryStream stream = new MemoryStream())
			//	{
			//		barcodeImage.Save(stream, ImageFormat.Png);

			//		using (XImage pdfImage = XImage.FromStream(stream))
			//		{
			//			source.DrawImage(pdfImage, bounds, horizontalAlignment, verticalAlignment);
			//		}
			//	}
			//}
		}
	}
}
