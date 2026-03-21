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
	/// Provides extension methods for manipulating and aligning PDF bounds and coordinates.
	/// </summary>
	/// <remarks>The methods in this class support common operations for positioning, normalizing, and adjusting PDF
	/// layout bounds. These extensions are intended to simplify layout calculations when working with PDF grids, pages,
	/// and alignment scenarios.</remarks>
	public static class PdfBoundsExtensions
	{
		/// <summary>
		/// Calculates the top-left point for horizontally aligning the inner bounds within the outer bounds according to the
		/// specified alignment.
		/// </summary>
		/// <remarks>The vertical position is always aligned to the top of the outer bounds. This method does not
		/// modify the original bounds; it only calculates the alignment point.</remarks>
		/// <param name="outerBounds">The bounds within which the inner bounds will be aligned. Represents the container area.</param>
		/// <param name="innerBounds">The bounds to be aligned horizontally inside the outer bounds. Represents the content area.</param>
		/// <param name="alignment">The horizontal alignment to apply when positioning the inner bounds within the outer bounds. Supported values are
		/// Left, Center, and Right.</param>
		/// <returns>A PdfPoint representing the top-left position where the inner bounds should be placed to achieve the specified
		/// horizontal alignment within the outer bounds.</returns>
		public static PdfPoint AlignHorizontally(this PdfBounds outerBounds, PdfBounds innerBounds, PdfHorizontalAlignment alignment)
		{
			PdfPoint returnValue = new() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case PdfHorizontalAlignment.Left:
					returnValue.Column = outerBounds.LeftColumn;
					returnValue.Row = outerBounds.TopRow;
					break;
				case PdfHorizontalAlignment.Center:
					returnValue.Column = outerBounds.LeftColumn + (int)((outerBounds.Columns - innerBounds.Columns) / 2.0);
					returnValue.Row = outerBounds.TopRow;
					break;
				case PdfHorizontalAlignment.Right:
					returnValue.Column = outerBounds.RightColumn - innerBounds.Columns;
					returnValue.Row = outerBounds.TopRow;
					break;
			}

			return returnValue;
		}

		/// <summary>
		/// Calculates the top-left point for positioning the inner bounds within the outer bounds according to the specified
		/// vertical alignment.
		/// </summary>
		/// <remarks>The returned point always uses the left column of the outer bounds for horizontal positioning.
		/// Use this method to determine the starting row for vertical alignment scenarios such as top, center, or bottom
		/// placement.</remarks>
		/// <param name="outerBounds">The bounds within which the inner bounds are to be aligned. Represents the container area.</param>
		/// <param name="innerBounds">The bounds to be aligned vertically inside the outer bounds. Represents the content area.</param>
		/// <param name="alignment">The vertical alignment to apply when positioning the inner bounds within the outer bounds. Specifies whether to
		/// align to the top, center, or bottom.</param>
		/// <returns>A PdfPoint representing the top-left position where the inner bounds should be placed within the outer bounds
		/// based on the specified vertical alignment.</returns>
		public static PdfPoint AlignVertically(this PdfBounds outerBounds, PdfBounds innerBounds, PdfVerticalAlignment alignment)
		{
			PdfPoint returnValue = new() { Column = innerBounds.LeftColumn, Row = innerBounds.TopRow };

			switch (alignment)
			{
				case PdfVerticalAlignment.Top:
					returnValue.Row = outerBounds.TopRow;
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case PdfVerticalAlignment.Center:
					returnValue.Row = outerBounds.TopRow + (int)((outerBounds.Rows - innerBounds.Rows) / 2.0);
					returnValue.Column = outerBounds.LeftColumn;
					break;
				case PdfVerticalAlignment.Bottom:
					returnValue.Row = outerBounds.BottomRow - innerBounds.Rows;
					returnValue.Column = outerBounds.LeftColumn;
					break;
			}

			return returnValue;
		}

		/// <summary>
		/// Sets the top row of the specified bounds and returns a normalized result.
		/// </summary>
		/// <remarks>Normalization ensures the bounds are valid after updating the top row. The original bounds
		/// instance is not modified.</remarks>
		/// <param name="bounds">The bounds to update with the new top row value.</param>
		/// <param name="topRow">The row index to set as the top row. Must be within the valid range for the bounds.</param>
		/// <returns>A new instance of PdfBounds with the top row set to the specified value and normalized.</returns>
		public static PdfBounds WithTopRow(this PdfBounds bounds, int topRow)
		{
			bounds.TopRow = topRow;
			return bounds.Normalize();
		}

		/// <summary>
		/// Returns a new set of PDF bounds with the left column set to the specified value.
		/// </summary>
		/// <remarks>Normalization ensures the resulting bounds are valid and consistent. Use this method to adjust
		/// the left boundary while preserving other bound properties.</remarks>
		/// <param name="bounds">The current PDF bounds to modify.</param>
		/// <param name="leftColumn">The column index to set as the left boundary. Must be a valid column index within the bounds.</param>
		/// <returns>A normalized PdfBounds instance with the left column updated to the specified value.</returns>
		public static PdfBounds WithLeftColumn(this PdfBounds bounds, int leftColumn)
		{
			bounds.TopRow = leftColumn;
			return bounds.Normalize();
		}

		/// <summary>
		/// Returns a normalized copy of the specified bounds, ensuring all values are within valid ranges.
		/// </summary>
		/// <remarks>Normalization sets negative column or row indices to 0, and column or row counts less than or
		/// equal to 0 to 1. This helps prevent invalid bounds when working with PDF layouts.</remarks>
		/// <param name="bounds">The bounds to normalize. Column and row indices must be non-negative; column and row counts must be positive.</param>
		/// <returns>A new PdfBounds instance with non-negative column and row indices, and positive column and row counts.</returns>
		public static PdfBounds Normalize(this PdfBounds bounds)
		{
			return new PdfBounds()
			{
				LeftColumn = bounds.LeftColumn >= 0 ? bounds.LeftColumn : 0,
				TopRow = bounds.TopRow >= 0 ? bounds.TopRow : 0,
				Columns = bounds.Columns > 0 ? bounds.Columns : 1,
				Rows = bounds.Rows > 0 ? bounds.Rows : 1
			};
		}

		/// <summary>
		/// Calculates a new set of bounds by subtracting the specified inner spacing from the given outer bounds.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model used for grid calculations. Must implement the IPdfModel interface.</typeparam>
		/// <param name="outerBounds">The original bounds from which the inner spacing will be subtracted.</param>
		/// <param name="g">The PDF grid page associated with the bounds calculation.</param>
		/// <param name="m">The PDF model instance used for grid calculations.</param>
		/// <param name="innerBounds">The spacing to subtract from the outer bounds. Specifies the amount to remove from each side.</param>
		/// <returns>A PdfBounds instance representing the adjusted bounds after subtracting the specified inner spacing.</returns>
		public static PdfBounds SubtractBounds<TModel>(this PdfBounds outerBounds, PdfGridPage g, TModel m, PdfSpacing innerBounds)
			where TModel : IPdfModel
		{
			PdfBounds returnValue = outerBounds;

			int left = outerBounds.LeftColumn + innerBounds.Left;
			int top = outerBounds.TopRow + innerBounds.Top;
			int columns = outerBounds.Columns - (innerBounds.Left + innerBounds.Right);
			int rows = outerBounds.Rows - (innerBounds.Top + innerBounds.Bottom);

			returnValue = (new PdfBounds(left, top, columns, rows)).Normalize();

			return returnValue;
		}

		/// <summary>
		/// Creates a new set of bounds by expanding the specified outer bounds using the provided inner spacing values.
		/// </summary>
		/// <typeparam name="TModel">The type of the model associated with the PDF grid page. Must implement the IPdfModel interface.</typeparam>
		/// <param name="outerBounds">The original bounds to be expanded.</param>
		/// <param name="g">The PDF grid page to which the bounds and model are related.</param>
		/// <param name="m">The model instance associated with the PDF grid page.</param>
		/// <param name="innerBounds">The spacing values to apply to each side of the outer bounds when expanding.</param>
		/// <returns>A new PdfBounds instance representing the expanded bounds after applying the specified inner spacing.</returns>
		public static PdfBounds AddBounds<TModel>(this PdfBounds outerBounds, PdfGridPage g, TModel m, PdfSpacing innerBounds)
			where TModel : IPdfModel
		{
			PdfBounds returnValue = outerBounds;

			int left = outerBounds.LeftColumn - innerBounds.Left;
			int top = outerBounds.TopRow - innerBounds.Top;
			int columns = outerBounds.Columns + (innerBounds.Left + innerBounds.Right);
			int rows = outerBounds.Rows + (innerBounds.Top + innerBounds.Bottom);

			returnValue = (new PdfBounds(left, top, columns, rows)).Normalize();

			return returnValue;
		}
	}
}
