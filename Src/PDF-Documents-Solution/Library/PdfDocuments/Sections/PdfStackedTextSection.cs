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
	public class PdfStackedTextSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public IEnumerable<BindProperty<string, TModel>> StackedItems { get; set; }

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get style.
			//
			PdfStyle<TModel> style1 = this.ResolveStyle(0);
			PdfStyle<TModel> style2 = this.ResolveStyle(1);

			//
			// Calculate the starting point.
			//
			PdfSpacing padding1 = style1.Padding.Resolve(g, m);
			PdfSpacing padding2 = style2.Padding.Resolve(g, m);
			int top = bounds.TopRow + padding2.Top;
			int left = bounds.LeftColumn + padding2.Left;

			foreach (BindProperty<string, TModel> item in this.StackedItems)
			{
				//
				// Get the text for the item.
				//
				string text = item.Resolve(g, m);

				//
				// Don't draw the item (or a blank line)
				// if it is empty or null.
				//
				if (!string.IsNullOrWhiteSpace(text))
				{
					//
					// Draw the item.
					//
					if (item == this.StackedItems.First())
					{
						PdfSize size = g.MeasureText(style2.Font.Resolve(g, m));

						g.DrawText(text, style2.Font.Resolve(g, m),
							left,
							top,
							bounds.Columns - (padding2.Left + padding2.Right),
							size.Rows,
							style1.TextAlignment.Resolve(g, m), style1.ForegroundColor.Resolve(g, m));

						top += size.Rows + padding2.Top + padding2.Bottom;
					}
					else
					{
						PdfSize size = g.MeasureText(style1.Font.Resolve(g, m));

						g.DrawText(text, style1.Font.Resolve(g, m),
							left,
							top,
							bounds.Columns - (padding1.Left + padding1.Right),
							size.Rows,
							XStringFormats.TopLeft, style1.ForegroundColor.Resolve(g, m));

						top += size.Rows + (padding1.Top + padding1.Bottom);
					}
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
