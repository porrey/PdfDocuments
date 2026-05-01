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
		public virtual double XOffset { get; set; }

		/// <summary>
		/// Gets or sets the vertical offset value.
		/// </summary>
		public virtual double YOffset { get; set; }

		/// <summary>
		/// Gets or sets the width of the element.
		/// </summary>
		public virtual double Width { get; set; }

		/// <summary>
		/// Gets or sets the height value for the object.
		/// </summary>
		public virtual double Height { get; set; }

		/// <summary>
		/// Gets or sets the number of rows.
		/// </summary>
		public virtual int Rows { get; set; }

		/// <summary>
		/// Gets or sets the number of columns in the layout.
		/// </summary>
		public virtual int Columns { get; set; }

		/// <summary>
		/// Gets the height of each row in the layout, calculated based on the total height and the number of rows.
		/// </summary>
		public virtual double RowHeight => this.Height / this.Rows;

		/// <summary>
		/// Gets the width of a single column, calculated based on the total width and the number of columns.
		/// </summary>
		public virtual double ColumnWidth => this.Width / this.Columns;
	}
}
