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
	public class PdfSpacingTests
	{
		[Fact]
		public void DefaultConstructor_CreatesInstanceWithZeroValues()
		{
			PdfSpacing spacing = new();

			Assert.Equal(0, spacing.Left);
			Assert.Equal(0, spacing.Top);
			Assert.Equal(0, spacing.Right);
			Assert.Equal(0, spacing.Bottom);
		}

		[Fact]
		public void ParameterizedConstructor_SetsAllPropertiesCorrectly()
		{
			PdfSpacing spacing = new(left: 1, top: 2, right: 3, bottom: 4);

			Assert.Equal(1, spacing.Left);
			Assert.Equal(2, spacing.Top);
			Assert.Equal(3, spacing.Right);
			Assert.Equal(4, spacing.Bottom);
		}

		[Fact]
		public void ImplicitConversionFromTuple_ConvertsAllValuesCorrectly()
		{
			PdfSpacing spacing = (5, 6, 7, 8);

			Assert.Equal(5, spacing.Left);
			Assert.Equal(6, spacing.Top);
			Assert.Equal(7, spacing.Right);
			Assert.Equal(8, spacing.Bottom);
		}

		[Fact]
		public void ImplicitConversionFromTuple_WithZeroValues_ConvertsCorrectly()
		{
			PdfSpacing spacing = (0, 0, 0, 0);

			Assert.Equal(0, spacing.Left);
			Assert.Equal(0, spacing.Top);
			Assert.Equal(0, spacing.Right);
			Assert.Equal(0, spacing.Bottom);
		}

		[Fact]
		public void ToString_ReturnsFormattedString()
		{
			PdfSpacing spacing = new(left: 1, top: 2, right: 3, bottom: 4);

			Assert.Equal("1, 2, 3, 4", spacing.ToString());
		}

		[Fact]
		public void Properties_CanBeSetIndividually()
		{
			PdfSpacing spacing = new()
			{
				Left = 10,
				Top = 20,
				Right = 30,
				Bottom = 40
			};

			Assert.Equal(10, spacing.Left);
			Assert.Equal(20, spacing.Top);
			Assert.Equal(30, spacing.Right);
			Assert.Equal(40, spacing.Bottom);
		}
	}
}
