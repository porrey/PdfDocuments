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

namespace PdfDocuments.Theme.Basic
{
	public class ThemeColor : IThemeColor
	{
		public XColor TitleColor => ThemeColors.DarkGray;
		public XColor TitleBackgroundColor => ThemeColors.White;
		
		public XColor SubTitleColor => ThemeColors.White;
		public XColor SubTitleBackgroundColor => ThemeColors.BlueGray;
		
		public XColor HeaderFooterColor => ThemeColors.DarkGray.WithLuminosity(.18);
		public XColor HeaderFooterBackgroundColor => ThemeColors.LightGray.WithLuminosity(.90);
		
		public XColor BodyColor => ThemeColors.DarkGray.WithLuminosity(.16);
		public XColor BodyLightColor => ThemeColors.DarkGray.WithLuminosity(.20);
		public XColor BodySubtleColor => ThemeColors.DarkGray.WithLuminosity(.24);
		public XColor BodyVeryLightColor => ThemeColors.DarkGray.WithLuminosity(.64);
		public XColor BodyEmphasisColor => ThemeColors.GreenishBlue.WithLuminosity(.40);
		public XColor BodyHighlightColor => ThemeColors.Orange.WithLuminosity(.40);
		public XColor BodyBoldColor => ThemeColors.DarkGray.WithLuminosity(.11);

		public XColor BodyBackgroundColor => ThemeColors.White;
		public XColor AlternateBackgroundColor1 => ThemeColors.LightBlue;
		public XColor AlternateBackgroundColor2 => ThemeColors.Brown;
		public XColor AlternateBackgroundColor3 => ThemeColors.BrownishOrange;
		public XColor AlternateBackgroundColor4 => ThemeColors.Tan;
	}
}
