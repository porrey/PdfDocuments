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

namespace PdfDocuments
{
	public enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	}

	public enum VerticalAlignment
	{
		Top,
		Center,
		Bottom
	}

	public static class PdfBoundsExtensions
	{
		public static IPdfPoint AlignHorizontally(this IPdfBounds outerBounds, IPdfBounds innerBounds, HorizontalAlignment alignment)
		{
			PdfPoint returnValue = new PdfPoint() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case HorizontalAlignment.Left:
					returnValue.Column = outerBounds.LeftColumn;
					returnValue.Row = outerBounds.TopRow;
					break;
				case HorizontalAlignment.Center:
					returnValue.Column = outerBounds.LeftColumn + (int)((outerBounds.Columns - innerBounds.Columns) / 2.0);
					returnValue.Row = outerBounds.TopRow;
					break;
				case HorizontalAlignment.Right:
					returnValue.Column = outerBounds.RightColumn - innerBounds.Columns;
					returnValue.Row = outerBounds.TopRow;
					break;
			}

			return returnValue;
		}

		public static IPdfPoint AlignVertically(this IPdfBounds outerBounds, IPdfBounds innerBounds, VerticalAlignment alignment)
		{
			PdfPoint returnValue = new PdfPoint() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case VerticalAlignment.Top:
					returnValue.Row = outerBounds.TopRow;
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case VerticalAlignment.Center:
					returnValue.Row = outerBounds.TopRow + (int)((outerBounds.Rows - innerBounds.Rows) / 2.0);
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case VerticalAlignment.Bottom:
					returnValue.Row = outerBounds.BottomRow - innerBounds.Rows;
					returnValue.Column = outerBounds.LeftColumn;
					break;
			}

			return returnValue;
		}

		public static IPdfBounds WithTopRow(this IPdfBounds bounds, int topRow)
		{
			bounds.TopRow = topRow;
			return bounds;
		}

		public static IPdfBounds WithLeftColumn(this IPdfBounds bounds, int leftColumn)
		{
			bounds.TopRow = leftColumn;
			return bounds;
		}

		public static IPdfBounds ApplyMargins(this IPdfBounds bounds, IPdfGridPage gridPage, IPdfSpacing spacing)
		{
			IPdfBounds returnValue = bounds;

			if (bounds.Columns > (spacing.Left + spacing.Right) &&
				bounds.Rows > (spacing.Top + spacing.Bottom))
			{
				//
				// Don't apply a margin to an item aligned to the left edge.
				//
				int left = bounds.LeftColumn > 1 ? bounds.LeftColumn + spacing.Left : 1;

				//
				// Don't apply a margin to an item aligned to the top edge.
				//
				int top = bounds.TopRow > 1 ? bounds.TopRow + spacing.Top : 1;

				//
				// Don't apply a margin to an item aligned to the right edge.
				//
				int columns = bounds.Columns - spacing.Left - (bounds.RightColumn == gridPage.Grid.Columns ?  0 : spacing.Right);

				//
				// Don't apply a margin to an item aligned to the bottom edge.
				//
				int rows = bounds.Rows - (bounds.BottomRow == gridPage.Grid.Rows ? 0 : spacing.Top) - spacing.Bottom;

				returnValue = new PdfBounds(left, top, columns, rows);
			}

			return returnValue;
		}
	}
}
