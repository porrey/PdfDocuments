﻿/*
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
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	public static class PdfGridPageTextExtensions
	{
		public static IPdfSize MeasureText(this IPdfGridPage source, string familyName, double emSize, XFontStyle style, string text = "Test")
		{
			IPdfSize returnValue = new PdfSize();

			XFont font = new XFont(familyName, emSize, style);
			returnValue = source.MeasureText(font, text);

			return returnValue;
		}

		public static IPdfSize MeasureText(this IPdfGridPage source, XFont font, string text = "Test")
		{
			PdfSize returnValue = new PdfSize();

			XSize pixelSize = source.Graphics.MeasureString(text ?? string.Empty, font);

			returnValue.Rows = (int)(pixelSize.Height / source.Grid.RowHeight);
			returnValue.Columns = (int)(pixelSize.Width / source.Grid.ColumnWidth);

			return returnValue;
		}

		public static IPdfSize DrawText(this IPdfGridPage source, string text, XFont font, IPdfBounds bounds, XStringFormat formats, XColor color, bool forceNoDebug = false)
		{
			IPdfSize returnValue = source.MeasureText(font, text);

			XRect layout = new XRect(source.Grid.Left(bounds.LeftColumn), source.Grid.Top(bounds.TopRow), source.Grid.ColumnsWidth(bounds.Columns), source.Grid.RowsHeight(bounds.Rows));
			XBrush brush = new XSolidBrush(color);
			source.Graphics.DrawString(text ?? string.Empty, font, brush, layout, formats);

			if (!forceNoDebug && source.DebugMode.HasFlag(DebugMode.RevealFontDetails))
			{
				XFont debugFont = source.DebugFont();
				IPdfSize textSize = source.MeasureText(debugFont, font.FontFamily.Name);
				IPdfBounds labelBounds = new PdfBounds(bounds.LeftColumn + (int)((bounds.Columns - textSize.Columns) / 2.0), bounds.TopRow + (int)((bounds.Rows - textSize.Rows) / 2.0), textSize.Columns + 2, textSize.Rows + 2);
				source.DrawFilledRectangle(labelBounds, XColors.Black);
				XRect labelLayout = new XRect(source.Grid.Left(labelBounds.LeftColumn), source.Grid.Top(labelBounds.TopRow), source.Grid.ColumnsWidth(labelBounds.Columns), source.Grid.RowsHeight(labelBounds.Rows));
				XBrush labelBrush = new XSolidBrush(XColors.Wheat);
				source.Graphics.DrawString(font.FontFamily.Name, debugFont, labelBrush, labelLayout, XStringFormats.Center);
			}

			return returnValue;
		}

		public static IPdfSize DrawText(this IPdfGridPage source, string text, string familyName, double emSize, XFontStyle style, IPdfBounds bounds, XStringFormat formats, XColor color)
		{
			XFont font = new XFont(familyName, emSize, style);
			return source.DrawText(text, font, bounds, formats, color);
		}

		public static IPdfSize DrawText(this IPdfGridPage source, string text, string familyName, double emSize, XFontStyle style, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color)
		{
			PdfBounds bounds = new PdfBounds(leftColumn, topRow, columns, rows);
			return source.DrawText(text, familyName, emSize, style, bounds, formats, color);
		}

		public static IPdfSize DrawText(this IPdfGridPage source, string text, XFont font, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color)
		{
			PdfBounds bounds = new PdfBounds(leftColumn, topRow, columns, rows);
			return source.DrawText(text, font, bounds, formats, color);
		}

		public static void DrawWrappingText(this IPdfGridPage source, string text, XFont font, IPdfBounds bounds, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default)
		{
			XRect layout = new XRect(source.Grid.Left(bounds.LeftColumn), source.Grid.Top(bounds.TopRow), source.Grid.ColumnsWidth(bounds.Columns), source.Grid.RowsHeight(bounds.Rows));

			XTextFormatter formatter = new XTextFormatter(source.Graphics)
			{
				Alignment = paragraphAlignment
			};

			XBrush brush = new XSolidBrush(color);
			formatter.DrawString(text, font, brush, layout, formats);
		}

		public static void DrawWrappingText(this IPdfGridPage source, string text, string familyName, double emSize, XFontStyle style, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default)
		{
			PdfBounds bounds = new PdfBounds(leftColumn, topRow, columns, rows);
			XFont font = new XFont(familyName, emSize, style);
			source.DrawWrappingText(text, font, bounds, formats, color, paragraphAlignment);
		}

		public static void DrawWrappingText(this IPdfGridPage source, string text, XFont font, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default)
		{
			PdfBounds bounds = new PdfBounds(leftColumn, topRow, columns, rows);
			source.DrawWrappingText(text, font, bounds, formats, color, paragraphAlignment);
		}

		public static IPdfSize ApplyPadding(this IPdfSize textSize, IPdfGridPage source, IPdfSpacing padding, bool usePadding)
		{
			IPdfSize returnValue = textSize;

			if (usePadding)
			{
				returnValue.Columns = textSize.Columns + padding.Left + padding.Right;
				returnValue.Rows = textSize.Rows + padding.Top + padding.Bottom;
			}

			return returnValue;
		}

		public static IPdfSpacing MultipyPadding(this IPdfSpacing padding, double multiplier)
		{
			IPdfSpacing returnValue = new PdfSpacing() { Left = padding.Left, Top = padding.Top, Right = padding.Right, Bottom = padding.Bottom };

			returnValue.Left = (int)(returnValue.Left * multiplier);
			returnValue.Top = (int)(returnValue.Top * multiplier);
			returnValue.Right = (int)(returnValue.Right * multiplier);
			returnValue.Bottom = (int)(returnValue.Bottom * multiplier);

			return returnValue;
		}
	}
}