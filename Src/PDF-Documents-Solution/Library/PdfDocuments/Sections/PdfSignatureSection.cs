/*
 *	MIT License
 *
 *	Copyright (c) 2021-2024 Daniel Porrey
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
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfSignatureSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfSpacing padding = style.Padding.Resolve(g, m);

			//
			// Use the standard small body font.
			//
			string label = $"{this.Text.Resolve(g, m)}:";
			XFont bodyFont = style.Font.Resolve(g, m);
			PdfSize bodyFontSize = g.MeasureText(bodyFont, label);

			//
			// Draw the signature line.
			//
			int top = bounds.BottomRow - (2 * padding.Bottom);
			g.DrawHorizontalLine(top, bounds.LeftColumn, bounds.RightColumn, RowEdge.Bottom, style.BorderWidth.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

			//
			// Get the relative width of the sections.
			//
			double[] widths = style.RelativeWidths.Resolve(g, m);
			double width = widths.Length > 0 ? widths[0] : .4;

			//
			// Draw the text.
			//
			top -= bodyFontSize.Rows + padding.Bottom;

			g.DrawText(label, bodyFont,
				bounds.LeftColumn + padding.Left,
				top,
				bounds.Columns - (padding.Left + padding.Right),
				bodyFontSize.Rows,
				style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

			int left = bounds.RightColumn - (int)(bounds.Columns * width);

			g.DrawText("Date:", bodyFont,
				left,
				top,
				bounds.Columns - (padding.Left + padding.Right),
				bodyFontSize.Rows,
				style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
