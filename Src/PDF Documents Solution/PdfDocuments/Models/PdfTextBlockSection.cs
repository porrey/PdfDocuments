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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfTextBlockSection<TModel> : PdfSection<TModel>
	{
		public XStringFormat Alignment { get; set; } = XStringFormats.CenterLeft;

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			//
			//
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			//
			//
			//
			IPdfBounds bounds = new PdfBounds()
			{
				LeftColumn = this.ActualBounds.LeftColumn + (usePadding ? this.Padding.Left : 0),
				TopRow = this.ActualBounds.TopRow + (usePadding ? this.Padding.Top : 0),
				Columns = this.ActualBounds.Columns + (usePadding ? this.Padding.Left : 0) + (usePadding ? this.Padding.Right : 0),
				Rows = this.ActualBounds.Rows + (usePadding ? this.Padding.Top : 0) + (usePadding ? this.Padding.Bottom : 0),
			};

			//
			// Draw the text.
			//
			gridPage.DrawText(this.Text.Invoke(gridPage, model), 
							 this.Font.Invoke(gridPage, model), 
							 bounds, 
							 this.Alignment, 
							 this.ForegroundColor.Invoke(gridPage, model));

			return Task.FromResult(returnValue);
		}
	}
}
