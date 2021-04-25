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

namespace PdfDocuments.Example.Theme
{
	public class ThemeFontSize : IThemeFontSize
	{
		public double Title1 => 48;
		public double Title2 => 24;
		public double Title3 => 12;
		public double SubTitle1 => 28;
		public double SubTitle2 => 14;
		public double SubTitle3 => 11;
		public double BodyExtraSmall => 6.25;
		public double BodySmall => 7.50;
		public double Body => 7.75;
		public double BodyLarge => 9.25;
		public double BodyExtraLarge => 11.75;
		public double Legal => 6.75;
		public double HeaderFooter => 6.25;
		public double Debug => 6.50;
	}
}
