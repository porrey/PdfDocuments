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
using PdfSharp.Drawing.Layout;
using System.Linq;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfWrappingTextSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfSpacing padding = style.Padding.Resolve(g, m);

			g.DrawWrappingText(this.Text.Resolve(g, m),
				style.Font.Resolve(g, m),
				bounds.LeftColumn + padding.Left,
				bounds.TopRow + padding.Top,
				bounds.Columns - (padding.Left + padding.Right),
				bounds.Rows - (padding.Top + padding.Bottom),
				style.TextAlignment.Resolve(g, m),
				style.ForegroundColor.Resolve(g, m),
				style.ParagraphAlignment.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
