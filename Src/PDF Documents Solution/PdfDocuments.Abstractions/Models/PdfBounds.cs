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
	public class PdfBounds
	{
		public PdfBounds()
		{
		}

		public PdfBounds(int leftColumn, int topRow, int columns, int rows)
		{
			this.TopRow = topRow;
			this.LeftColumn = leftColumn;
			this.Rows = rows;
			this.Columns = columns;
		}

		public int TopRow { get; set; }
		public int LeftColumn { get; set; }
		public int Rows { get; set; }
		public int Columns { get; set; }

		public int RightColumn => this.LeftColumn + this.Columns - 1;
		public int BottomRow => this.TopRow + this.Rows - 1;

		public override string ToString()
		{
			return $"{this.LeftColumn}, {this.TopRow}, {this.Columns}, {this.Rows}";
		}
	}
}
