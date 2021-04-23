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
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocuments
{
	public static class PdfGridExtensions
	{
		public static XForm CreateForm(this (IPdfGrid Grid, IPdfBounds Bounds) source, PdfDocument document)
		{
			return new XForm(document, source.Grid.ColumnsWidth(source.Bounds.Columns), source.Grid.RowsHeight(source.Bounds.Rows));
		}

		public static XSize ToXSize(this IPdfGrid grid, int columnCount, int rowCount)
		{
			return new XSize(grid.ColumnsWidth(columnCount), grid.RowsHeight(rowCount));
		}

		public static void DrawForm(this (XGraphics Graphics, IPdfGrid Grid, XForm Form) source, int column, int row)
		{
			source.Graphics.DrawImage(source.Form, source.Grid.Left(column), source.Grid.Top(row));
		}
	}
}
