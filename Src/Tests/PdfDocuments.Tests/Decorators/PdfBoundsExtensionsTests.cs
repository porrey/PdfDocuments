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

namespace PdfDocuments.Tests.Decorators
{
	public class PdfBoundsExtensionsTests
	{
		// ─── AlignHorizontally ───────────────────────────────────────────────────────

		[Fact]
		public void AlignHorizontally_Left_SetsColumnToOuterLeftColumn()
		{
			PdfBounds outer = new(leftColumn: 5, topRow: 2, columns: 20, rows: 10);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 4);

			PdfPoint result = outer.AlignHorizontally(inner, PdfHorizontalAlignment.Left);

			Assert.Equal(5, result.Column);
			Assert.Equal(2, result.Row);
		}

		[Fact]
		public void AlignHorizontally_Center_CentersInnerWithinOuter()
		{
			PdfBounds outer = new(leftColumn: 0, topRow: 0, columns: 20, rows: 10);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 4);

			PdfPoint result = outer.AlignHorizontally(inner, PdfHorizontalAlignment.Center);

			// Center: LeftColumn + (20 - 8) / 2 = 0 + 6 = 6
			Assert.Equal(6, result.Column);
			Assert.Equal(0, result.Row);
		}

		[Fact]
		public void AlignHorizontally_Right_AlignsInnerToRightEdgeOfOuter()
		{
			// outer: leftColumn=0, columns=20, so RightColumn = 0+20-1 = 19
			PdfBounds outer = new(leftColumn: 0, topRow: 0, columns: 20, rows: 10);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 4);

			PdfPoint result = outer.AlignHorizontally(inner, PdfHorizontalAlignment.Right);

			// Right: RightColumn - innerColumns = 19 - 8 = 11
			Assert.Equal(11, result.Column);
			Assert.Equal(0, result.Row);
		}

		// ─── AlignVertically ─────────────────────────────────────────────────────────

		[Fact]
		public void AlignVertically_Top_SetsRowToOuterTopRow()
		{
			PdfBounds outer = new(leftColumn: 0, topRow: 5, columns: 20, rows: 10);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 4);

			PdfPoint result = outer.AlignVertically(inner, PdfVerticalAlignment.Top);

			Assert.Equal(5, result.Row);
			Assert.Equal(0, result.Column);
		}

		[Fact]
		public void AlignVertically_Center_CentersInnerVertically()
		{
			PdfBounds outer = new(leftColumn: 0, topRow: 0, columns: 20, rows: 20);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 6);

			PdfPoint result = outer.AlignVertically(inner, PdfVerticalAlignment.Center);

			// Center: TopRow + (20 - 6) / 2 = 0 + 7 = 7
			Assert.Equal(7, result.Row);
			Assert.Equal(0, result.Column);
		}

		[Fact]
		public void AlignVertically_Bottom_AlignsInnerToBottomOfOuter()
		{
			// outer: topRow=0, rows=20, BottomRow = 0+20-1 = 19
			PdfBounds outer = new(leftColumn: 0, topRow: 0, columns: 20, rows: 20);
			PdfBounds inner = new(leftColumn: 0, topRow: 0, columns: 8, rows: 6);

			PdfPoint result = outer.AlignVertically(inner, PdfVerticalAlignment.Bottom);

			// Bottom: BottomRow - innerRows = 19 - 6 = 13
			Assert.Equal(13, result.Row);
			Assert.Equal(0, result.Column);
		}

		// ─── WithTopRow ──────────────────────────────────────────────────────────────

		[Fact]
		public void WithTopRow_SetsTopRowAndReturnsBounds()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 0, columns: 10, rows: 5);

			PdfBounds result = bounds.WithTopRow(4);

			Assert.Equal(4, result.TopRow);
			Assert.Equal(10, result.Columns);
			Assert.Equal(5, result.Rows);
		}

		[Fact]
		public void WithTopRow_NegativeValue_NormalizesToZero()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 5, columns: 10, rows: 5);

			PdfBounds result = bounds.WithTopRow(-1);

			Assert.Equal(0, result.TopRow);
		}

		// ─── WithLeftColumn ──────────────────────────────────────────────────────────

		[Fact]
		public void WithLeftColumn_SetsLeftColumnAndReturnsBounds()
		{
			PdfBounds bounds = new(leftColumn: 0, topRow: 3, columns: 10, rows: 5);

			PdfBounds result = bounds.WithLeftColumn(7);

			Assert.Equal(7, result.LeftColumn);
			Assert.Equal(10, result.Columns);
			Assert.Equal(5, result.Rows);
		}

		// ─── Normalize ───────────────────────────────────────────────────────────────

		[Fact]
		public void Normalize_ValidBounds_ReturnsUnchanged()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 5);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(3, result.LeftColumn);
			Assert.Equal(2, result.TopRow);
			Assert.Equal(10, result.Columns);
			Assert.Equal(5, result.Rows);
		}

		[Fact]
		public void Normalize_NegativeLeftColumn_SetsToZero()
		{
			PdfBounds bounds = new(leftColumn: -5, topRow: 2, columns: 10, rows: 5);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(0, result.LeftColumn);
		}

		[Fact]
		public void Normalize_NegativeTopRow_SetsToZero()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: -3, columns: 10, rows: 5);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(0, result.TopRow);
		}

		[Fact]
		public void Normalize_ZeroColumns_SetsToOne()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 0, rows: 5);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(1, result.Columns);
		}

		[Fact]
		public void Normalize_NegativeColumns_SetsToOne()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: -2, rows: 5);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(1, result.Columns);
		}

		[Fact]
		public void Normalize_ZeroRows_SetsToOne()
		{
			PdfBounds bounds = new(leftColumn: 3, topRow: 2, columns: 10, rows: 0);

			PdfBounds result = bounds.Normalize();

			Assert.Equal(1, result.Rows);
		}

		// ─── SubtractBounds ──────────────────────────────────────────────────────────

		[Fact]
		public void SubtractBounds_ReducesBoundsBySpacing()
		{
			PdfBounds outer = new(leftColumn: 5, topRow: 5, columns: 20, rows: 10);
			PdfSpacing spacing = new(left: 2, top: 1, right: 2, bottom: 1);

			PdfBounds result = outer.SubtractBounds<NullModel>(g: null!, m: null!, spacing);

			// left = 5 + 2 = 7
			// top = 5 + 1 = 6
			// columns = 20 - (2+2) = 16
			// rows = 10 - (1+1) = 8
			Assert.Equal(7, result.LeftColumn);
			Assert.Equal(6, result.TopRow);
			Assert.Equal(16, result.Columns);
			Assert.Equal(8, result.Rows);
		}

		[Fact]
		public void SubtractBounds_ZeroSpacing_ReturnsSameBounds()
		{
			PdfBounds outer = new(leftColumn: 5, topRow: 5, columns: 20, rows: 10);
			PdfSpacing spacing = new(left: 0, top: 0, right: 0, bottom: 0);

			PdfBounds result = outer.SubtractBounds<NullModel>(g: null!, m: null!, spacing);

			Assert.Equal(5, result.LeftColumn);
			Assert.Equal(5, result.TopRow);
			Assert.Equal(20, result.Columns);
			Assert.Equal(10, result.Rows);
		}

		// ─── AddBounds ───────────────────────────────────────────────────────────────

		[Fact]
		public void AddBounds_ExpandsBoundsWithSpacing()
		{
			PdfBounds outer = new(leftColumn: 5, topRow: 5, columns: 20, rows: 10);
			PdfSpacing spacing = new(left: 2, top: 1, right: 2, bottom: 1);

			PdfBounds result = outer.AddBounds<NullModel>(g: null!, m: null!, spacing);

			// left = 5 - 2 = 3
			// top = 5 - 1 = 4
			// columns = 20 + (2+2) = 24
			// rows = 10 + (1+1) = 12
			Assert.Equal(3, result.LeftColumn);
			Assert.Equal(4, result.TopRow);
			Assert.Equal(24, result.Columns);
			Assert.Equal(12, result.Rows);
		}

		[Fact]
		public void AddBounds_ZeroSpacing_ReturnsSameBounds()
		{
			PdfBounds outer = new(leftColumn: 5, topRow: 5, columns: 20, rows: 10);
			PdfSpacing spacing = new(left: 0, top: 0, right: 0, bottom: 0);

			PdfBounds result = outer.AddBounds<NullModel>(g: null!, m: null!, spacing);

			Assert.Equal(5, result.LeftColumn);
			Assert.Equal(5, result.TopRow);
			Assert.Equal(20, result.Columns);
			Assert.Equal(10, result.Rows);
		}
	}
}
