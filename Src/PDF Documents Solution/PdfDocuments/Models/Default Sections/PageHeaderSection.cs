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
using System.IO;
using System.Threading.Tasks;
using PdfDocuments.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PageHeaderSection<TModel> : PdfSection<TModel>
	{
		public PageHeaderSection()
		{
			this.RelativeHeight = .03;
		}

		public override string Key => "PageHeader";
		public BindProperty<string, TModel> LogoPath { get; set; }
		public BindProperty<string, TModel> Title { get; set; }

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Draw the background.
			//
			gridPage.DrawFilledRectangle(this.ActualBounds, this.BackgroundColor.Invoke(gridPage, model));

			//
			// Draw the image left aligned and vertically centered in the
			// header leaving a 1 row margin above and below it. Also 
			// leave a one column margin on the left.
			//
			string path = this.LogoPath.Invoke(gridPage, model);

			if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
			{
				gridPage.DrawImageWithFixedHeight(path, this.ActualBounds.LeftColumn + this.Padding.Left, this.ActualBounds.TopRow + this.Padding.Top, this.ActualBounds.Rows - (this.Padding.Top + this.Padding.Bottom));
			}

			if (this.Title != null)
			{
				gridPage.DrawText(this.Title.Invoke(gridPage, model), this.Font.Invoke(gridPage, model), this.ActualBounds, XStringFormats.CenterRight, this.ForegroundColor.Invoke(gridPage, model));
			}

			return Task.FromResult(returnValue);
		}
	}
}
