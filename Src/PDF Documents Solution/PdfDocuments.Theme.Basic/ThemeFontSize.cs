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

namespace PdfDocuments.Theme.Basic
{
	public class ThemeFontSize : IThemeFontSize
	{
		public virtual double Title1 => 20.25;
		public virtual double Title2 => 14.0;
		public virtual double Title3 => 9.75;
		public virtual double BodyExtraSmall => 6.25;
		public virtual double BodySmall => 7.50;
		public virtual double Body => 7.75;
		public virtual double BodyLarge => 9.25;
		public virtual double BodyExtraLarge => 11.75;
		public virtual double Legal => 6.75;
		public virtual double HeaderFooter => 6.25;
		public virtual double Debug => 6.50;
	}
}
