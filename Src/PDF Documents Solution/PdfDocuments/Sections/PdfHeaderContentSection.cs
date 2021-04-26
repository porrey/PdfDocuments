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
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfHeaderContentSection<TModel> : PdfSection<TModel>, IHeaderForegroundColor<TModel>, IHeaderBackgroundColor<TModel>
		where TModel : IPdfModel
	{
		public PdfHeaderContentSection()
		{
			this.Font = new BindPropertyAction<XFont, TModel>((g, m) => g.SubTitle2Font());
		}

		public virtual BindProperty<XColor, TModel> HeaderForegroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((g, m) => g.Theme.Color.SubTitleColor);
		public virtual BindProperty<XColor, TModel> HeaderBackgroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((g, m) => g.Theme.Color.SubTitleBackgroundColor);

		protected override Task<bool> OnLayoutChildrenAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			if (this.Children.Any())
			{
				//
				// Get the header rectangle.
				//
				PdfBounds headerRect = this.GetHeaderRect(gridPage, model, bounds);

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
				this.Children.Single().LayoutAsync(gridPage, model);
			}

			return Task.FromResult(returnValue);
		}

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the header rectangle.
			//
			PdfBounds headerRect = this.GetHeaderRect(gridPage, model, bounds);

			//
			// Draw the filled rectangle.
			//
			gridPage.DrawFilledRectangle(headerRect, this.HeaderBackgroundColor.Resolve(gridPage, model));

			//
			// Check the padding flag.
			//
			bool usePadding = this.UsePadding.Resolve(gridPage, model);

			//
			// Draw the text.
			//
			gridPage.DrawText(this.Text.Resolve(gridPage, model).ToUpper(), this.Font.Resolve(gridPage, model),
						headerRect.LeftColumn + (usePadding ? this.Padding.Left : 0),
						headerRect.TopRow + (usePadding ? this.Padding.Top : 0),
						headerRect.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Left : 0)),
						headerRect.Rows - ((usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0)),
						XStringFormats.CenterLeft, this.HeaderForegroundColor.Resolve(gridPage, model));

			return Task.FromResult(returnValue);
		}

		protected virtual PdfSize GetHeaderSize(PdfGridPage gridPage, TModel model)
		{
			//
			// Check the padding flag.
			//
			bool usePadding = this.UsePadding.Resolve(gridPage, model);

			//
			//
			//
			string text = this.Text.Resolve(gridPage, model).ToUpper();

			//
			// Get the size of the text.
			//
			PdfSize size = gridPage.MeasureText(this.Font.Resolve(gridPage, model), text);
			size.Rows += (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
			size.Columns += (usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0);

			return size;
		}

		protected virtual PdfBounds GetHeaderRect(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			PdfSize size = this.GetHeaderSize(gridPage, model);
			return (new PdfBounds(bounds.LeftColumn, bounds.TopRow, bounds.Columns, size.Rows));
		}
	}
}
