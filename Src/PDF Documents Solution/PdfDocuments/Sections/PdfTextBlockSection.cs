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
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfTextBlockSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the padding flagging.
			//
			bool usePadding = this.UsePadding.Resolve(gridPage, model);

			//
			// Determine the bounds.
			//
			PdfBounds paddedBounds = new PdfBounds()
			{
				LeftColumn = bounds.LeftColumn + (usePadding ? this.Padding.Left : 0),
				TopRow = bounds.TopRow + (usePadding ? this.Padding.Top : 0),
				Columns = bounds.Columns - ((usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0)),
				Rows = bounds.Rows - ((usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0)),
			};

			//
			// Draw the text.
			//
			gridPage.DrawText(this.Text.Resolve(gridPage, model),
							  this.Font.Resolve(gridPage, model),
							  paddedBounds,
							  this.TextAlignment.Resolve(gridPage, model),
							  this.ForegroundColor.Resolve(gridPage, model));

			return Task.FromResult(returnValue);
		}
	}
}
