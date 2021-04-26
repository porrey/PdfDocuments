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

namespace PdfDocuments.Theme.Basic
{
	public class BasicTheme : ITheme
	{
		public virtual IThemeColor Color => this.OnGetThemeColor();
		public virtual IThemeFontFamily FontFamily => this.OnGetThemeFontFamily();
		public virtual IThemeFontSize FontSize => this.OnGetThemeFontSize();
		public virtual IThemeDrawing Drawing => this.OnGetThemeDrawing();

		protected virtual IThemeColor OnGetThemeColor()
		{
			return new ThemeColor();
		}

		protected virtual IThemeFontFamily OnGetThemeFontFamily()
		{
			return new ThemeFontFamily();
		}

		protected virtual IThemeFontSize OnGetThemeFontSize()
		{
			return new ThemeFontSize();
		}

		protected virtual IThemeDrawing OnGetThemeDrawing()
		{
			return new ThemeDrawing();
		}
	}
}
