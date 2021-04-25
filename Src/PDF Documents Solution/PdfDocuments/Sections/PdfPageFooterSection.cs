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

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the font.
			//
			XFont font = gridPage.HeaderFooterFont();

			//
			// Draw the background.
			//
			gridPage.DrawFilledRectangle(bounds, gridPage.Theme.Color.HeaderFooterBackgroundColor);

			//
			// Get the height of the smaller text.
			//
			PdfSize textSize = gridPage.MeasureText(font, this.Copyright.Resolve(gridPage, model));

			//
			// Calculate the number of text rows in this section.
			//
			int textRows = (int)(bounds.Rows / textSize.Rows);

			//
			// Calculate the number of rows to use for the text.
			//
			int top = bounds.TopRow + (int)(((textRows * textSize.Rows) - (2 * textSize.Rows)) / 2.0);

			gridPage.DrawText(this.Copyright.Resolve(gridPage, model), font, bounds.LeftColumn + this.Padding.Left, top, bounds.Columns - (2 * this.Padding.Left), textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.HeaderFooterColor);
			gridPage.DrawText($"Page {gridPage.PageNumber} of {gridPage.Document.PageCount}", font, bounds.LeftColumn + this.Padding.Left, top, bounds.Columns - (2 * this.Padding.Left), textSize.Rows, XStringFormats.CenterRight, gridPage.Theme.Color.HeaderFooterColor);

			top += textSize.Rows;
			gridPage.DrawText(this.Disclaimer.Resolve(gridPage, model), font, bounds.LeftColumn + this.Padding.Left, top, bounds.Columns - (2 * this.Padding.Left), textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.HeaderFooterColor);
			gridPage.DrawText($"Created {model.CreateDateTime.ToLongDateString()} at {model.CreateDateTime.ToLongTimeString()}", font, bounds.LeftColumn + this.Padding.Left, top, bounds.Columns - (2 * this.Padding.Left), textSize.Rows, XStringFormats.CenterRight, gridPage.Theme.Color.HeaderFooterColor);

			return Task.FromResult(returnValue);
		}
	}
}
