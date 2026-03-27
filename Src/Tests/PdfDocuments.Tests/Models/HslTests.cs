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
using System.Drawing;
using Xunit;

namespace PdfDocuments.Tests.Models
{
	public class HslTests
	{
		[Fact]
		public void Constructor_SetsAllPropertiesCorrectly()
		{
			Hsl hsl = new(h: 120.0, s: 0.5, l: 0.75);

			Assert.Equal(120.0, hsl.H);
			Assert.Equal(0.5, hsl.S);
			Assert.Equal(0.75, hsl.L);
		}

		[Fact]
		public void Properties_CanBeModifiedAfterConstruction()
		{
			Hsl hsl = new(h: 0, s: 0, l: 0);
			hsl.H = 240.0;
			hsl.S = 1.0;
			hsl.L = 0.5;

			Assert.Equal(240.0, hsl.H);
			Assert.Equal(1.0, hsl.S);
			Assert.Equal(0.5, hsl.L);
		}

		[Fact]
		public void ToColor_Black_ReturnsBlack()
		{
			// H=0, S=0, L=0 -> black
			Hsl hsl = new(h: 0.0, s: 0.0, l: 0.0);

			Color color = hsl.ToColor();

			Assert.Equal(0, color.R);
			Assert.Equal(0, color.G);
			Assert.Equal(0, color.B);
		}

		[Fact]
		public void ToColor_White_ReturnsWhite()
		{
			// H=0, S=0, L=1 -> white
			Hsl hsl = new(h: 0.0, s: 0.0, l: 1.0);

			Color color = hsl.ToColor();

			Assert.Equal(255, color.R);
			Assert.Equal(255, color.G);
			Assert.Equal(255, color.B);
		}

		[Fact]
		public void ToColor_PureRed_ReturnsRed()
		{
			// H=0, S=1, L=0.5 -> pure red
			Hsl hsl = new(h: 0.0, s: 1.0, l: 0.5);

			Color color = hsl.ToColor();

			Assert.Equal(255, color.R);
			Assert.Equal(0, color.G);
			Assert.Equal(0, color.B);
		}

		[Fact]
		public void ToColor_PureGreen_ReturnsGreen()
		{
			// H=120, S=1, L=0.5 -> pure green
			Hsl hsl = new(h: 120.0, s: 1.0, l: 0.5);

			Color color = hsl.ToColor();

			Assert.Equal(0, color.R);
			Assert.Equal(255, color.G);
			Assert.Equal(0, color.B);
		}

		[Fact]
		public void ToColor_PureBlue_ReturnsBlue()
		{
			// H=240, S=1, L=0.5 -> pure blue
			Hsl hsl = new(h: 240.0, s: 1.0, l: 0.5);

			Color color = hsl.ToColor();

			Assert.Equal(0, color.R);
			Assert.Equal(0, color.G);
			Assert.Equal(255, color.B);
		}

		[Fact]
		public void ToColor_MidGray_ReturnsMidGray()
		{
			// H=0, S=0, L=0.5 -> mid gray (approximately 128,128,128)
			Hsl hsl = new(h: 0.0, s: 0.0, l: 0.5);

			Color color = hsl.ToColor();

			Assert.Equal(color.R, color.G);
			Assert.Equal(color.G, color.B);
			Assert.InRange(color.R, 126, 130);
		}

		[Fact]
		public void ToString_ReturnsFormattedString()
		{
			Hsl hsl = new(h: 120.0, s: 0.5, l: 0.75);

			string result = hsl.ToString();

			Assert.Contains("120", result);
			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}
	}
}
