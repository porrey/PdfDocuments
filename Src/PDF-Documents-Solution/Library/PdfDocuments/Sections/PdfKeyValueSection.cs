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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfDocuments
{
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
			PdfStyle<TModel> keyStyle = this.ResolveStyle(1);
			PdfStyle<TModel> valueStyle = this.ResolveStyle(2);

			//
			// Get the relative width of the sections.
			//
			double[] widths = keyStyle.RelativeWidths.Resolve(g, m);
			double relativeWidth = widths.Length > 0 ? widths[0] : .5;

			//
			// Determine the width
			//
			int keyWidth = (int)(bounds.Columns * relativeWidth);

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
				PdfTextElement<TModel> keyElement = new PdfTextElement<TModel>(item.Key);
				PdfSize keySize = keyElement.Measure(g, m, keyStyle);
				PdfBounds keyBounds = new PdfBounds(bounds.LeftColumn, top, keyWidth, keySize.Rows);
				keyElement.Render(g, m, keyBounds, keyStyle);

				//
				// Draw the Value
				//
				PdfTextElement<TModel> valueElement = new PdfTextElement<TModel>(item.Value.Resolve(g, m));
				PdfSize valueSize = valueElement.Measure(g, m, valueStyle);
				PdfBounds valueBounds = new PdfBounds(bounds.LeftColumn + keyWidth, top, bounds.Columns - keyWidth, valueSize.Rows);
				valueElement.Render(g, m, valueBounds, valueStyle);

				top += (new int[] { keySize.Rows, valueSize.Rows }).Max();
			}

			return Task.FromResult(returnValue);
		}
	}
}
