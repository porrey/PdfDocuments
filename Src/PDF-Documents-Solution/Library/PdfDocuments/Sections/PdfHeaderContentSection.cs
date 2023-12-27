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
using System.Linq;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfHeaderContentSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		protected override Task<bool> OnLayoutChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			if (this.Children.Any())
			{
				//
				// Get the header rectangle.
				//
				PdfBounds headerRect = this.GetHeaderRect(g, m, bounds);

				//
				// Set the bound of the child section to be just
				// below the header section.
				//
				this.Children.Single().ActualBounds.LeftColumn = headerRect.LeftColumn;
				this.Children.Single().SetActualColumns(headerRect.Columns);
				this.Children.Single().ActualBounds.TopRow = headerRect.BottomRow + 1;
				this.Children.Single().SetActualRows(bounds.Rows - headerRect.Rows);

				//
				// Apply the layout.
				//
				this.Children.Single().LayoutAsync(g, m);
			}

			return Task.FromResult(returnValue);
		}

		protected override bool OnShouldDrawBackground()
		{
			return false;
		}

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the styles.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the header rectangle.
			//
			PdfBounds headerRect = this.GetHeaderRect(g, m, bounds);

			//
			// Draw the filled rectangle.
			//
			g.DrawFilledRectangle(headerRect, style.BackgroundColor.Resolve(g, m));

			//
			// Draw the text.
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);

			g.DrawText(this.Text.Resolve(g, m).ToUpper(),
						style.Font.Resolve(g, m),
						headerRect.LeftColumn + padding.Left,
						headerRect.TopRow + padding.Top,
						headerRect.Columns - (padding.Left + padding.Right),
						headerRect.Rows - (padding.Top + padding.Bottom),
						style.TextAlignment.Resolve(g, m), 
						style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}

		protected virtual PdfSize GetHeaderSize(PdfGridPage g, TModel m)
		{
			//
			// Get the styles.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the text.
			//
			string text = this.Text.Resolve(g, m).ToUpper();

			//
			// Get the size of the text.
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);
			PdfSize size = g.MeasureText(style.Font.Resolve(g, m), text);
			size.Rows += padding.Top + padding.Bottom;
			size.Columns += padding.Left + padding.Right;

			return size;
		}

		protected virtual PdfBounds GetHeaderRect(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			PdfSize size = this.GetHeaderSize(g, m);
			return (new PdfBounds(bounds.LeftColumn, bounds.TopRow, bounds.Columns, size.Rows));
		}
	}
}
