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
	public class PdfSignatureSection<TModel> : PdfSection<TModel>
	{
		public int RightDateColumnPadding { get; set; } = 40;
		public override BindProperty<XFont, TModel> Font => new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyFont().WithSize(gp.Theme.FontSize.BodySmall); });

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Check the padding flag.
			//
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			//
			// Use the standard small body font.
			//
			string label = $"{this.Text.Invoke(gridPage, model)}:";
			XFont bodyFont = this.Font.Invoke(gridPage, model);
			IPdfSize bodyFontSize = gridPage.MeasureText(bodyFont, label);

			//
			// Draw the signature line.
			//
			int top = this.ActualBounds.BottomRow - (usePadding ? 2 * this.Padding.Bottom : 0);
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeightNormal, gridPage.Theme.Color.BodyBoldColor);

			//
			// Draw the text.
			//
			top -= bodyFontSize.Rows + (usePadding ? this.Padding.Bottom : 0);

			gridPage.DrawText(label, bodyFont,
				this.ActualBounds.LeftColumn + (usePadding ? this.Padding.Left : 0),
				top,
				this.ActualBounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
				bodyFontSize.Rows,
				XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			int left = this.ActualBounds.RightColumn - this.RightDateColumnPadding;

			gridPage.DrawText("Date:", bodyFont,
				left,
				top,
				this.ActualBounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
				bodyFontSize.Rows,
				XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			return Task.FromResult(returnValue);
		}
	}
}
