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
using System.Linq;
using System.Threading.Tasks;
using PdfDocuments.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfHeaderContentSection<TModel> : PdfSection<TModel>
	{
		public virtual BindProperty<XColor, TModel> HeaderBackgroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.SubTitleBackgroundColor; });
		public virtual BindProperty<XColor, TModel> HeaderForegroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.SubTitleColor; });

		protected override Task<bool> OnLayoutChildrenAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			if (this.Children.Count() == 1)
			{
				//
				//
				//
				(string _, XFont _, bool _, IPdfSize size) = this.GetSize(gridPage, model);

				//
				//
				//
				this.Children.Single().SetActualColumns(this.ActualBounds.Columns);
				this.Children.Single().SetActualRows(this.ActualBounds.Rows - size.Rows);
				this.Children.Single().ActualBounds.LeftColumn = this.ActualBounds.LeftColumn;
				this.Children.Single().ActualBounds.TopRow = this.ActualBounds.TopRow + size.Rows;

				//
				//
				//
				this.Children.Single().LayoutAsync(gridPage, model);
			}
			else
			{
				throw new Exception("This section must have exactly one child section.");
			}

			return Task.FromResult(returnValue);
		}

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Get the size needed.
			//
			(string text, XFont font, bool usePadding, IPdfSize size) = this.GetSize(gridPage, model);

			//
			// Draw the filled rectangle.
			//
			IPdfBounds fillBounds = new PdfBounds(this.ActualBounds.LeftColumn, this.ActualBounds.TopRow, this.ActualBounds.Columns, size.Rows);
			gridPage.DrawFilledRectangle(fillBounds, this.HeaderBackgroundColor.Invoke(gridPage, model));

			//
			// Draw the text.
			//
			gridPage.DrawText(text, font,
						fillBounds.LeftColumn + (usePadding ? this.Padding.Left : 0),
						fillBounds.TopRow + (usePadding ? this.Padding.Top : 0),
						fillBounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Left : 0)),
						fillBounds.Rows - ((usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0)),
						XStringFormats.CenterLeft, this.HeaderForegroundColor.Invoke(gridPage, model));

			return Task.FromResult(returnValue);
		}

		protected virtual (string, XFont, bool, IPdfSize) GetSize(IPdfGridPage gridPage, TModel model)
		{
			//
			//
			//
			XFont font = gridPage.SubTitleFont();

			//
			// Check the padding flag.
			//
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			//
			//
			//
			string text = this.Text.Invoke(gridPage, model).ToUpper();

			//
			// Get the size of the text.
			//
			IPdfSize size = gridPage.MeasureText(font, text);
			size.Rows += (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0);
			size.Columns += (usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0);

			return (text, font, usePadding, size);
		}
	}
}
