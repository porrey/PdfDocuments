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

namespace PdfDocuments
{
	/// <summary>
	/// Represents a color in the HSL (Hue, Saturation, Lightness) color space.
	/// </summary>
	/// <remarks>The HSL color model is commonly used for color manipulation and selection, as it separates color
	/// information (hue) from intensity (lightness) and purity (saturation). The values for hue, saturation, and lightness
	/// are not validated; callers should ensure they are within the expected ranges for correct color
	/// conversion.</remarks>
	public class Hsl
	{
		/// <summary>
		/// Initializes a new instance of the Hsl class with the specified hue, saturation, and lightness values.
		/// </summary>
		/// <param name="h">The hue component of the color, typically in degrees from 0 to 360.</param>
		/// <param name="s">The saturation component of the color, as a value between 0 and 1.</param>
		/// <param name="l">The lightness component of the color, as a value between 0 and 1.</param>
		public Hsl(double h, double s, double l)
		{
			this.H = h;
			this.S = s;
			this.L = l;
		}

		/// <summary>
		/// Gets or sets the value of H.
		/// </summary>
		public double H { get; set; }

		/// <summary>
		/// Gets or sets the value of S.
		/// </summary>
		public double S { get; set; }

		/// <summary>
		/// Gets or sets the value of L.
		/// </summary>
		public double L { get; set; }

		/// <summary>
		/// Converts the current HSL color representation to a corresponding RGB color.
		/// </summary>
		/// <remarks>This method performs a conversion from HSL (Hue, Saturation, Lightness) to RGB (Red, Green, Blue)
		/// color space. The resulting color is suitable for use in graphics APIs that require RGB values. The conversion
		/// preserves the perceived color as closely as possible.</remarks>
		/// <returns>A Color instance representing the equivalent RGB color. The returned color will have its alpha channel set to 255.</returns>
		public Color ToColor()
		{
			double v;
			double r, g, b;

			r = this.L;
			g = this.L;
			b = this.L;
			v = (this.L <= 0.5) ? (this.L * (1.0 + this.S)) : (this.L + this.S - this.L * this.S);
			
			if (v > 0)
			{
				double m;
				double sv;
				int sextant;
				double fract, vsf, mid1, mid2;

				m = this.L + this.L - v;
				sv = (v - m) / v;
				double hue = (this.H / 360.0) * 6.0;
				sextant = (int)hue;
				fract = hue - sextant;
				vsf = v * sv * fract;
				mid1 = m + vsf;
				mid2 = v - vsf;
				switch (sextant)
				{
					case 0:
						r = v;
						g = mid1;
						b = m;
						break;
					case 1:
						r = mid2;
						g = v;
						b = m;
						break;
					case 2:
						r = m;
						g = v;
						b = mid1;
						break;
					case 3:
						r = m;
						g = mid2;
						b = v;
						break;
					case 4:
						r = mid1;
						g = m;
						b = v;
						break;
					case 5:
						r = v;
						g = m;
						b = mid2;
						break;
				}
			}

			return Color.FromArgb(Convert.ToByte(r * 255.0), Convert.ToByte(g * 255.0), Convert.ToByte(b * 255.0));
		}

		/// <summary>
		/// Returns a string representation of the HSL color using formatted numeric values.
		/// </summary>
		/// <remarks>The returned string is formatted as "H, S%, L%", where H is the hue, S is the saturation, and L
		/// is the lightness. This format is useful for displaying HSL color values in a human-readable form.</remarks>
		/// <returns>A string containing the hue as a number with one decimal place, and the saturation and lightness as percentages.</returns>
		public override string ToString()
		{
			return string.Format("{0:N1}, {1:P0}, {2:P0}", this.H, this.S, this.L);
		}
	}
}
