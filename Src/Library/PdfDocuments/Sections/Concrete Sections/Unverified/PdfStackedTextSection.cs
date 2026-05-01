/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	/// <summary>
	/// Represents a PDF section that renders a collection of text items stacked vertically, using customizable styles and
	/// bindings to a model.
	/// </summary>
	/// <remarks>This section allows each text item to be bound to a property of the model and rendered with
	/// distinct styles. The first item and subsequent items can use different font and alignment settings. Items with null
	/// or whitespace values are not rendered. Use this class to display multiple lines of text in a stacked layout within
	/// a PDF grid page.</remarks>
	/// <typeparam name="TModel">The type of the model used for binding text items. Must implement the IPdfModel interface.</typeparam>
	public class PdfStackedTextSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the collection of stacked items bound to the model.
		/// </summary>
		/// <remarks>Each item in the collection represents a binding between a string identifier and a model
		/// instance. The order of items in the collection may affect how they are processed or displayed.</remarks>
		public IEnumerable<BindProperty<string, TModel>> StackedItems { get; set; }

		/// <summary>
		/// Renders stacked text items onto the specified PDF grid page using the provided model and layout bounds.
		/// </summary>
		/// <remarks>The rendering process applies different styles to the first and subsequent items in the stack.
		/// Items with null or whitespace text are not rendered. Padding and alignment are determined by the resolved styles
		/// for each item.</remarks>
		/// <param name="g">The PDF grid page on which the items will be rendered.</param>
		/// <param name="m">The data model used to resolve item values and styles during rendering.</param>
		/// <param name="bounds">The layout bounds that define the area and positioning for rendering the items.</param>
		/// <returns>A task that represents the asynchronous rendering operation. The result is <see langword="true"/> if rendering was
		/// successful; otherwise, <see langword="false"/>.</returns>
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
