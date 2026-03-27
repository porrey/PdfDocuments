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
using Xunit;

namespace PdfDocuments.Tests.Models
{
	public class PdfGridTests
	{
		[Fact]
		public void Constructor_FourParams_SetsPropertiesCorrectly()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			Assert.Equal(600.0, grid.Width);
			Assert.Equal(800.0, grid.Height);
			Assert.Equal(10, grid.Rows);
			Assert.Equal(12, grid.Columns);
			Assert.Equal(0.0, grid.XOffset);
			Assert.Equal(0.0, grid.YOffset);
		}

		[Fact]
		public void Constructor_SixParams_SetsAllPropertiesCorrectly()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			Assert.Equal(600.0, grid.Width);
			Assert.Equal(800.0, grid.Height);
			Assert.Equal(20.0, grid.XOffset);
			Assert.Equal(30.0, grid.YOffset);
			Assert.Equal(10, grid.Rows);
			Assert.Equal(12, grid.Columns);
		}

		[Fact]
		public void ColumnWidth_IsWidthDividedByColumns()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			Assert.Equal(50.0, grid.ColumnWidth);
		}

		[Fact]
		public void RowHeight_IsHeightDividedByRows()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			Assert.Equal(80.0, grid.RowHeight);
		}

		[Fact]
		public void Left_ForFirstColumn_ReturnsXOffset()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Left(1) = XOffset + (1 - 1) * ColumnWidth = 20 + 0 = 20
			Assert.Equal(20.0, grid.Left(1));
		}

		[Fact]
		public void Left_ForSecondColumn_ReturnsXOffsetPlusColumnWidth()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Left(2) = XOffset + (2 - 1) * ColumnWidth = 20 + 50 = 70
			Assert.Equal(70.0, grid.Left(2));
		}

		[Fact]
		public void Right_ForFirstColumn_ReturnsXOffsetPlusColumnWidth()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Right(1) = XOffset + 1 * ColumnWidth = 20 + 50 = 70
			Assert.Equal(70.0, grid.Right(1));
		}

		[Fact]
		public void Right_ForLastColumn_ReturnsXOffsetPlusWidth()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Right(12) = XOffset + 12 * ColumnWidth = 20 + 600 = 620
			Assert.Equal(620.0, grid.Right(12));
		}

		[Fact]
		public void Top_ForFirstRow_ReturnsYOffset()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Top(1) = YOffset + (1 - 1) * RowHeight = 30 + 0 = 30
			Assert.Equal(30.0, grid.Top(1));
		}

		[Fact]
		public void Top_ForSecondRow_ReturnsYOffsetPlusRowHeight()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Top(2) = YOffset + (2 - 1) * RowHeight = 30 + 80 = 110
			Assert.Equal(110.0, grid.Top(2));
		}

		[Fact]
		public void Bottom_ForFirstRow_ReturnsYOffsetPlusRowHeight()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Bottom(1) = YOffset + 1 * RowHeight = 30 + 80 = 110
			Assert.Equal(110.0, grid.Bottom(1));
		}

		[Fact]
		public void Bottom_ForLastRow_ReturnsYOffsetPlusHeight()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, xOffset: 20.0, yOffset: 30.0, rows: 10, columns: 12);

			// Bottom(10) = YOffset + 10 * RowHeight = 30 + 800 = 830
			Assert.Equal(830.0, grid.Bottom(10));
		}

		[Fact]
		public void ColumnsWidth_MultipliesColumnWidthByCount()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			// ColumnsWidth(3) = ColumnWidth * 3 = 50 * 3 = 150
			Assert.Equal(150.0, grid.ColumnsWidth(3));
		}

		[Fact]
		public void RowsHeight_MultipliesRowHeightByCount()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			// RowsHeight(4) = RowHeight * 4 = 80 * 4 = 320
			Assert.Equal(320.0, grid.RowsHeight(4));
		}

		[Fact]
		public void GetBounds_ReturnsGridBoundsStartingAtOneOne()
		{
			PdfGrid grid = new(width: 600.0, height: 800.0, rows: 10, columns: 12);

			PdfBounds bounds = grid.GetBounds();

			Assert.Equal(1, bounds.LeftColumn);
			Assert.Equal(1, bounds.TopRow);
			Assert.Equal(12, bounds.Columns);
			Assert.Equal(10, bounds.Rows);
		}

		[Fact]
		public void WithNoOffset_Left_StartsAtZero()
		{
			PdfGrid grid = new(width: 500.0, height: 400.0, rows: 4, columns: 5);

			// Left(1) = 0 + (1-1) * 100 = 0
			Assert.Equal(0.0, grid.Left(1));
		}
	}
}
