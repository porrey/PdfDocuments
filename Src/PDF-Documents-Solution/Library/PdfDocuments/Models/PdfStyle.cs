/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	public class PdfStyle<TModel> : IStyleBuilder<TModel>
			where TModel : IPdfModel
	{
		public virtual BindProperty<double, TModel> RelativeHeight { get; set; } = 0;
		public virtual BindProperty<double[], TModel> RelativeWidths { get; set; } = new double[] { 0 };
		public virtual BindProperty<XFont, TModel> Font { get; set; } = new XFont("Arial", 12);
		public virtual BindProperty<PdfSpacing, TModel> Margin { get; set; } = new PdfSpacing(0, 0, 0, 0);
		public virtual BindProperty<PdfSpacing, TModel> Padding { get; set; } = new PdfSpacing(0, 0, 0, 0);
		public virtual BindProperty<XColor, TModel> ForegroundColor { get; set; } = XColors.Black;
		public virtual BindProperty<XColor, TModel> BackgroundColor { get; set; } = XColors.Transparent;
		public virtual BindProperty<double, TModel> BorderWidth { get; set; } = 0;
		public virtual BindProperty<XColor, TModel> BorderColor { get; set; } = XColors.Transparent;
		public virtual BindProperty<XStringFormat, TModel> TextAlignment { get; set; } = XStringFormats.CenterLeft;
		public virtual BindProperty<XParagraphAlignment, TModel> ParagraphAlignment { get; set; } = XParagraphAlignment.Justify;
		public virtual BindProperty<PdfSpacing, TModel> CellPadding { get; set; } = new PdfSpacing(0, 0, 0, 0);
	}
}
