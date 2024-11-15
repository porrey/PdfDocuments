﻿/*
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

namespace PdfDocuments
{
	public class PdfGrid
	{
		public PdfGrid(double width, double height, int rows, int columns)
		{
			this.Width = width;
			this.Height = height;
			this.Rows = rows;
			this.Columns = columns;
		}

		public PdfGrid(double width, double height, double xOffset, double yOffset, int rows, int columns)
		{
			this.Width = width;
			this.Height = height;
			this.XOffset = xOffset;
			this.YOffset = yOffset;
			this.Rows = rows;
			this.Columns = columns;
		}

		public double XOffset { get; set; }
		public double YOffset { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public int Rows { get; set; }
		public int Columns { get; set; }

		public virtual double Left(int columnIndex)
		{
			return this.XOffset + ((columnIndex - 1) * this.ColumnWidth);
		}

		public virtual double Right(int columnIndex)
		{
			return this.XOffset + (columnIndex * this.ColumnWidth);
		}

		public virtual double Top(int rowIndex)
		{
			return this.YOffset + ((rowIndex - 1) * this.RowHeight);
		}

		public virtual double Bottom(int rowIndex)
		{
			return this.YOffset + (rowIndex * this.RowHeight);
		}

		public virtual double ColumnWidth => this.Width / this.Columns;

		public virtual double ColumnsWidth(int columnCount)
		{
			return this.ColumnWidth * columnCount;
		}

		public virtual double RowHeight => this.Height / this.Rows;

		public virtual double RowsHeight(int rowCount)
		{
			return this.RowHeight * rowCount;
		}

		public virtual PdfBounds GetBounds()
		{
			return new PdfBounds(1, 1, this.Columns, this.Rows);
		}
	}
}
