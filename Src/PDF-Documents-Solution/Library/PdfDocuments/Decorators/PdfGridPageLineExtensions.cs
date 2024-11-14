/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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
	public enum ColumnEdge
	{
		Left,
		Right
	}

	public enum RowEdge
	{
		Top,
		Bottom
	}

	public static class PdfGridPageLineExtensions
	{
		public static void DrawFilledRectangle(this PdfGridPage source, PdfBounds bounds, XColor color)
		{
			source.DrawFilledRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, color);
		}

		public static void DrawFilledRectangle(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow, XColor color)
		{
			//
			// Create the rectangle.
			//
			XRect rect = new XRect(source.Grid.Left(leftColumn), source.Grid.Top(topRow), source.Grid.Right(rightColumn) - source.Grid.Left(leftColumn), source.Grid.Bottom(bottomRow) - source.Grid.Top(topRow));

			//
			// Draw the rectangle.
			//
			source.Graphics.DrawRectangle(new XPen(color, 1), rect);
			source.Graphics.DrawRectangle(new XSolidBrush(color), rect);
		}

		public static void DrawRectangle(this PdfGridPage source, PdfBounds bounds, XPen pen)
		{
			source.DrawRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, pen);
		}

		public static void DrawRectangle(this PdfGridPage source, PdfBounds bounds, double weight, XColor color)
		{
			XPen pen = new XPen(color, weight);
			source.DrawRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, pen);
		}

		public static void DrawRectangle(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow, XPen pen)
		{
			//
			// Create the rectangle.
			//
			XRect rect = source.GetRect(leftColumn, topRow, rightColumn, bottomRow);

			if (!rect.IsEmpty)
			{
				//
				// Draw the rectangle.
				//
				source.Graphics.DrawRectangle(pen, rect);
			}
		}

		public static void DrawVerticalLine(this PdfGridPage source, int column, int startRow, int endRow, ColumnEdge columnEdge, double weight, XColor color)
		{
			XPen linePen = new XPen(color, weight);
			XPoint p1 = new XPoint(columnEdge == ColumnEdge.Right ? source.Grid.Right(column) : source.Grid.Left(column), source.Grid.Top(startRow));
			XPoint p2 = new XPoint(columnEdge == ColumnEdge.Right ? source.Grid.Right(column) : source.Grid.Left(column), source.Grid.Bottom(endRow));
			source.Graphics.DrawLine(linePen, p1, p2);
		}

		public static void DrawHorizontalLine(this PdfGridPage source, int row, int startColumn, int endColumn, RowEdge rowEdge, double weight, XColor color)
		{
			XPen linePen = new XPen(color, weight);
			XPoint p1 = new XPoint(source.Grid.Left(startColumn), rowEdge == RowEdge.Top ? source.Grid.Top(row) : source.Grid.Bottom(row));
			XPoint p2 = new XPoint(source.Grid.Right(endColumn), rowEdge == RowEdge.Top ? source.Grid.Top(row) : source.Grid.Bottom(row));
			source.Graphics.DrawLine(linePen, p1, p2);
		}

		public static void DrawGrid(this PdfGridPage source, XColor color, double weight)
		{
			for (int row = 1; row <= source.Grid.Rows; row++)
			{
				source.DrawHorizontalLine(row, 1, source.Grid.Columns, RowEdge.Top, weight, color);
			}

			source.DrawHorizontalLine(source.Grid.Rows, 1, source.Grid.Columns, RowEdge.Bottom, weight, color);

			for (int column = 1; column <= source.Grid.Columns; column++)
			{
				source.DrawVerticalLine(column, 1, source.Grid.Rows, ColumnEdge.Left, weight, color);
			}

			source.DrawVerticalLine(source.Grid.Columns, 1, source.Grid.Rows, ColumnEdge.Right, weight, color);
		}

		public static XRect GetRect(this PdfGridPage source, PdfBounds bounds)
		{
			//
			// Create the rectangle.
			//
			double x = source.Grid.Left(bounds.LeftColumn);
			double y = source.Grid.Top(bounds.TopRow);
			double w = source.Grid.ColumnsWidth(bounds.Columns) > 0 ? source.Grid.ColumnsWidth(bounds.Columns) : 1;
			double h = source.Grid.RowsHeight(bounds.Rows) > 0 ? source.Grid.RowsHeight(bounds.Rows) : 1;

			return new XRect(x, y, w, h);
		}

		public static XRect GetRect(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow)
		{
			//
			// Create the rectangle.
			//
			double x = source.Grid.Left(leftColumn);
			double y = source.Grid.Top(topRow);
			double w = source.Grid.Right(rightColumn) - x > 0 ? source.Grid.Right(rightColumn) - x : 1;
			double h = source.Grid.Bottom(bottomRow) - y > 0 ? source.Grid.Bottom(bottomRow) - y : 1;

			return new XRect(x, y, w, h);
		}
	}
}
