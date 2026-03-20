/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for the XSize structure to facilitate grid creation and related operations.
	/// </summary>
	/// <remarks>This class contains static methods that extend the functionality of XSize, enabling convenient
	/// integration with PDF grid layouts. All methods are static and intended for use with XSize instances.</remarks>
	public static class XSizeExtensions
	{
		/// <summary>
		/// Creates a new PDF grid with the specified bounds, number of columns, and number of rows.
		/// </summary>
		/// <param name="bounds">The size of the grid, specifying its width and height in points.</param>
		/// <param name="columns">The number of columns to include in the grid. Must be greater than zero.</param>
		/// <param name="rows">The number of rows to include in the grid. Must be greater than zero.</param>
		/// <returns>A new instance of PdfGrid representing a grid with the specified dimensions and layout.</returns>
		public static PdfGrid CreateGrid(this XSize bounds, int columns, int rows)
		{
			return new PdfGrid(bounds.Width, bounds.Height, rows, columns);
		}
	}
}
