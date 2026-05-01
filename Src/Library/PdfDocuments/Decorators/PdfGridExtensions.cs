using PdfSharp.Drawing;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for the PdfGrid class to facilitate coordinate and rectangle calculations for grid
	/// regions and cells.
	/// </summary>
	/// <remarks>These methods simplify working with PDF grid layouts by converting grid-based bounds and cell
	/// positions into XRect and XPoint structures. They are intended to assist with rendering and layout calculations when
	/// generating PDF documents.</remarks>
	public static class PdfGridExtensions
	{
		/// <summary>
		/// Calculates the bounding rectangle for the specified grid region defined by the given bounds.
		/// </summary>
		/// <remarks>The returned rectangle corresponds to the area covering the columns and rows defined in the
		/// bounds. Use this method to obtain the exact coordinates and dimensions for rendering or layout purposes.</remarks>
		/// <param name="grid">The grid from which to calculate the rectangle. Cannot be null.</param>
		/// <param name="bounds">The bounds specifying the region within the grid, including columns and rows to include in the rectangle.</param>
		/// <returns>An XRect representing the position and size of the specified grid region.</returns>
		public static XRect GetRect(this PdfGrid grid, PdfBounds bounds)
		{
			//
			// Create the rectangle.
			//
			double x = grid.Left(bounds.LeftColumn);
			double y = grid.Top(bounds.TopRow);
			double w = grid.ColumnsWidth(bounds.Columns) > 0 ? grid.ColumnsWidth(bounds.Columns) : 1;
			double h = grid.RowsHeight(bounds.Rows) > 0 ? grid.RowsHeight(bounds.Rows) : 1;

			return new XRect(x, y, w, h);
		}

		/// <summary>
		/// Returns the bounding rectangle for the specified range of columns and rows in the grid.
		/// </summary>
		/// <remarks>If the calculated width or height is less than or equal to zero, a minimum value of 1 is used for
		/// that dimension.</remarks>
		/// <param name="grid">The grid from which to calculate the rectangle. Cannot be null.</param>
		/// <param name="leftColumn">The zero-based index of the leftmost column in the range.</param>
		/// <param name="topRow">The zero-based index of the topmost row in the range.</param>
		/// <param name="rightColumn">The zero-based index of the rightmost column in the range.</param>
		/// <param name="bottomRow">The zero-based index of the bottommost row in the range.</param>
		/// <returns>An XRect representing the area covered by the specified columns and rows in the grid.</returns>
		public static XRect GetRect(this PdfGrid grid, int leftColumn, int topRow, int rightColumn, int bottomRow)
		{
			//
			// Create the rectangle.
			//
			double x = grid.Left(leftColumn);
			double y = grid.Top(topRow);
			double w = grid.Right(rightColumn) - x > 0 ? grid.Right(rightColumn) - x : 1;
			double h = grid.Bottom(bottomRow) - y > 0 ? grid.Bottom(bottomRow) - y : 1;

			return new XRect(x, y, w, h);
		}

		/// <summary>
		/// Calculates the X and Y coordinates of the top-left corner of the specified grid cell bounds.
		/// </summary>
		/// <param name="grid">The grid from which to calculate the coordinates. Cannot be null.</param>
		/// <param name="bounds">The bounds specifying the cell range for which to obtain the coordinates.</param>
		/// <returns>An XPoint representing the X and Y coordinates of the top-left corner of the specified cell bounds.</returns>
		public static XPoint GetPoint(this PdfGrid grid, PdfBounds bounds)
		{
			return new()
			{
				X = grid.Left(bounds.LeftColumn),
				Y = grid.Top(bounds.TopRow)
			};
		}

		/// <summary>
		/// Gets the coordinates of the top-left corner of the specified cell in the grid.
		/// </summary>
		/// <param name="grid">The grid from which to retrieve the cell coordinates. Cannot be null.</param>
		/// <param name="column">The zero-based index of the column containing the cell.</param>
		/// <param name="row">The zero-based index of the row containing the cell.</param>
		/// <returns>An XPoint representing the X and Y coordinates of the top-left corner of the specified cell.</returns>
		public static XPoint GetXPoint(this PdfGrid grid, int column, int row)
		{
			return new()
			{
				X = grid.Left(column),
				Y = grid.Top(row)
			};
		}

		/// <summary>
		/// Returns the bounding rectangle for the current PDF grid based on its column and row configuration.
		/// </summary>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <returns>A <see cref="PdfBounds"/> instance representing the bounds of the grid. The bounds reflect the current number of
		/// columns and rows.</returns>
		public static PdfBounds GetBounds(this PdfGrid grid)
		{
			return new PdfBounds(1, 1, grid.Columns, grid.Rows);
		}

		/// <summary>
		/// Calculates the horizontal position of the left edge of the specified column.
		/// </summary>
		/// <remarks>The returned value is based on the current X offset and column width. Use this method to align
		/// sections or determine layout positions for columns.</remarks>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="columnIndex">The zero-based index of the column for which to determine the left edge position. Must be greater than or equal to
		/// 1.</param>
		/// <returns>A double value representing the horizontal offset of the left edge of the specified column.</returns>
		public static double Left(this PdfGrid grid, int columnIndex)
		{
			return grid.XOffset + ((columnIndex - 1) * grid.ColumnWidth);
		}

		/// <summary>
		/// Calculates the rightmost X-coordinate of the specified column within the layout.
		/// </summary>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="columnIndex">The zero-based index of the column for which to determine the right edge. Must be greater than or equal to 0.</param>
		/// <returns>The X-coordinate representing the right edge of the specified column.</returns>
		public static double Right(this PdfGrid grid, int columnIndex)
		{
			return grid.XOffset + (columnIndex * grid.ColumnWidth);
		}

		/// <summary>
		/// Calculates the vertical position of the specified row within the layout.
		/// </summary>
		/// <remarks>The calculation uses the current row height and Y offset. This method is useful for determining
		/// the placement of rows in a grid or table layout.</remarks>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="rowIndex">The zero-based index of the row for which to determine the vertical position. Must be greater than or equal to 1.</param>
		/// <returns>The vertical offset, in pixels, from the top of the layout to the specified row.</returns>
		public static double Top(this PdfGrid grid, int rowIndex)
		{
			return grid.YOffset + ((rowIndex - 1) * grid.RowHeight);
		}

		/// <summary>
		/// Calculates the vertical position of the bottom edge of the specified row.
		/// </summary>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="rowIndex">The zero-based index of the row for which to determine the bottom position. Must be greater than or equal to zero.</param>
		/// <returns>The vertical coordinate representing the bottom edge of the specified row, relative to the current offset.</returns>
		public static double Bottom(this PdfGrid grid, int rowIndex)
		{
			return grid.YOffset + (rowIndex * grid.RowHeight);
		}

		/// <summary>
		/// Calculates the total width of the specified number of columns.
		/// </summary>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="columnCount">The number of columns to include in the calculation. Must be greater than or equal to zero.</param>
		/// <returns>The combined width of all columns, calculated as the product of the column width and the specified column count.</returns>
		public static double ColumnsWidth(this PdfGrid grid, int columnCount)
		{
			return grid.ColumnWidth * columnCount;
		}

		/// <summary>
		/// Calculates the total height of multiple rows based on the specified row count.
		/// </summary>
		/// <param name="grid">The PDF grid for which to calculate the column width. Cannot be null.</param>
		/// <param name="rowCount">The number of rows for which to calculate the total height. Must be non-negative.</param>
		/// <returns>The combined height of all rows, calculated as the product of the row count and the row height.</returns>
		public static double RowsHeight(this PdfGrid grid, int rowCount)
		{
			return grid.RowHeight * rowCount;
		}
	}
}
