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
using System.IO;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfPageHeaderSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public virtual BindProperty<string, TModel> Logo { get; set; } = string.Empty;
		public virtual BindProperty<string, TModel> Title { get; set; } = string.Empty;

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the first style for this section
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the default style for this section
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);

			//
			// Draw the image left aligned and vertically centered in the
			// header leaving a 1 row margin above and below it. Also 
			// leave a one column margin on the left.
			//
			string path = this.Logo.Resolve(g, m);

			if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
			{
				g.DrawImageWithFixedHeight(path, bounds.LeftColumn + padding.Left, bounds.TopRow + padding.Top, bounds.Rows - (padding.Top + padding.Bottom));
			}

			if (this.Title != null)
			{
				string title = this.Title.Resolve(g, m);
				PdfBounds textBounds = bounds.SubtractBounds(g, m, padding);
				g.DrawText(title, style.Font.Resolve(g, m), textBounds, style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));
			}

			return Task.FromResult(returnValue);
		}
	}
}
