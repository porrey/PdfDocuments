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
using System.Collections.Generic;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public interface IPdfSection<TModel>
	{
		string Key { get; set; }
		IPdfStyleManager<TModel> StyleManager { get; set; }
		IEnumerable<string> StyleNames { get; set; }
		BindProperty<string, TModel> Text { get; set; }
		PdfBounds ActualBounds { get; set; }
		IPdfSection<TModel> ParentSection { get; set; }
		BindProperty<bool, TModel> ShouldRender { get; set; }
		IList<IPdfSection<TModel>> Children { get; }
		BindProperty<string, TModel> WaterMarkImagePath { get; set; }
		Task<bool> RenderAsync(PdfGridPage gridPage, TModel model);
		Task<bool> LayoutAsync(PdfGridPage gridPage, TModel model);
		Task<bool> RenderDebugAsync(PdfGridPage gridPage, TModel model);
		Task SetActualRows(int rows);
		Task SetActualColumns(int columns);
		BindProperty<double, TModel> RelativeHeight { get; set; }
		BindProperty<double, TModel> RelativeWidth { get; set; }
	}
}