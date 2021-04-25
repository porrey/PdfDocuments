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
	public class PdfKeyValueItem<TModel>
		where TModel : IPdfModel
	{
		public XStringFormat KeyAlignment { get; set; }
		public XStringFormat ValueAlignment { get; set; }
		public string Key { get; set; }
		public BindProperty<string, TModel> Value { get; set; }
	}

	public class PdfKeyValueSection<TModel> : PdfSection<TModel>, IPdfValueFont<TModel>
		where TModel : IPdfModel
	{
		public PdfKeyValueSection()
		{
		}

		public PdfKeyValueSection(params PdfKeyValueItem<TModel>[] values)
		{
			foreach (PdfKeyValueItem<TModel> value in values)
			{
				this.Items.Add(value);
			}
		}

		public virtual BindProperty<XFont, TModel> ValueFont { get; set; } = new BindProperty<XFont, TModel>((gp, m) => gp.BodyFont());
		public virtual BindProperty<double, TModel> NameColumnRelativeWidth { get; set; } = 0;
		public virtual IList<PdfKeyValueItem<TModel>> Items { get; } = new List<PdfKeyValueItem<TModel>>();

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Determine if padding should be used.
			//
			bool usePadding = this.UsePadding.Resolve(gridPage, model);

			//
			// Create the fonts.
			//
			XFont nameFont = this.Font.Resolve(gridPage, model);
			XFont valueFont = this.ValueFont.Resolve(gridPage, model);

			//
			// Get the size of the name text.
			//
			PdfSize textSize = gridPage.MeasureText(nameFont);

			//
			// Determine the width
			//
			int width = bounds.Columns / 2;

			//
			// Determine the height.
			//
			int height = bounds.Rows / this.Items.Count;

			//
			// Get the starting point for the top of the text.
			//
			int top = bounds.TopRow;

			//
			// Set the initial top and the left for the name and values.
			//
			foreach (PdfKeyValueItem<TModel> item in this.Items)
			{
				//
				// Draw the Key value
				//
				{
					PdfBounds textBounds = new PdfBounds(bounds.LeftColumn, top, width, height);
					PdfBounds paddedBounds = this.ApplyPadding(gridPage, model, textBounds, this.Padding);
					gridPage.DrawText(item.Key, nameFont, paddedBounds, item.KeyAlignment, this.ForegroundColor.Resolve(gridPage, model));
				}

				//
				// Draw the Key value
				//
				{
					PdfBounds textBounds = new PdfBounds(bounds.LeftColumn + width, top, width, height);
					PdfBounds paddedBounds = this.ApplyPadding(gridPage, model, textBounds, this.Padding);
					gridPage.DrawText(item.Value.Resolve(gridPage, model), valueFont, paddedBounds, item.ValueAlignment, this.ForegroundColor.Resolve(gridPage, model));
				}

				top += height;
			}

			return Task.FromResult(returnValue);
		}
	}
}
