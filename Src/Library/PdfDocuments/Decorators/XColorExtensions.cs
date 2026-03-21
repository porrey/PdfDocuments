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

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for working with XColor and related color types, including conversions, color
	/// manipulation, and utility functions.
	/// </summary>
	/// <remarks>This static class offers methods to convert between XColor and System.Drawing.Color, generate
	/// random colors, calculate color contrast, and perform color space transformations. The methods are intended to
	/// simplify common color operations when working with XColor and related types.</remarks>
	public static class XColorExtensions
	{
		private static Random Rnd { get; } = new Random();

		/// <summary>
		/// Calculates the contrast value between two colors based on their brightness.
		/// </summary>
		/// <remarks>The contrast is determined by the difference in brightness between the two colors, scaled to a
		/// 0–255 range. This method can be used to assess the visual distinction between foreground and background
		/// colors.</remarks>
		/// <param name="color1">The first color to compare. Represents one of the two colors for which the contrast is calculated.</param>
		/// <param name="color2">The second color to compare. Represents the other color for which the contrast is calculated.</param>
		/// <returns>A double value representing the absolute difference in brightness between the two colors. Higher values indicate
		/// greater contrast.</returns>
		public static double Contrast(this XColor color1, XColor color2)
		{
			//
			// Calculate the contrast between the label color and the background color.
			//
			double cb1 = color1.GetBrightness() * 255;
			double cb2 = color2.GetBrightness() * 255;
			return Math.Abs(cb1 - cb2);
		}

		/// <summary>
		/// Generates a random color with full opacity.
		/// </summary>
		/// <remarks>Each call produces a different color. The alpha channel is always set to fully opaque.</remarks>
		/// <returns>A new XColor instance with random red, green, and blue channel values and an alpha value of 255.</returns>
		public static XColor RandomColor()
		{
			//
			// Create a random color
			//
			return new XColor() { A = 255, R = (byte)Rnd.Next(0, 255), G = (byte)Rnd.Next(0, 255), B = (byte)Rnd.Next(0, 255) };
		}

		/// <summary>
		/// Converts an XColor structure to a System.Drawing.Color structure.
		/// </summary>
		/// <remarks>The alpha component of the XColor is scaled from a 0.0–1.0 range to a 0–255 range in the
		/// resulting System.Drawing.Color.</remarks>
		/// <param name="color">The XColor value to convert to a System.Drawing.Color.</param>
		/// <returns>A System.Drawing.Color structure that represents the equivalent color of the specified XColor.</returns>
		public static Color ToGdiColor(this XColor color)
		{
			return Color.FromArgb((int)(color.A * 255), color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts a System.Drawing.Color instance to an XColor instance with the same ARGB values.
		/// </summary>
		/// <param name="color">The System.Drawing.Color to convert to an XColor.</param>
		/// <returns>An XColor instance that represents the same color as the specified System.Drawing.Color.</returns>
		public static XColor ToXColor(this Color color)
		{
			return XColor.FromArgb(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified RGB color to its equivalent HSL (Hue, Saturation, Lightness) representation.
		/// </summary>
		/// <remarks>The returned HSL values are calculated according to the standard RGB-to-HSL conversion algorithm.
		/// Hue is expressed in degrees (0–360), while saturation and lightness are in the range 0–1. This method is useful
		/// for color manipulation scenarios where HSL representation is preferred over RGB.</remarks>
		/// <param name="color">The color to convert. The RGB components are used to calculate the HSL values.</param>
		/// <returns>An Hsl structure representing the hue, saturation, and lightness values corresponding to the input color.</returns>
		public static Hsl ToHsl(this Color color)
		{
			double[] percentage = [color.R / 255.0d, color.G / 255.0d, color.B / 255.0d];

			double min = percentage.Min();
			double max = percentage.Max();

			double delta = max - min;

			double l = (max + min) / 2.0d;
			double s;
			double h;

			if (max > 0.0d)
			{
				if (l < 0.5d)
				{
					s = delta / (max + min);
				}
				else
				{
					s = delta / (2 - max - min);
				}
			}
			else
			{
				s = 0;
			}

			if (Math.Abs(percentage[0] - percentage.Max()) < 0.01)
			{
				h = (percentage[1] - percentage[2]) / delta;
			}
			else if (Math.Abs(percentage[1] - percentage.Max()) < 0.01)
			{
				h = 2 + (percentage[2] - percentage[0]) / delta;
			}
			else
			{
				h = 4 + (percentage[0] - percentage[1]) / delta;
			}

			h *= 60;

			if (h < 0.0d)
			{
				h += 360.0d;
			}

			h = double.IsNaN(h) ? 0.0d : h;
			s = double.IsNaN(s) ? 0.0d : s;

			return new Hsl(h, s, l);
		}

		/// <summary>
		/// Determines whether the specified double-precision floating-point value is near to the current value within a given
		/// number of decimal digits.
		/// </summary>
		/// <remarks>This method is useful for comparing floating-point values where exact equality is not required
		/// due to potential rounding errors. It is commonly used in scenarios where small differences between values are
		/// acceptable.</remarks>
		/// <param name="x">The current double-precision floating-point value to compare.</param>
		/// <param name="y">The double-precision floating-point value to compare against.</param>
		/// <param name="digits">The number of decimal digits to use for the comparison. Must be non-negative. The comparison considers the values
		/// near if their difference is less than 1 divided by 10 to the power of this value. The default is 3.</param>
		/// <returns>true if the two values are within the specified number of decimal digits of each other; otherwise, false.</returns>
		public static bool IsNearTo(this double x, double y, int digits = 3)
		{
			double difference = 1.0d / Math.Pow(10, digits);

			return Math.Abs(x - y) < difference;
		}

		/// <summary>
		/// Returns a new color based on the specified color with its luminosity component set to the specified value.
		/// </summary>
		/// <remarks>This method does not modify the original color. The resulting color may be clipped if the
		/// luminosity value is outside the valid range.</remarks>
		/// <param name="color">The source color to modify.</param>
		/// <param name="luminosity">The luminosity value to set, typically in the range 0.0 (black) to 1.0 (white).</param>
		/// <returns>A new XColor instance with the same hue and saturation as the original color, but with the specified luminosity.</returns>
		public static XColor WithLuminosity(this XColor color, double luminosity)
		{
			Color c = color.ToGdiColor();
			Hsl hsl = c.ToHsl();
			hsl.L = luminosity;
			return hsl.ToColor().ToXColor();
		}

		/// <summary>
		/// Converts a hexadecimal color string to an equivalent XColor instance.
		/// </summary>
		/// <remarks>If the input string contains six hexadecimal digits, the color is interpreted as opaque (alpha
		/// value of 255). If eight digits are provided, the first two represent the alpha channel.</remarks>
		/// <param name="hexColor">A string representing the color in hexadecimal format. The string may optionally start with a '#' and must be in
		/// either RRGGBB or AARRGGBB format.</param>
		/// <returns>An XColor instance corresponding to the specified hexadecimal color value.</returns>
		/// <exception cref="ArgumentNullException">Thrown if hexColor is null, empty, or consists only of white-space characters.</exception>
		/// <exception cref="ArgumentException">Thrown if hexColor is not in a valid RRGGBB or AARRGGBB hexadecimal format.</exception>
		public static XColor ToXColor(this string hexColor)
		{
			XColor returnValue = new();

			if (string.IsNullOrWhiteSpace(hexColor))
			{
				throw new ArgumentNullException(nameof(hexColor));
			}

			string value = hexColor.Trim().TrimStart('#');

			if (value.Length == 6)
			{
				byte r = Convert.ToByte(value.Substring(0, 2), 16);
				byte g = Convert.ToByte(value.Substring(2, 2), 16);
				byte b = Convert.ToByte(value.Substring(4, 2), 16);

				returnValue = XColor.FromArgb(r, g, b);
			}
			else if (value.Length == 8)
			{
				byte a = Convert.ToByte(value.Substring(0, 2), 16);
				byte r = Convert.ToByte(value.Substring(2, 2), 16);
				byte g = Convert.ToByte(value.Substring(4, 2), 16);
				byte b = Convert.ToByte(value.Substring(6, 2), 16);

				returnValue = XColor.FromArgb(a, r, g, b);
			}
			else
			{
				throw new ArgumentException("Color must be in #RRGGBB or #AARRGGBB format.", nameof(hexColor));
			}

			return returnValue;
		}
	}
}
