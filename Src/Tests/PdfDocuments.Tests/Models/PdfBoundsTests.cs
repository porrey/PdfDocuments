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
	public class PdfBoundsTests
	{
		[Fact]
		public void DefaultConstructor_CreatesInstanceWithZeroValues()
		{
			PdfBounds bounds = new();

			Assert.Equal(0, bounds.TopRow);
			Assert.Equal(0, bounds.LeftColumn);
			Assert.Equal(0, bounds.Rows);
			Assert.Equal(0, bounds.Columns);
		}

		[Fact]
		public void ParameterizedConstructor_SetsPropertiesCorrectly()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 5);

			Assert.Equal(2, bounds.TopRow);
			Assert.Equal(3, bounds.LeftColumn);
			Assert.Equal(5, bounds.Rows);
			Assert.Equal(10, bounds.Columns);
		}

		[Fact]
		public void RightColumn_IsLeftColumnPlusColumnsMinusOne()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 5);

			// RightColumn = LeftColumn + Columns - 1 = 3 + 10 - 1 = 12
			Assert.Equal(12, bounds.RightColumn);
		}

		[Fact]
		public void RightColumn_WhenLeftColumnIsZero_IsColumnsMinusOne()
		{
			PdfBounds bounds = new(leftColumn: 0, topRow: 0, columns: 8, rows: 4);

			Assert.Equal(7, bounds.RightColumn);
		}

		[Fact]
		public void BottomRow_IsTopRowPlusRowsMinusOne()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 5);

			// BottomRow = TopRow + Rows - 1 = 2 + 5 - 1 = 6
			Assert.Equal(6, bounds.BottomRow);
		}

		[Fact]
		public void BottomRow_WhenTopRowIsZero_IsRowsMinusOne()
		{
			PdfBounds bounds = new(leftColumn: 0, topRow: 0, columns: 8, rows: 6);

			Assert.Equal(5, bounds.BottomRow);
		}

		[Fact]
		public void ToString_ReturnsFormattedString()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 5);

			Assert.Equal("3, 2, 10, 5", bounds.ToString());
		}

		[Fact]
		public void Properties_CanBeSetIndividually()
		{
			PdfBounds bounds = new()
			{
				LeftColumn = 5,
				TopRow = 7,
				Columns = 20,
				Rows = 15
			};

			Assert.Equal(5, bounds.LeftColumn);
			Assert.Equal(7, bounds.TopRow);
			Assert.Equal(20, bounds.Columns);
			Assert.Equal(15, bounds.Rows);
		}
	}
}
