﻿/*
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

namespace PdfDocuments
{
	public class PdfReportHeaderSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfReportHeaderSection()
		{
			this.RelativeHeight = .05;
			this.ShouldRender = new BindPropertyAction<bool, TModel>((gp, m) => { return gp.PageNumber == 1; });
		}

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Draw the title.
			//
			PdfStyle<TModel> style = this.StyleManager.GetStyle(this.StyleNames.First());
			g.DrawText(g.DocumentTitle.ToUpper(), style.Font.Resolve(g, m), bounds.LeftColumn, bounds.TopRow, bounds.Columns, bounds.Rows, style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}
	}
}
