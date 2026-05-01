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
	/// Provides extension methods for drawing rectangles, lines, and grids on a PDF grid page using specified bounds,
	/// colors, and pens.
	/// </summary>
	/// <remarks>These extension methods simplify rendering shapes and lines on a PDF grid page by abstracting
	/// coordinate calculations and drawing logic. Methods support both filled and outlined rectangles, horizontal and
	/// vertical lines, and full grid rendering. All drawing operations are performed relative to the grid's cell
	/// structure, allowing for precise placement within the page layout. Thread safety is not guaranteed; ensure that
	/// drawing operations are performed in a single-threaded context.</remarks>
	public static class PdfGridPageLineExtensions
	{
		/// <summary>
		/// Draws a filled rectangle on the specified PDF grid page using the given bounds and color.
		/// </summary>
		/// <param name="source">The PDF grid page on which the filled rectangle will be drawn.</param>
		/// <param name="bounds">The bounds defining the rectangle's position and size within the grid page.</param>
		/// <param name="color">The color used to fill the rectangle.</param>
		public static void DrawFilledRectangle(this PdfGridPage source, PdfBounds bounds, XColor color)
		{
			source.DrawFilledRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, color);
		}

		/// <summary>
		/// Draws a filled rectangle on the specified PDF grid page using the given grid coordinates and color.
		/// </summary>
		/// <remarks>The rectangle is drawn based on the grid's column and row indices, allowing alignment with grid
		/// cells. The rectangle will cover all cells from the specified left and top indices to the right and bottom indices,
		/// inclusive.</remarks>
		/// <param name="source">The PDF grid page on which the rectangle will be drawn.</param>
		/// <param name="leftColumn">The index of the leftmost column of the rectangle within the grid.</param>
		/// <param name="topRow">The index of the topmost row of the rectangle within the grid.</param>
		/// <param name="rightColumn">The index of the rightmost column of the rectangle within the grid.</param>
		/// <param name="bottomRow">The index of the bottommost row of the rectangle within the grid.</param>
		/// <param name="color">The color used to fill the rectangle.</param>
		public static void DrawFilledRectangle(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow, XColor color)
		{
			//
			// Create the rectangle.
			//
			XRect rect = new(source.Grid.Left(leftColumn), source.Grid.Top(topRow), source.Grid.Right(rightColumn) - source.Grid.Left(leftColumn), source.Grid.Bottom(bottomRow) - source.Grid.Top(topRow));

			//
			// Draw the rectangle.
			//
			source.Graphics.DrawRectangle(new XPen(color, 1), rect);
			source.Graphics.DrawRectangle(new XSolidBrush(color), rect);
		}

		/// <summary>
		/// Draws a filled rectangle on the specified PDF grid page, applying a background color and overlaying diagonal line
		/// patterns within the given bounds.
		/// </summary>
		/// <remarks>The rectangle is aligned to the specified grid columns and rows. The diagonal line pattern is
		/// drawn from the lower left to the upper right within the rectangle. This method does not return a value and
		/// modifies the graphics state of the provided PDF grid page.</remarks>
		/// <param name="source">The PDF grid page on which to draw the filled rectangle.</param>
		/// <param name="bounds">The grid-based bounds that define the rectangle's position and size on the page.</param>
		/// <param name="backgroundColor">The color used to fill the interior of the rectangle.</param>
		/// <param name="lineColor">The color used for the diagonal line pattern drawn over the filled rectangle.</param>
		/// <param name="spacing">The distance, in points, between adjacent diagonal lines in the overlay pattern. Must be positive. The default is
		/// 6.</param>
		/// <param name="lineWidth">The width, in points, of the diagonal lines in the overlay pattern. Must be positive. The default is 0.5.</param>
		public static void DrawFilledRectangle(this PdfGridPage source, PdfBounds bounds, XColor backgroundColor, XColor lineColor, double spacing = 12, double lineWidth = 0.5)
		{
			source.Graphics.Save();

			//
			// Create the rectangle.
			//
			XRect rect = new(source.Grid.Left(bounds.LeftColumn), source.Grid.Top(bounds.TopRow), source.Grid.Right(bounds.RightColumn) - source.Grid.Left(bounds.LeftColumn), source.Grid.Bottom(bounds.BottomRow) - source.Grid.Top(bounds.TopRow));

			//
			// Set the clipping region to the rectangle to ensure that drawing operations are confined within it.
			//
			source.Graphics.IntersectClip(rect);

			//
			// Fill the rectangle with the background color.
			//
			XBrush backgroundBrush = new XSolidBrush(backgroundColor);
			source.Graphics.DrawRectangle(backgroundBrush, rect);

			//
			// Draw diagonal lines across the rectangle at the specified spacing and line width.
			//
			XPen patternPen = new(lineColor, lineWidth);

			double height = rect.Height;
			double startX = rect.Left - height;
			double endX = rect.Right;

			for (double x = startX; x <= endX; x += spacing)
			{
				XPoint point1 = new(x, rect.Bottom);
				XPoint point2 = new(x + height, rect.Top);
				source.Graphics.DrawLine(patternPen, point1, point2);
			}

			source.Graphics.Restore();
		}

		/// <summary>
		/// Draws a filled rectangle with a hollow center on the specified PDF grid page, using the given colors and line
		/// pattern.
		/// </summary>
		/// <remarks>The method fills the area between the outer and inner rectangles and overlays diagonal lines,
		/// leaving the inner rectangle transparent. The drawing is clipped so that lines and fill do not appear inside the
		/// inner rectangle. This method does not modify the PDF page outside the specified bounds.</remarks>
		/// <param name="source">The PDF grid page on which to draw the filled rectangle.</param>
		/// <param name="outerBounds">The grid bounds that define the outer rectangle to be filled and outlined.</param>
		/// <param name="innerBounds">The grid bounds that define the inner rectangle to be excluded from filling and pattern drawing, creating a hollow
		/// center.</param>
		/// <param name="backgroundColor">The background color used to fill the area between the outer and inner rectangles.</param>
		/// <param name="lineColor">The color of the diagonal lines drawn across the filled area.</param>
		/// <param name="spacing">The spacing, in points, between diagonal lines in the pattern. The default is 6.</param>
		/// <param name="lineWidth">The width, in points, of the diagonal lines. The default is 0.5.</param>
		public static void DrawFilledRectangle(this PdfGridPage source, PdfBounds outerBounds, PdfBounds innerBounds, XColor backgroundColor, XColor lineColor, double spacing = 6, double lineWidth = 0.5)
		{
			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				//
				// Create the outer rectangle.
				//
				XRect outerRect = new(
					source.Grid.Left(outerBounds.LeftColumn),
					source.Grid.Top(outerBounds.TopRow),
					source.Grid.Right(outerBounds.RightColumn) - source.Grid.Left(outerBounds.LeftColumn),
					source.Grid.Bottom(outerBounds.BottomRow) - source.Grid.Top(outerBounds.TopRow));

				//
				// Create the inner rectangle.
				//
				XRect innerRect = new(
					source.Grid.Left(innerBounds.LeftColumn),
					source.Grid.Top(innerBounds.TopRow),
					source.Grid.Right(innerBounds.RightColumn) - source.Grid.Left(innerBounds.LeftColumn),
					source.Grid.Bottom(innerBounds.BottomRow) - source.Grid.Top(innerBounds.TopRow));

				//
				// Create a clipping path where the outer rectangle is included and the inner rectangle is excluded.
				//
				XGraphicsPath clipPath = new();
				clipPath.AddRectangle(outerRect);
				clipPath.AddRectangle(innerRect);

				//
				// Use Alternate fill mode so the second rectangle becomes a hole in the clipping region.
				//
				clipPath.FillMode = XFillMode.Alternate;

				//
				// Set the clipping region to the combined path, ensuring that drawing operations only
				// affect the area between the outer and inner rectangles.
				//
				source.Graphics.IntersectClip(clipPath);
				
				//
				// Fill only the outer ring area with the background color.
				//
				XBrush backgroundBrush = new XSolidBrush(backgroundColor);
				source.Graphics.DrawRectangle(backgroundBrush, outerRect);

				//
				// Draw diagonal lines across the outer rectangle. The clipping path prevents them
				// from drawing inside the inner rectangle.
				//
				XPen patternPen = new(lineColor, lineWidth);

				double height = outerRect.Height;
				double startX = outerRect.Left - height;
				double endX = outerRect.Right;

				for (double x = startX; x <= endX; x += spacing)
				{
					XPoint point1 = new(x, outerRect.Bottom);
					XPoint point2 = new(x + height, outerRect.Top);

					source.Graphics.DrawLine(patternPen, point1, point2);
				}
			}
			finally
			{
				source.Graphics.Restore(state);
			}
		}

		/// <summary>
		/// Draws a rectangle on the specified PDF grid page using the provided bounds and pen.
		/// </summary>
		/// <param name="source">The PDF grid page on which the rectangle will be drawn.</param>
		/// <param name="bounds">The bounds that define the position and size of the rectangle within the grid page.</param>
		/// <param name="pen">The pen used to outline the rectangle. Cannot be null.</param>
		public static void DrawRectangle(this PdfGridPage source, PdfBounds bounds, XPen pen)
		{
			source.DrawRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, pen);
		}

		/// <summary>
		/// Draws a rectangle on the specified PDF grid page using the given bounds, line weight, and color.
		/// </summary>
		/// <param name="source">The PDF grid page on which the rectangle will be drawn.</param>
		/// <param name="bounds">The bounds defining the position and size of the rectangle within the grid page.</param>
		/// <param name="weight">The thickness of the rectangle's outline, in points. Must be greater than zero.</param>
		/// <param name="color">The color used for the rectangle's outline.</param>
		public static void DrawRectangle(this PdfGridPage source, PdfBounds bounds, double weight, XColor color)
		{
			XPen pen = new(color, weight);
			source.DrawRectangle(bounds.LeftColumn, bounds.TopRow, bounds.RightColumn, bounds.BottomRow, pen);
		}

		/// <summary>
		/// Draws a rectangle on the specified PDF grid page using the given pen and grid coordinates.
		/// </summary>
		/// <remarks>The rectangle is only drawn if the specified grid coordinates define a non-empty area. The method
		/// does not modify the contents of the grid cells.</remarks>
		/// <param name="source">The PDF grid page on which the rectangle will be drawn.</param>
		/// <param name="leftColumn">The index of the leftmost column of the rectangle within the grid.</param>
		/// <param name="topRow">The index of the topmost row of the rectangle within the grid.</param>
		/// <param name="rightColumn">The index of the rightmost column of the rectangle within the grid.</param>
		/// <param name="bottomRow">The index of the bottommost row of the rectangle within the grid.</param>
		/// <param name="pen">The pen used to outline the rectangle.</param>
		public static void DrawRectangle(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow, XPen pen)
		{
			//
			// Create the rectangle.
			//
			XRect rect = source.GetRect(leftColumn, topRow, rightColumn, bottomRow);

			if (!rect.IsEmpty)
			{
				//
				// Draw the rectangle.
				//
				source.Graphics.DrawRectangle(pen, rect);
			}
		}

		/// <summary>
		/// Draws a vertical line on the specified grid page between two rows at the given column edge.
		/// </summary>
		/// <param name="source">The grid page on which the vertical line will be drawn.</param>
		/// <param name="column">The column index where the vertical line will be placed.</param>
		/// <param name="startRow">The index of the row where the vertical line starts.</param>
		/// <param name="endRow">The index of the row where the vertical line ends.</param>
		/// <param name="columnEdge">Specifies whether the line is drawn at the left or right edge of the column.</param>
		/// <param name="weight">The thickness of the line, in points. Must be greater than zero.</param>
		/// <param name="color">The color of the line.</param>
		public static void DrawVerticalLine(this PdfGridPage source, int column, int startRow, int endRow, PdfColumnEdge columnEdge, double weight, XColor color)
		{
			XPen linePen = new(color, weight);
			XPoint p1 = new(columnEdge == PdfColumnEdge.Right ? source.Grid.Right(column) : source.Grid.Left(column), source.Grid.Top(startRow));
			XPoint p2 = new(columnEdge == PdfColumnEdge.Right ? source.Grid.Right(column) : source.Grid.Left(column), source.Grid.Bottom(endRow));
			source.Graphics.DrawLine(linePen, p1, p2);
		}

		/// <summary>
		/// Draws a horizontal line across the specified row of the grid on the PDF page.
		/// </summary>
		/// <remarks>The line is drawn from the left edge of the start column to the right edge of the end column. Use
		/// this method to visually separate rows or highlight specific sections within the grid.</remarks>
		/// <param name="source">The PDF grid page on which the horizontal line will be drawn.</param>
		/// <param name="row">The index of the row where the line will be placed.</param>
		/// <param name="startColumn">The index of the starting column for the line segment.</param>
		/// <param name="endColumn">The index of the ending column for the line segment.</param>
		/// <param name="rowEdge">Specifies whether the line is drawn at the top or bottom edge of the row.</param>
		/// <param name="weight">The thickness of the line, in points.</param>
		/// <param name="color">The color of the line.</param>
		public static void DrawHorizontalLine(this PdfGridPage source, int row, int startColumn, int endColumn, PdfRowEdge rowEdge, double weight, XColor color)
		{
			XPen linePen = new(color, weight);
			XPoint p1 = new(source.Grid.Left(startColumn), rowEdge == PdfRowEdge.Top ? source.Grid.Top(row) : source.Grid.Bottom(row));
			XPoint p2 = new(source.Grid.Right(endColumn), rowEdge == PdfRowEdge.Top ? source.Grid.Top(row) : source.Grid.Bottom(row));
			source.Graphics.DrawLine(linePen, p1, p2);
		}

		/// <summary>
		/// Draws a complete grid on the specified PDF page using the given color and line weight.
		/// </summary>
		/// <remarks>This method draws both horizontal and vertical lines to outline all rows and columns of the grid.
		/// Use this method to visually separate grid cells on the page. The operation does not modify the grid's data or
		/// structure.</remarks>
		/// <param name="source">The PDF grid page on which the grid lines will be drawn. Must not be null.</param>
		/// <param name="color">The color used for the grid lines.</param>
		/// <param name="weight">The thickness of the grid lines, in points. Must be greater than zero.</param>
		public static void DrawGrid(this PdfGridPage source, XColor color, double weight)
		{
			for (int row = 1; row <= source.Grid.Rows; row++)
			{
				source.DrawHorizontalLine(row, 1, source.Grid.Columns, PdfRowEdge.Top, weight, color);
			}

			source.DrawHorizontalLine(source.Grid.Rows, 1, source.Grid.Columns, PdfRowEdge.Bottom, weight, color);

			for (int column = 1; column <= source.Grid.Columns; column++)
			{
				source.DrawVerticalLine(column, 1, source.Grid.Rows, PdfColumnEdge.Left, weight, color);
			}

			source.DrawVerticalLine(source.Grid.Columns, 1, source.Grid.Rows, PdfColumnEdge.Right, weight, color);
		}

		/// <summary>
		/// Calculates the rectangle that corresponds to the specified grid bounds on the given PDF page.
		/// </summary>
		/// <remarks>The returned rectangle reflects the position and size of the specified grid region within the PDF
		/// page. If the bounds specify columns or rows with zero or negative width or height, the rectangle will default to a
		/// size of 1 in the corresponding dimension.</remarks>
		/// <param name="source">The PDF grid page from which to determine the rectangle. Must not be null.</param>
		/// <param name="bounds">The grid bounds specifying the columns and rows for which to calculate the rectangle. Must not be null.</param>
		/// <returns>An XRect representing the area covered by the specified bounds on the grid. The rectangle will have a minimum
		/// width and height of 1 if the calculated size is zero or negative.</returns>
		public static XRect GetRect(this PdfGridPage source, PdfBounds bounds)
		{
			return source.Grid.GetRect(bounds);
		}

		/// <summary>
		/// Calculates the rectangle that spans the specified grid columns and rows on the given PDF grid page.
		/// </summary>
		/// <remarks>Use this method to obtain the exact rectangle for a cell or range of cells in a PDF grid, which
		/// can be useful for drawing or layout operations. The rectangle coordinates are based on the grid's current
		/// layout.</remarks>
		/// <param name="source">The PDF grid page from which to obtain the grid coordinates.</param>
		/// <param name="leftColumn">The index of the leftmost column of the rectangle. Must be within the valid column range of the grid.</param>
		/// <param name="topRow">The index of the topmost row of the rectangle. Must be within the valid row range of the grid.</param>
		/// <param name="rightColumn">The index of the rightmost column of the rectangle. Must be greater than or equal to leftColumn and within the
		/// valid column range.</param>
		/// <param name="bottomRow">The index of the bottommost row of the rectangle. Must be greater than or equal to topRow and within the valid row
		/// range.</param>
		/// <returns>An XRect representing the area defined by the specified columns and rows. The rectangle will have a minimum width
		/// and height of 1 if the calculated size is less than or equal to zero.</returns>
		public static XRect GetRect(this PdfGridPage source, int leftColumn, int topRow, int rightColumn, int bottomRow)
		{
			return source.Grid.GetRect(leftColumn, topRow, rightColumn, bottomRow);
		}
	}
}
