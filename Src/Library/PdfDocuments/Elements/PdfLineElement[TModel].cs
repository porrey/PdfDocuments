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
using System.ComponentModel.DataAnnotations;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	/// <summary>
	/// Represents a line element within a PDF grid, positioned by row or column edge, and supporting customizable line
	/// direction and style.
	/// </summary>
	/// <remarks>Use PdfLineElement to draw horizontal or vertical lines at specific row or column edges within a
	/// PDF grid. The line's appearance and position can be customized using the LineDirection, RowEdge, and ColumnEdge
	/// properties, as well as style settings resolved from the model. This element is typically used to visually separate
	/// or highlight sections within a PDF table or grid layout.</remarks>
	/// <typeparam name="TModel">The type of the model providing context for style resolution and content. Must implement IPdfModel.</typeparam>
	public class PdfLineElement<TModel> : IPdfElement<TModel> where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfLineElement class with the specified row edge.
		/// </summary>
		/// <param name="rowEdge">The row edge to which this line element is associated.</param>
		public PdfLineElement(PdfRowEdge rowEdge)
		{
			this.RowEdge = rowEdge;
			this.LineDirection = PdfLineDirection.Horizontal;
		}

		/// <summary>
		/// Initializes a new instance of the PdfLineElement class with the specified column edge.
		/// </summary>
		/// <param name="columnEdge">The column edge that defines the alignment or position of the line element within the PDF layout.</param>
		public PdfLineElement(PdfColumnEdge columnEdge)
		{
			this.ColumnEdge = columnEdge;
			this.LineDirection = PdfLineDirection.Horizontal;
		}

		/// <summary>
		/// Gets or sets the direction in which lines are drawn in the PDF document.
		/// </summary>
		public PdfLineDirection LineDirection { get; set; }

		/// <summary>
		/// Gets or sets the edge of the row to which this property applies.
		/// </summary>
		public PdfRowEdge RowEdge { get; set; }

		/// <summary>
		/// Gets or sets the edge of the column used for alignment or positioning operations.
		/// </summary>
		public PdfColumnEdge ColumnEdge { get; set; }

		/// <summary>
		/// Calculates the size required to render the text, including font, margin, padding, and cell padding, for the
		/// specified model and style on the given PDF grid page.
		/// </summary>
		/// <remarks>The returned size accounts for the text measurement as well as all resolved margin, padding, and
		/// cell padding values. Use this method to determine layout requirements before rendering content.</remarks>
		/// <param name="g">The PDF grid page used to measure the text and resolve style properties.</param>
		/// <param name="m">The model instance providing context for style resolution and text content.</param>
		/// <param name="style">The style applied to the model, including font, margin, padding, and cell padding settings.</param>
		/// <returns>A PdfSize representing the total width and height needed to render the text with all style adjustments applied.</returns>
		public virtual Task<PdfSize> MeasureAsync(PdfGridPage g, TModel m, PdfStyle<TModel> style)
		{
			PdfSize returnValue = new();
			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Renders the grid cell content within the specified bounds using the provided style and model data.
		/// </summary>
		/// <remarks>The rendering process applies margin, background, border, padding, and cell padding before
		/// drawing the text. The method does not draw content in the margin area. The style parameters are resolved using the
		/// provided model and state, allowing for dynamic customization.</remarks>
		/// <param name="g">The PDF grid page on which the cell content will be rendered.</param>
		/// <param name="m">The model data used to resolve style and content for rendering.</param>
		/// <param name="bounds">The bounds that define the area in which the cell content will be rendered.</param>
		/// <param name="style">The style settings that determine appearance aspects such as margin, padding, font, colors, and alignment.</param>
		/// <param name="state">An optional state object that can be used to resolve dynamic style or content values. May be null.</param>
		public virtual Task RenderAsync(PdfGridPage g, TModel m, PdfBounds bounds, PdfStyle<TModel> style, object state = null)
		{
			//
			// Draw the background.
			//
			XColor backgroundColor = style.BackgroundColor.Resolve(g, m, state);
			g.DrawFilledRectangle(bounds, backgroundColor);

			//
			// get the line weight and color.
			//
			double lineWeight = style.BorderWidth.Resolve(g, m, state);
			XColor lineColor = style.BorderColor.Resolve(g, m, state);

			if (this.LineDirection == PdfLineDirection.Horizontal)
			{
				PdfVerticalAlignment alignment = style.VerticalLineAlignment.Resolve(g, m, state);

				//
				// Draw the horizontal line at the specified row edge.
				//
				g.DrawHorizontalLine(alignment == PdfVerticalAlignment.Top ? bounds.TopRow : bounds.BottomRow,
									 bounds.LeftColumn,
									 bounds.RightColumn,
									 this.RowEdge,
									 lineWeight,
									 lineColor);
			}
			else
			{
				PdfHorizontalAlignment alignment = style.HorizontalImageAlignment.Resolve(g, m, state);

				//
				// Draw the vertical line at the specified column edge.
				//
				g.DrawVerticalLine(alignment == PdfHorizontalAlignment.Left ? bounds.LeftColumn : bounds.RightColumn,
									bounds.TopRow,
									bounds.BottomRow,
									this.ColumnEdge,
									lineWeight,
									lineColor);
			}

			return Task.CompletedTask;
		}
	}
}
