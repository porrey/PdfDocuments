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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfDocuments.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfStackedTextSection<TModel> : PdfSection<TModel>
	{
		public IList<BindProperty<string, TModel>> StackedItems { get; } = new List<BindProperty<string, TModel>>();
		public bool FirstItemDifferent { get; set; }
		public BindProperty<XFont, TModel> FirstItemFont { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyMediumFont(XFontStyle.Bold); });

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Use the standard body font.
			//
			XFont bodyFont = this.Font.Invoke(gridPage, model);
			IPdfSize bodyFontSize = gridPage.MeasureText(bodyFont);

			//
			// Use the standard body font.
			//
			XFont bodyMediumBoldFont = this.FirstItemFont.Invoke(gridPage, model);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont);

			//
			// Check if padding should be used.
			//
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			int top = this.ActualBounds.TopRow + (usePadding ? this.Padding.Top : 0);
			int left = this.ActualBounds.LeftColumn + (usePadding ? this.Padding.Left : 0);

			foreach (BindProperty<string, TModel> item in this.StackedItems)
			{
				//
				// Get the text for the item.
				//
				string text = item.Invoke(gridPage, model);

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
						gridPage.DrawText(item.Invoke(gridPage, model), bodyMediumBoldFont,
							left,
							top,
							this.ActualBounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
							bodyMediumBoldFontSize.Rows,
							XStringFormats.TopLeft, this.ForegroundColor.Invoke(gridPage, model));

						top += bodyMediumBoldFontSize.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
					}
					else
					{
						gridPage.DrawText(item.Invoke(gridPage, model), bodyFont,
							left,
							top,
							this.ActualBounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
							bodyFontSize.Rows,
							XStringFormats.TopLeft, this.ForegroundColor.Invoke(gridPage, model));

						top += bodyFontSize.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
					}
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
