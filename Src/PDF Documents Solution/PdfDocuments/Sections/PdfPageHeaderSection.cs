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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfPageHeaderSection<TModel> : PdfSection<TModel>, IPdfTitle<TModel>, IPdfLogoPath<TModel>
		where TModel : IPdfModel
	{
		public BindProperty<string, TModel> LogoPath { get; set; } = string.Empty;
		public BindProperty<string, TModel> Title { get; set; } = string.Empty;

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Draw the background.
			//
			gridPage.DrawFilledRectangle(bounds, this.BackgroundColor.Resolve(gridPage, model));

			//
			// Draw the image left aligned and vertically centered in the
			// header leaving a 1 row margin above and below it. Also 
			// leave a one column margin on the left.
			//
			string path = this.LogoPath.Resolve(gridPage, model);

			if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
			{
				gridPage.DrawImageWithFixedHeight(path, bounds.LeftColumn + this.Padding.Left, bounds.TopRow + this.Padding.Top, bounds.Rows - (this.Padding.Top + this.Padding.Bottom));
			}

			if (this.Title != null)
			{
				PdfBounds textBounds = this.ApplyPadding(gridPage, model, bounds, this.Padding);
				gridPage.DrawText(this.Title.Resolve(gridPage, model), this.Font.Resolve(gridPage, model), textBounds, XStringFormats.CenterRight, this.ForegroundColor.Resolve(gridPage, model));
			}

			return Task.FromResult(returnValue);
		}
	}
}
