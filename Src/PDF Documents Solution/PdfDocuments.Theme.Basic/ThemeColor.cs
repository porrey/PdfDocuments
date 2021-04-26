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

namespace PdfDocuments.Theme.Basic
{
	public class ThemeColor : IThemeColor
	{
		public virtual XColor TitleColor => ColorPalette.DarkGray;
		public virtual XColor TitleBackgroundColor => ColorPalette.White;
		
		public virtual XColor SubTitleColor => ColorPalette.White;
		public virtual XColor SubTitleBackgroundColor => ColorPalette.BlueGray;
		
		public virtual XColor HeaderFooterColor => ColorPalette.DarkGray.WithLuminosity(.18);
		public virtual XColor HeaderFooterBackgroundColor => ColorPalette.LightGray.WithLuminosity(.90);
		
		public virtual XColor BodyColor => ColorPalette.DarkGray.WithLuminosity(.16);
		public virtual XColor BodyLightColor => ColorPalette.DarkGray.WithLuminosity(.20);
		public virtual XColor BodySubtleColor => ColorPalette.DarkGray.WithLuminosity(.24);
		public virtual XColor BodyVeryLightColor => ColorPalette.DarkGray.WithLuminosity(.64);
		public virtual XColor BodyEmphasisColor => ColorPalette.GreenishBlue.WithLuminosity(.40);
		public virtual XColor BodyHighlightColor => ColorPalette.Orange.WithLuminosity(.40);
		public virtual XColor BodyBoldColor => ColorPalette.DarkGray.WithLuminosity(.11);

		public virtual XColor BodyBackgroundColor => ColorPalette.White;
		public virtual XColor AlternateBackgroundColor1 => ColorPalette.LightBlue;
		public virtual XColor AlternateBackgroundColor2 => ColorPalette.Brown;
		public virtual XColor AlternateBackgroundColor3 => ColorPalette.BrownishOrange;
		public virtual XColor AlternateBackgroundColor4 => ColorPalette.Tan;
	}
}
