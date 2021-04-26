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
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments.Example.Theme
{
	public class ThemeColor : IThemeColor
	{
		public XColor TitleColor => ColorPalette.Blue;
		public XColor TitleBackgroundColor => ColorPalette.Empty;
		
		public XColor SubTitleColor => ColorPalette.White;
		public XColor SubTitleBackgroundColor => ColorPalette.MediumRed;
		
		public XColor HeaderFooterColor => ColorPalette.Gray;
		public XColor HeaderFooterBackgroundColor => ColorPalette.Empty;
		
		public XColor BodyColor => ColorPalette.Gray;
		public XColor BodyLightColor => ColorPalette.Gray.WithLuminosity(.20);
		public XColor BodySubtleColor => ColorPalette.Gray.WithLuminosity(.24);
		public XColor BodyVeryLightColor => ColorPalette.Gray.WithLuminosity(.64);
		public XColor BodyEmphasisColor => ColorPalette.Blue.WithLuminosity(.40);
		public XColor BodyHighlightColor => ColorPalette.Red.WithLuminosity(.40);
		public XColor BodyBoldColor => ColorPalette.Gray.WithLuminosity(.11);
		public XColor BodyBackgroundColor => ColorPalette.Empty;

		public XColor AlternateBackgroundColor1 => ColorPalette.LightBlue;
		public XColor AlternateBackgroundColor2 => ColorPalette.MediumBlue;
		public XColor AlternateBackgroundColor3 => ColorPalette.LightRed;
		public XColor AlternateBackgroundColor4 => ColorPalette.MediumRed;
	}
}
