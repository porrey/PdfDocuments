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

namespace PdfDocuments
{
	public static class PdfSectionExtensions
	{
		public static PdfBounds ApplyMargins<TModel>(this IPdfSection<TModel> section, PdfGridPage gridPage, TModel model, PdfSpacing margin)
			where TModel : IPdfModel
		{
			PdfBounds returnValue = section.ActualBounds;

			//
			// Don't apply a margin to an item aligned to the left edge.
			//
			int left = section.ActualBounds.LeftColumn + margin.Left;

			//
			// Don't apply a margin to an item aligned to the top edge.
			//
			int top = section.ActualBounds.TopRow + margin.Top;

			//
			// Don't apply a margin to an item aligned to the right edge.
			//
			int columns = section.ActualBounds.Columns - (margin.Left + margin.Right);

			//
			// Don't apply a margin to an item aligned to the bottom edge.
			//
			int rows = section.ActualBounds.Rows - (margin.Top + margin.Bottom);

			//
			// Create the bounds.
			//
			returnValue = (new PdfBounds(left, top, columns, rows)).Normalize();

			return returnValue;
		}

		public static PdfBounds ApplyPadding<TModel>(this IPdfSection<TModel> section, PdfGridPage gridPage, TModel model, PdfBounds bounds, PdfSpacing padding)
		{
			PdfBounds returnValue = bounds;

			int l = bounds.LeftColumn + padding.Left;
			int t = bounds.TopRow + padding.Top;
			int w = bounds.Columns - (padding.Left + padding.Right);
			int h = bounds.Rows - (padding.Top + padding.Bottom);

			returnValue = new PdfBounds()
			{
				LeftColumn = l >= 0 ? l : 0,
				TopRow = t >= 0 ? t : 0,
				Columns = w > 0 ? w : 1,
				Rows = h > 0 ? h : 1
			};

			return returnValue;
		}
	}
}
