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
	public class ThemeFontFamily : IThemeFontFamily

	{
		/*
		Gotham Narrow Book
		Gotham Narrow Light
		TisaPro
		TisaPro-Bold
		TisaPro-BoldIta
		TisaPro-Ita
		TisaPro-Light
		TisaPro-LightIta
		TisaPro-Medi
		TisaPro-MediIta
		*/

		public virtual string Body => "TisaPro";
		public virtual string BodyLight => "TisaPro-Light";
		public virtual string BodyMedium => "TisaPro-Medi";
		public virtual string Title => "Gotham Narrow Book";
		public virtual string TitleLight => "Gotham Narrow Light";
		public virtual string Debug => "Arial Narrow";
		public virtual string HeaderFooter => "TisaPro-Light";
		public virtual string SubTitle => "Gotham Narrow Book";
	}
}
