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
namespace PdfDocuments
{
	/// <summary>
	/// Represents a rectangular region within a PDF document, defined by its position and size in terms of rows and
	/// columns.
	/// </summary>
	/// <remarks>Use this class to specify the bounds of a table, grid, or other layout element in a PDF by
	/// indicating its top-left position and dimensions. The bounds are expressed as integer values for rows and columns,
	/// which can be useful for mapping content or extracting regions from structured PDF layouts.</remarks>
	public class PdfBounds
	{
		/// <summary>
		/// Initializes a new instance of the PdfBounds class.
		/// </summary>
		public PdfBounds()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfBounds class with the specified position and size within a PDF grid.
		/// </summary>
		/// <param name="leftColumn">The zero-based index of the leftmost column of the bounds.</param>
		/// <param name="topRow">The zero-based index of the topmost row of the bounds.</param>
		/// <param name="columns">The number of columns spanned by the bounds. Must be greater than zero.</param>
		/// <param name="rows">The number of rows spanned by the bounds. Must be greater than zero.</param>
		public PdfBounds(int leftColumn, int topRow, int columns, int rows)
		{
			this.TopRow = topRow;
			this.LeftColumn = leftColumn;
			this.Rows = rows;
			this.Columns = columns;
		}

		/// <summary>
		/// Gets or sets the index of the topmost visible row in the control.
		/// </summary>
		public virtual int TopRow { get; set; }

		/// <summary>
		/// Gets or sets the index of the leftmost visible column.
		/// </summary>
		public virtual int LeftColumn { get; set; }

		/// <summary>
		/// Gets or sets the number of rows in the collection.
		/// </summary>
		public virtual int Rows { get; set; }

		/// <summary>
		/// Gets or sets the number of columns in the layout.
		/// </summary>
		public virtual int Columns { get; set; }

		/// <summary>
		/// Gets the index of the rightmost column in the range.
		/// </summary>
		public virtual int RightColumn => this.LeftColumn + this.Columns - 1;

		/// <summary>
		/// Gets the index of the bottom row in the current range.
		/// </summary>
		public virtual int BottomRow => this.TopRow + this.Rows - 1;

		/// <summary>
		/// Returns a string that represents the current object, including the values of the left column, top row, column
		/// count, and row count.
		/// </summary>
		/// <returns>A comma-separated string containing the left column, top row, number of columns, and number of rows.</returns>
		public override string ToString()
		{
			return $"{this.LeftColumn}, {this.TopRow}, {this.Columns}, {this.Rows}";
		}
	}
}
