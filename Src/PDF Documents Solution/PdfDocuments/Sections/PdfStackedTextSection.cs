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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfStackedTextSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public IList<BindProperty<string, TModel>> StackedItems { get; } = new List<BindProperty<string, TModel>>();
		public bool FirstItemDifferent { get; set; }
		public BindProperty<XFont, TModel> FirstItemFont { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyMediumFont(XFontStyle.Bold); });

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Use the standard body font.
			//
			XFont bodyFont = this.Font.Resolve(gridPage, model);
			PdfSize bodyFontSize = gridPage.MeasureText(bodyFont);

			//
			// Use the standard body font.
			//
			XFont bodyMediumBoldFont = this.FirstItemFont.Resolve(gridPage, model);
			PdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont);

			//
			// Check if padding should be used.
			//
			bool usePadding = this.UsePadding.Resolve(gridPage, model);

			int top = bounds.TopRow + (usePadding ? this.Padding.Top : 0);
			int left = bounds.LeftColumn + (usePadding ? this.Padding.Left : 0);

			foreach (BindProperty<string, TModel> item in this.StackedItems)
			{
				//
				// Get the text for the item.
				//
				string text = item.Resolve(gridPage, model);

				//
				// Don't draw the item (or a blank line)
				// if it is empty or null.
				//
				if (!string.IsNullOrWhiteSpace(text))
				{
					//
					// Draw the item.
					//
					if (this.FirstItemDifferent && item == this.StackedItems.First())
					{
						gridPage.DrawText(item.Resolve(gridPage, model), bodyMediumBoldFont,
							left,
							top,
							bounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
							bodyMediumBoldFontSize.Rows,
							XStringFormats.TopLeft, this.ForegroundColor.Resolve(gridPage, model));

						top += bodyMediumBoldFontSize.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
					}
					else
					{
						gridPage.DrawText(item.Resolve(gridPage, model), bodyFont,
							left,
							top,
							bounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
							bodyFontSize.Rows,
							XStringFormats.TopLeft, this.ForegroundColor.Resolve(gridPage, model));

						top += bodyFontSize.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
					}
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
