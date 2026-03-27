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
	public class PdfSizeTests
	{
		[Fact]
		public void DefaultConstructor_CreatesInstanceWithZeroValues()
		{
			PdfSize size = new();

			Assert.Equal(0, size.Columns);
			Assert.Equal(0, size.Rows);
		}

		[Fact]
		public void Columns_CanBeSetAndRetrieved()
		{
			PdfSize size = new() { Columns = 12 };

			Assert.Equal(12, size.Columns);
		}

		[Fact]
		public void Rows_CanBeSetAndRetrieved()
		{
			PdfSize size = new() { Rows = 8 };

			Assert.Equal(8, size.Rows);
		}

		[Fact]
		public void BothProperties_CanBeSetTogether()
		{
			PdfSize size = new() { Columns = 24, Rows = 16 };

			Assert.Equal(24, size.Columns);
			Assert.Equal(16, size.Rows);
		}
	}
}
