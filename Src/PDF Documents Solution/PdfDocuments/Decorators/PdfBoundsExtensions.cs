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

namespace PdfDocuments
{
	public static class PdfBoundsExtensions
	{
		public static PdfPoint AlignHorizontally(this PdfBounds outerBounds, PdfBounds innerBounds, PdfHorizontalAlignment alignment)
		{
			PdfPoint returnValue = new PdfPoint() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case PdfHorizontalAlignment.Left:
					returnValue.Column = outerBounds.LeftColumn;
					returnValue.Row = outerBounds.TopRow;
					break;
				case PdfHorizontalAlignment.Center:
					returnValue.Column = outerBounds.LeftColumn + (int)((outerBounds.Columns - innerBounds.Columns) / 2.0);
					returnValue.Row = outerBounds.TopRow;
					break;
				case PdfHorizontalAlignment.Right:
					returnValue.Column = outerBounds.RightColumn - innerBounds.Columns;
					returnValue.Row = outerBounds.TopRow;
					break;
			}

			return returnValue;
		}

		public static PdfPoint AlignVertically(this PdfBounds outerBounds, PdfBounds innerBounds, PdfVerticalAlignment alignment)
		{
			PdfPoint returnValue = new PdfPoint() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case PdfVerticalAlignment.Top:
					returnValue.Row = outerBounds.TopRow;
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case PdfVerticalAlignment.Center:
					returnValue.Row = outerBounds.TopRow + (int)((outerBounds.Rows - innerBounds.Rows) / 2.0);
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case PdfVerticalAlignment.Bottom:
					returnValue.Row = outerBounds.BottomRow - innerBounds.Rows;
					returnValue.Column = outerBounds.LeftColumn;
					break;
			}

			return returnValue;
		}

		public static PdfBounds WithTopRow(this PdfBounds bounds, int topRow)
		{
			bounds.TopRow = topRow;
			return bounds.Normalize();
		}

		public static PdfBounds WithLeftColumn(this PdfBounds bounds, int leftColumn)
		{
			bounds.TopRow = leftColumn;
			return bounds.Normalize();
		}

		public static PdfBounds Normalize(this PdfBounds bounds)
		{
			return new PdfBounds()
			{
				LeftColumn = bounds.LeftColumn >= 0 ? bounds.LeftColumn : 0,
				TopRow = bounds.TopRow >= 0 ? bounds.TopRow : 0,
				Columns = bounds.Columns > 0 ? bounds.Columns : 1,
				Rows = bounds.Rows > 0 ? bounds.Rows : 1
			};
		}
	}
}
