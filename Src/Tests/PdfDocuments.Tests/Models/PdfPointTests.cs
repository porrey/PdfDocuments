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
	public class PdfPointTests
	{
		[Fact]
		public void DefaultConstructor_CreatesInstanceWithZeroValues()
		{
			PdfPoint point = new();

			Assert.Equal(0, point.Column);
			Assert.Equal(0, point.Row);
		}

		[Fact]
		public void Column_CanBeSetAndRetrieved()
		{
			PdfPoint point = new() { Column = 7 };

			Assert.Equal(7, point.Column);
		}

		[Fact]
		public void Row_CanBeSetAndRetrieved()
		{
			PdfPoint point = new() { Row = 3 };

			Assert.Equal(3, point.Row);
		}

		[Fact]
		public void BothProperties_CanBeSetTogether()
		{
			PdfPoint point = new() { Column = 10, Row = 4 };

			Assert.Equal(10, point.Column);
			Assert.Equal(4, point.Row);
		}
	}
}
