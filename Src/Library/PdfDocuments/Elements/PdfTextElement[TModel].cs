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
	/// Represents a text element within a PDF grid, allowing for measurement and rendering of styled text content.
	/// </summary>
	/// <remarks>Use <see cref="PdfTextElement{TModel}"/> to add styled text to a PDF grid. The element supports
	/// customizable margins, padding, cell padding, and style properties such as font, colors, and alignment. Rendering
	/// and measurement are performed relative to the provided model and grid page context. Thread safety is not
	/// guaranteed; synchronize access if used concurrently.</remarks>
	/// <typeparam name="TModel">The model type that provides context for rendering and measuring the PDF element. Must implement <see
	/// cref="IPdfModel"/>.</typeparam>
	public class PdfTextElement<TModel> : IPdfElement<TModel> where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfTextElement class with the specified text content.
		/// </summary>
		/// <param name="text">The text to be displayed in the PDF element. Cannot be null.</param>
		public PdfTextElement(string text)
		{
			this.Text = text;
		}

		/// <summary>
		/// Gets or sets the text content associated with this instance.
		/// </summary>
		public virtual string Text { get; set; }

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

			//
			// Resolve the font from the style, as it is needed to measure the text.
			//
			XFont font = style.Font.Resolve(g, m);

			//
			// Measure the text.
			//
			returnValue = g.MeasureText(font, this.Text);

			//
			// Add the cell padding
			//
			PdfSpacing cellPaddng = style.CellPadding.Resolve(g, m);
			returnValue.Columns += cellPaddng.Left + cellPaddng.Right;
			returnValue.Rows += cellPaddng.Top + cellPaddng.Bottom;

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
			// Draw the border.
			//
			XColor bordercolor = style.BorderColor.Resolve(g, m, state);
			double borderWidth = style.BorderWidth.Resolve(g, m, state);
			XPen pen = new(bordercolor, borderWidth);
			g.DrawRectangle(bounds, pen);

			//
			// Pad the text. This allows the border and fill to extend beyond the text.
			//
			PdfSpacing cellPadding = style.CellPadding.Resolve(g, m, state);
			PdfBounds textBounds = bounds.SubtractBounds(g, m, cellPadding);

			//
			// Draw the text.
			//
			XFont font = style.Font.Resolve(g, m, state);
			XStringFormat textAlignment = style.TextAlignment.Resolve(g, m, state);
			XColor foregroundColor = style.ForegroundColor.Resolve(g, m, state);
			g.DrawText(this.Text, font, textBounds, textAlignment, foregroundColor);

			return Task.CompletedTask;
		}
	}
}
