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
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments.Example.Theme
{
	public static class ColorPalette
	{
		public static XColor Empty = XColors.Transparent;
		public static XColor White = XColor.FromArgb(255, 255, 255, 255);
		public static XColor Gray = XColor.FromArgb(255, 99, 99, 99);
		public static XColor Blue = XColor.FromArgb(255, 37, 32, 98);
		public static XColor MediumBlue = XColor.FromArgb(255, 37, 32, 98).WithLuminosity(.95);
		public static XColor LightBlue = XColor.FromArgb(255, 37, 32, 98).WithLuminosity(.98);
		public static XColor Red = XColor.FromArgb(255, 215, 35, 44);
		public static XColor MediumRed = XColor.FromArgb(255, 215, 35, 44).WithLuminosity(.95);
		public static XColor LightRed = XColor.FromArgb(255, 215, 35, 44).WithLuminosity(.98);
	}
}
