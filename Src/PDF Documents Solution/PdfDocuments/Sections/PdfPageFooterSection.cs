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
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfPageFooterSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfPageFooterSection()
		{
			this.RelativeHeight = .03;
		}

		public BindProperty<string, TModel> Copyright { get; set; } = string.Empty;
		public BindProperty<string, TModel> Disclaimer { get; set; } = string.Empty;

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get style.
			//
			PdfStyle<TModel> style = this.StyleManager.GetStyle(this.StyleNames.First());
			PdfSpacing padding = style.Padding.Resolve(g, m);
			XFont font = style.Font.Resolve(g, m);

			//
			// Draw the background.
			//
			g.DrawFilledRectangle(bounds, style.BackgroundColor.Resolve(g, m));

			//
			// Get the height of the smaller text.
			//
			PdfSize textSize = g.MeasureText(font, this.Copyright.Resolve(g, m));

			//
			// Calculate the number of text rows in this section.
			//
			int textRows = (int)(bounds.Rows / textSize.Rows);

			//
			// Calculate the number of rows to use for the text.
			//
			int top = bounds.TopRow + (int)(((textRows * textSize.Rows) - (2 * textSize.Rows)) / 2.0);

			g.DrawText(this.Copyright.Resolve(g, m), font, bounds.LeftColumn + padding.Left, top, bounds.Columns - (2 * padding.Left), textSize.Rows, XStringFormats.CenterLeft, style.ForegroundColor.Resolve(g, m));
			g.DrawText($"Page {g.PageNumber} of {g.Document.PageCount}", font, bounds.LeftColumn + padding.Left, top, bounds.Columns - (2 * padding.Left), textSize.Rows, XStringFormats.CenterRight, style.ForegroundColor.Resolve(g, m));

			top += textSize.Rows;
			g.DrawText(this.Disclaimer.Resolve(g, m), font, bounds.LeftColumn + padding.Left, top, bounds.Columns - (2 * padding.Left), textSize.Rows, XStringFormats.CenterLeft, style.ForegroundColor.Resolve(g, m));
			g.DrawText($"Created {m.CreateDateTime.ToLongDateString()} at {m.CreateDateTime.ToLongTimeString()}", font, bounds.LeftColumn + padding.Left, top, bounds.Columns - (2 * padding.Left), textSize.Rows, XStringFormats.CenterRight, style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
