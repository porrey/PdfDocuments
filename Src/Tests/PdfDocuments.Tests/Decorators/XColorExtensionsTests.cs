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
using PdfSharp.Drawing;
using Xunit;

namespace PdfDocuments.Tests.Decorators
{
	public class XColorExtensionsTests
	{
		// ─── Contrast ────────────────────────────────────────────────────────────────

		[Fact]
		public void Contrast_SameColor_ReturnsZero()
		{
			XColor color = XColor.FromArgb(128, 128, 128);

			double result = color.Contrast(color);

			Assert.Equal(0.0, result, precision: 5);
		}

		[Fact]
		public void Contrast_BlackAndWhite_ReturnsMaxContrast()
		{
			XColor black = XColor.FromArgb(0, 0, 0);
			XColor white = XColor.FromArgb(255, 255, 255);

			double result = black.Contrast(white);

			Assert.True(result > 200, $"Expected high contrast but got {result}");
		}

		[Fact]
		public void Contrast_IsSymmetric()
		{
			XColor color1 = XColor.FromArgb(50, 100, 150);
			XColor color2 = XColor.FromArgb(200, 80, 30);

			double forward = color1.Contrast(color2);
			double reverse = color2.Contrast(color1);

			Assert.Equal(forward, reverse, precision: 5);
		}

		// ─── RandomColor ─────────────────────────────────────────────────────────────

		[Fact]
		public void RandomColor_ReturnsColorWithFullAlpha()
		{
			XColor color = XColorExtensions.RandomColor();

			// Alpha is stored as 0-255 when set, but as 0.0-1.0 in the XColor struct
			Assert.InRange(color.A, 0.0, 1.0);
		}

		[Fact]
		public void RandomColor_CalledMultipleTimes_ReturnsValidColors()
		{
			for (int i = 0; i < 10; i++)
			{
				XColor color = XColorExtensions.RandomColor();
				Assert.InRange((int)color.R, 0, 255);
				Assert.InRange((int)color.G, 0, 255);
				Assert.InRange((int)color.B, 0, 255);
			}
		}

		// ─── ToGdiColor ──────────────────────────────────────────────────────────────

		[Fact]
		public void ToGdiColor_PreservesRgbComponents()
		{
			XColor xColor = XColor.FromArgb(100, 150, 200);

			Color gdiColor = xColor.ToGdiColor();

			Assert.Equal(100, gdiColor.R);
			Assert.Equal(150, gdiColor.G);
			Assert.Equal(200, gdiColor.B);
		}

		// ─── ToXColor (from GDI Color) ────────────────────────────────────────────────

		[Fact]
		public void ToXColor_FromGdiColor_PreservesAllComponents()
		{
			Color gdiColor = Color.FromArgb(255, 100, 150, 200);

			XColor xColor = gdiColor.ToXColor();

			Assert.Equal((byte)100, xColor.R);
			Assert.Equal((byte)150, xColor.G);
			Assert.Equal((byte)200, xColor.B);
		}

		// ─── IsNearTo ────────────────────────────────────────────────────────────────

		[Fact]
		public void IsNearTo_ExactMatch_ReturnsTrue()
		{
			double value = 0.5;

			Assert.True(value.IsNearTo(0.5));
		}

		[Fact]
		public void IsNearTo_WithinDefaultTolerance_ReturnsTrue()
		{
			double value = 0.5;

			// Difference = 0.0005 < 1/10^3 = 0.001
			Assert.True(value.IsNearTo(0.5005, digits: 3));
		}

		[Fact]
		public void IsNearTo_OutsideDefaultTolerance_ReturnsFalse()
		{
			double value = 0.5;

			// Difference = 0.002 > 1/10^3 = 0.001
			Assert.False(value.IsNearTo(0.502, digits: 3));
		}

		[Fact]
		public void IsNearTo_WithHigherPrecision_ReturnsFalseForSmallDifference()
		{
			double value = 1.0;

			// Difference = 0.0001 is within 3-digit tolerance but outside 5-digit tolerance
			Assert.False(value.IsNearTo(1.0002, digits: 5));
		}

		[Fact]
		public void IsNearTo_NegativeAndPositive_SymmetricComparison()
		{
			Assert.True(0.5001.IsNearTo(0.5, digits: 3));
			Assert.True(0.5.IsNearTo(0.5001, digits: 3));
		}

		// ─── ToXColor (from hex string) ───────────────────────────────────────────────

		[Fact]
		public void ToXColor_SixCharHexWithoutHash_ConvertsCorrectly()
		{
			XColor color = "FF0000".ToXColor();

			Assert.Equal((byte)255, color.R);
			Assert.Equal((byte)0, color.G);
			Assert.Equal((byte)0, color.B);
		}

		[Fact]
		public void ToXColor_SixCharHexWithHash_ConvertsCorrectly()
		{
			XColor color = "#00FF00".ToXColor();

			Assert.Equal((byte)0, color.R);
			Assert.Equal((byte)255, color.G);
			Assert.Equal((byte)0, color.B);
		}

		[Fact]
		public void ToXColor_SixCharBlueHex_ConvertsCorrectly()
		{
			XColor color = "#0000FF".ToXColor();

			Assert.Equal((byte)0, color.R);
			Assert.Equal((byte)0, color.G);
			Assert.Equal((byte)255, color.B);
		}

		[Fact]
		public void ToXColor_EightCharHex_ParsesAlphaAndRgb()
		{
			// AARRGGBB = FF112233
			XColor color = "#FF112233".ToXColor();

			Assert.Equal((byte)0x11, color.R);
			Assert.Equal((byte)0x22, color.G);
			Assert.Equal((byte)0x33, color.B);
		}

		[Fact]
		public void ToXColor_NullInput_ThrowsArgumentNullException()
		{
			string? hexColor = null;

			Assert.Throws<ArgumentNullException>(() => hexColor!.ToXColor());
		}

		[Fact]
		public void ToXColor_EmptyString_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => "".ToXColor());
		}

		[Fact]
		public void ToXColor_WhitespaceString_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => "   ".ToXColor());
		}

		[Fact]
		public void ToXColor_InvalidLengthHex_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => "#12345".ToXColor());
		}

		// ─── ToHsl ───────────────────────────────────────────────────────────────────

		[Fact]
		public void ToHsl_BlackColor_ReturnsZeroLightness()
		{
			Color black = Color.FromArgb(0, 0, 0);

			Hsl hsl = black.ToHsl();

			Assert.Equal(0.0, hsl.L, precision: 5);
		}

		[Fact]
		public void ToHsl_WhiteColor_ReturnsOneLightness()
		{
			Color white = Color.FromArgb(255, 255, 255);

			Hsl hsl = white.ToHsl();

			Assert.Equal(1.0, hsl.L, precision: 5);
		}

		[Fact]
		public void ToHsl_PureRed_ReturnsHueNearZero()
		{
			Color red = Color.FromArgb(255, 0, 0);

			Hsl hsl = red.ToHsl();

			// Hue for pure red is 0 degrees
			Assert.True(hsl.H.IsNearTo(0.0, digits: 0) || hsl.H.IsNearTo(360.0, digits: 0));
			Assert.True(hsl.S > 0.9);
		}

		[Fact]
		public void ToHsl_PureGreen_ReturnsHueNear120()
		{
			Color green = Color.FromArgb(0, 255, 0);

			Hsl hsl = green.ToHsl();

			Assert.True(hsl.H.IsNearTo(120.0, digits: 0));
		}

		[Fact]
		public void ToHsl_PureBlue_ReturnsHueNear240()
		{
			Color blue = Color.FromArgb(0, 0, 255);

			Hsl hsl = blue.ToHsl();

			Assert.True(hsl.H.IsNearTo(240.0, digits: 0));
		}

		[Fact]
		public void ToHsl_GrayColor_ReturnsZeroSaturation()
		{
			Color gray = Color.FromArgb(128, 128, 128);

			Hsl hsl = gray.ToHsl();

			Assert.Equal(0.0, hsl.S, precision: 5);
		}

		// ─── WithLuminosity ───────────────────────────────────────────────────────────

		[Fact]
		public void WithLuminosity_LowValue_DarkensColor()
		{
			XColor original = XColor.FromArgb(100, 150, 200);
			XColor darker = original.WithLuminosity(0.1);

			// Darkened color should have lower brightness
			Assert.True(darker.GetBrightness() < original.GetBrightness());
		}

		[Fact]
		public void WithLuminosity_HighValue_LightensColor()
		{
			XColor original = XColor.FromArgb(100, 50, 50);
			XColor lighter = original.WithLuminosity(0.9);

			// Lightened color should have higher brightness
			Assert.True(lighter.GetBrightness() > original.GetBrightness());
		}
	}
}
