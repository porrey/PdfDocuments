/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using PdfSharp.Drawing;

namespace PdfDocuments.Theme.Basic
{
	public static class ColorPalette
	{
		//
		// Luminosity = 100%
		//
		public static XColor White = XColor.FromArgb(255, 255, 255, 255);

		//
		// Luminosity = 95%
		//
		public static XColor LightGray = XColor.FromArgb(255, 243, 243, 243);

		//
		// Luminosity = 87%
		//
		public static XColor LightBlue = XColor.FromArgb(255, 186, 210, 221);

		//
		// Luminosity = 58%
		//
		public static XColor GreenishBlue = XColor.FromArgb(255, 76, 132, 147);

		//
		// Luminosity = 33%
		//
		public static XColor DarkGray = XColor.FromArgb(255, 74, 79, 85);

		//
		// Luminosity = 44%
		//
		public static XColor BlueGray = XColor.FromArgb(255, 91, 103, 112);

		//
		// Luminosity = 60%
		//
		public static XColor DarkBlue = XColor.FromArgb(255, 101, 128, 153);

		//
		// Luminosity = 95%
		//
		public static XColor Yellow = XColor.FromArgb(255, 241, 196, 24);

		//
		// Luminosity = 85%
		//
		public static XColor Orange = XColor.FromArgb(255, 217, 111, 39);

		//
		// Luminosity = 38%
		//
		public static XColor Purple = XColor.FromArgb(255, 63, 58, 96);

		//
		// Luminosity = 35%
		//
		public static XColor Brown = XColor.FromArgb(255, 89, 61, 61);

		//
		// Luminosity = 64%
		//
		public static XColor BrownishOrange = XColor.FromArgb(255, 162, 82, 49);

		//
		// Luminosity = 67%
		//
		public static XColor Gold = XColor.FromArgb(255, 172, 133, 31);

		//
		// Luminosity = 95%
		//
		public static XColor Tan = XColor.FromArgb(255, 241, 211, 132);
	}
}
