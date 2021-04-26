/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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
using System;
using System.Drawing;
using System.Linq;
using PdfSharp.Drawing;

namespace PdfDocuments.Theme.Abstractions
{
	public static class XColorExtensions
	{
		private static Random Rnd { get; } = new Random();

		public static double Contrast(this XColor color1, XColor color2)
		{
			//
			// Calculate the contrast between the label color and the background color.
			//
			double cb1 = color1.GetBrightness() * 255;
			double cb2 = color2.GetBrightness() * 255;
			return Math.Abs(cb1 - cb2);
		}

		public static XColor RandomColor()
		{
			//
			// Create a random color
			//
			return new XColor() { A = 255, R = (byte)Rnd.Next(0, 255), G = (byte)Rnd.Next(0, 255), B = (byte)Rnd.Next(0, 255) };
		}

		public static Color ToGdiColor(this XColor color)
		{
			return Color.FromArgb((int)(color.A * 255), color.R, color.G, color.B);
		}

		public static XColor ToXColor(this Color color)
		{
			return XColor.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static Hsl ToHsl(this Color color)
		{
			double[] percentage = new[] { color.R / 255.0d, color.G / 255.0d, color.B / 255.0d };

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

		public static bool IsNearTo(this double x, double y, int digits = 3)
		{
			double difference = 1.0d / Math.Pow(10, digits);

			return Math.Abs(x - y) < difference;
		}

		public static XColor WithLuminosity(this XColor color, double luminosity)
		{
			Color c = color.ToGdiColor();
			Hsl hsl = c.ToHsl();
			hsl.L = luminosity;
			return hsl.ToColor().ToXColor();
		}
	}
}
