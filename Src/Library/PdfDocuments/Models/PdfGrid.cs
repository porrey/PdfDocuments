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
	/// Represents a rectangular grid layout for arranging sections within a PDF document. Provides methods and properties
	/// for calculating cell positions and dimensions based on the specified grid size and offsets.
	/// </summary>
	/// <remarks>Use the grid to determine the coordinates and sizes of individual cells or groups of cells when
	/// rendering content in a PDF. The grid supports configurable width, height, row and column counts, and optional
	/// offsets for positioning. All calculations assume uniform cell sizes across the grid. This class is useful for
	/// scenarios such as table layouts, form fields, or structured content placement.</remarks>
	public class PdfGrid
	{
		/// <summary>
		/// Initializes a new instance of the PdfGrid class with the specified dimensions and grid layout.
		/// </summary>
		/// <param name="width">The width of the grid, in points. Must be a positive value.</param>
		/// <param name="height">The height of the grid, in points. Must be a positive value.</param>
		/// <param name="rows">The number of rows in the grid. Must be greater than zero.</param>
		/// <param name="columns">The number of columns in the grid. Must be greater than zero.</param>
		public PdfGrid(double width, double height, int rows, int columns)
		{
			this.Width = width;
			this.Height = height;
			this.Rows = rows;
			this.Columns = columns;
		}

		/// <summary>
		/// Initializes a new instance of the PdfGrid class with the specified dimensions, offsets, and grid layout.
		/// </summary>
		/// <param name="width">The total width of the grid, in points. Must be positive.</param>
		/// <param name="height">The total height of the grid, in points. Must be positive.</param>
		/// <param name="xOffset">The horizontal offset, in points, from the origin where the grid starts.</param>
		/// <param name="yOffset">The vertical offset, in points, from the origin where the grid starts.</param>
		/// <param name="rows">The number of rows in the grid. Must be greater than zero.</param>
		/// <param name="columns">The number of columns in the grid. Must be greater than zero.</param>
		public PdfGrid(double width, double height, double xOffset, double yOffset, int rows, int columns)
		{
			this.Width = width;
			this.Height = height;
			this.XOffset = xOffset;
			this.YOffset = yOffset;
			this.Rows = rows;
			this.Columns = columns;
		}

		/// <summary>
		/// Gets or sets the horizontal offset value.
		/// </summary>
		public double XOffset { get; set; }

		/// <summary>
		/// Gets or sets the vertical offset value.
		/// </summary>
		public double YOffset { get; set; }

		/// <summary>
		/// Gets or sets the width of the element.
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Gets or sets the height value for the object.
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Gets or sets the number of rows.
		/// </summary>
		public int Rows { get; set; }

		/// <summary>
		/// Gets or sets the number of columns in the layout.
		/// </summary>
		public int Columns { get; set; }

		/// <summary>
		/// Calculates the horizontal position of the left edge of the specified column.
		/// </summary>
		/// <remarks>The returned value is based on the current X offset and column width. Use this method to align
		/// sections or determine layout positions for columns.</remarks>
		/// <param name="columnIndex">The zero-based index of the column for which to determine the left edge position. Must be greater than or equal to
		/// 1.</param>
		/// <returns>A double value representing the horizontal offset of the left edge of the specified column.</returns>
		public virtual double Left(int columnIndex)
		{
			return this.XOffset + ((columnIndex - 1) * this.ColumnWidth);
		}

		/// <summary>
		/// Calculates the rightmost X-coordinate of the specified column within the layout.
		/// </summary>
		/// <param name="columnIndex">The zero-based index of the column for which to determine the right edge. Must be greater than or equal to 0.</param>
		/// <returns>The X-coordinate representing the right edge of the specified column.</returns>
		public virtual double Right(int columnIndex)
		{
			return this.XOffset + (columnIndex * this.ColumnWidth);
		}

		/// <summary>
		/// Calculates the vertical position of the specified row within the layout.
		/// </summary>
		/// <remarks>The calculation uses the current row height and Y offset. This method is useful for determining
		/// the placement of rows in a grid or table layout.</remarks>
		/// <param name="rowIndex">The zero-based index of the row for which to determine the vertical position. Must be greater than or equal to 1.</param>
		/// <returns>The vertical offset, in pixels, from the top of the layout to the specified row.</returns>
		public virtual double Top(int rowIndex)
		{
			return this.YOffset + ((rowIndex - 1) * this.RowHeight);
		}

		/// <summary>
		/// Calculates the vertical position of the bottom edge of the specified row.
		/// </summary>
		/// <param name="rowIndex">The zero-based index of the row for which to determine the bottom position. Must be greater than or equal to zero.</param>
		/// <returns>The vertical coordinate representing the bottom edge of the specified row, relative to the current offset.</returns>
		public virtual double Bottom(int rowIndex)
		{
			return this.YOffset + (rowIndex * this.RowHeight);
		}

		/// <summary>
		/// Gets the width of a single column, calculated based on the total width and the number of columns.
		/// </summary>
		public virtual double ColumnWidth => this.Width / this.Columns;

		/// <summary>
		/// Calculates the total width of the specified number of columns.
		/// </summary>
		/// <param name="columnCount">The number of columns to include in the calculation. Must be greater than or equal to zero.</param>
		/// <returns>The combined width of all columns, calculated as the product of the column width and the specified column count.</returns>
		public virtual double ColumnsWidth(int columnCount)
		{
			return this.ColumnWidth * columnCount;
		}

		/// <summary>
		/// Gets the height of each row in the layout, calculated based on the total height and the number of rows.
		/// </summary>
		public virtual double RowHeight => this.Height / this.Rows;

		/// <summary>
		/// Calculates the total height of multiple rows based on the specified row count.
		/// </summary>
		/// <param name="rowCount">The number of rows for which to calculate the total height. Must be non-negative.</param>
		/// <returns>The combined height of all rows, calculated as the product of the row count and the row height.</returns>
		public virtual double RowsHeight(int rowCount)
		{
			return this.RowHeight * rowCount;
		}

		/// <summary>
		/// Returns the bounding rectangle for the current PDF grid based on its column and row configuration.
		/// </summary>
		/// <returns>A <see cref="PdfBounds"/> instance representing the bounds of the grid. The bounds reflect the current number of
		/// columns and rows.</returns>
		public virtual PdfBounds GetBounds()
		{
			return new PdfBounds(1, 1, this.Columns, this.Rows);
		}
	}
}
