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
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfNameValueItem<TModel>
	{
		public XStringFormat NameAlignment { get; set; }
		public XStringFormat ValueAlignment { get; set; }
		public string Key { get; set; }
		public BindProperty<string, TModel> Value { get; set; }
	}

	public class PdfNameValueSection<TModel> : PdfSection<TModel>
	{
		public virtual BindProperty<XFont, TModel> ValueFont { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyFont(); });
		public virtual double NameColumnRelativeWidth { get; set; }
		public virtual IList<PdfNameValueItem<TModel>> Items { get; } = new List<PdfNameValueItem<TModel>>();

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Determine if padding should be used.
			//
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			//
			// Create the fonts.
			//
			XFont nameFont = this.Font.Invoke(gridPage, model);
			XFont valueFont = this.ValueFont.Invoke(gridPage, model);

			//
			// Get the size of the name text.
			//
			IPdfSize textSize = gridPage.MeasureText(nameFont);

			//
			// Determine the number of columns to use for the name.
			//
			int columns = 0;

			if (this.NameColumnRelativeWidth == 0)
			{
				//
				// Get the widest text.
				//
				int widest = 0;

				foreach (PdfNameValueItem<TModel> item in this.Items)
				{
					IPdfSize size = gridPage.MeasureText(nameFont, item.Key);

					if (size.Columns > widest)
					{
						widest = size.Columns;
					}
				}

				columns = widest + (usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0);
			}
			else
			{
				columns = (int)(this.ActualBounds.Columns * this.NameColumnRelativeWidth);
			}

			//
			// Set the initial top and the left for the name and values.
			//
			int top = this.ActualBounds.TopRow + (usePadding ? this.Padding.Top : 0);
			int nameLeft = this.ActualBounds.LeftColumn + (usePadding ? this.Padding.Left : 0);
			int valueLeft = nameLeft + columns;
			int textRows = textSize.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);

			foreach (PdfNameValueItem<TModel> item in this.Items)
			{
				gridPage.DrawText(item.Key, nameFont, nameLeft, top, columns, textRows, item.NameAlignment, gridPage.Theme.Color.BodyColor);
				gridPage.DrawText(item.Value.Invoke(gridPage, model), valueFont, valueLeft, top, columns, textRows, item.ValueAlignment, gridPage.Theme.Color.BodyColor);
				top += textRows;
			}

			return Task.FromResult(returnValue);
		}
	}
}
