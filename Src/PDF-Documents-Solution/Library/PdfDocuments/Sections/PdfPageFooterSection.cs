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
	public class PdfPageFooterSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public virtual BindProperty<string, TModel> TopLeftText { get; set; } = string.Empty;
		public virtual BindProperty<string, TModel> TopRightText { get; set; } = string.Empty;
		public virtual BindProperty<string, TModel> BottomLeftText { get; set; } = string.Empty;
		public virtual BindProperty<string, TModel> BottomRightText { get; set; } = string.Empty;

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfSpacing padding = style.Padding.Resolve(g, m);
			XFont font = style.Font.Resolve(g, m);
			PdfBounds textBounds = bounds.SubtractBounds(g, m, padding);

			//
			// Top left.
			//
			g.DrawText(this.TopLeftText.Resolve(g, m), font, textBounds, XStringFormats.TopLeft, style.ForegroundColor.Resolve(g, m));

			//
			// Top right.
			//
			g.DrawText(this.TopRightText.Resolve(g, m), font, textBounds, XStringFormats.TopRight, style.ForegroundColor.Resolve(g, m));

			//
			// Bottom left
			//
			g.DrawText(this.BottomLeftText.Resolve(g, m), font, textBounds, XStringFormats.BottomLeft, style.ForegroundColor.Resolve(g, m));

			//
			// Bottom right.
			//
			g.DrawText(this.BottomRightText.Resolve(g, m), font, textBounds, XStringFormats.BottomRight, style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
