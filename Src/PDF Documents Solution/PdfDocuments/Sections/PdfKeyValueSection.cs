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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfKeyValueItem<TModel>
		where TModel : IPdfModel
	{
		public string Key { get; set; }
		public BindProperty<string, TModel> Value { get; set; }
	}

	public class PdfKeyValueSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfKeyValueSection(params PdfKeyValueItem<TModel>[] values)
		{
			foreach (PdfKeyValueItem<TModel> value in values)
			{
				this.Items.Add(value);
			}
		}

		public virtual IList<PdfKeyValueItem<TModel>> Items { get; } = new List<PdfKeyValueItem<TModel>>();

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the styles.
			//
			PdfStyle<TModel> keyStyle = this.StyleManager.GetStyle(this.StyleNames.ElementAt(1));
			PdfStyle<TModel> valueStyle = this.StyleManager.GetStyle(this.StyleNames.ElementAt(2));

			//
			// Create the fonts.
			//
			XFont nameFont = keyStyle.Font.Resolve(g, m);
			XFont valueFont = valueStyle.Font.Resolve(g, m);

			//
			// Get the relative width of the sections.
			//
			double[] widths = keyStyle.RelativeWidths.Resolve(g, m);
			double relativeWidth = widths.Length > 0 ? widths[0] : .5;

			//
			// Determine the width
			//
			int keyWidth = (int)(bounds.Columns * relativeWidth);
			int valueWidth = (int)(bounds.Columns * (1 - relativeWidth));

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
				// Draw the Key
				//
				{
					PdfBounds textBounds = new PdfBounds(bounds.LeftColumn, top, keyWidth, height);
					PdfBounds paddedBounds = this.ApplyPadding(g, m, textBounds, keyStyle.Padding.Resolve(g, m));
					g.DrawFilledRectangle(paddedBounds, keyStyle.BackgroundColor.Resolve(g, m));
					g.DrawText(item.Key, nameFont, paddedBounds, keyStyle.TextAlignment.Resolve(g, m), keyStyle.ForegroundColor.Resolve(g, m));
				}

				//
				// Draw the Value
				//
				{
					PdfBounds textBounds = new PdfBounds(bounds.LeftColumn + keyWidth, top, valueWidth, height);
					PdfBounds paddedBounds = this.ApplyPadding(g, m, textBounds, valueStyle.Padding.Resolve(g, m));
					g.DrawFilledRectangle(paddedBounds, valueStyle.BackgroundColor.Resolve(g, m));
					g.DrawText(item.Value.Resolve(g, m), valueFont, paddedBounds, valueStyle.TextAlignment.Resolve(g, m), valueStyle.ForegroundColor.Resolve(g, m));
				}

				top += height;
			}

			return Task.FromResult(returnValue);
		}
	}
}
