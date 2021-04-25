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

namespace PdfDocuments
{
	public static class ThemeFontDecorator
	{
		public static XFont BodyFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.Body, source.Theme.FontSize.Body, style);
		}

		public static XFont BodyLightFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.BodyLight, source.Theme.FontSize.BodySmall, style);
		}

		public static XFont BodyMediumFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.BodyMedium, source.Theme.FontSize.Body, style);
		}

		public static XFont DebugFont(this PdfGridPage source, XFontStyle style = XFontStyle.Bold)
		{
			return new XFont(source.Theme.FontFamily.Debug, source.Theme.FontSize.Debug, style);
		}

		public static XFont HeaderFooterFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.HeaderFooter, source.Theme.FontSize.HeaderFooter, style);
		}

		public static XFont TitleFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.Title, source.Theme.FontSize.Title1, style);
		}

		public static XFont TitleLightFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.TitleLight, source.Theme.FontSize.Title1, style);
		}

		public static XFont SubTitleFont(this PdfGridPage source, XFontStyle style = XFontStyle.Regular)
		{
			return new XFont(source.Theme.FontFamily.SubTitle, source.Theme.FontSize.Title3, style);
		}

		public static XFont WithSize(this XFont font, double emSize)
		{
			return new XFont(font.FontFamily.Name, emSize, font.Style);
		}
	}
}
