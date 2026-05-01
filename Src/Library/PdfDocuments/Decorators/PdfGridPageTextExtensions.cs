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
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for measuring and drawing text within a grid-based PDF page, including support for font
	/// selection, text formatting, color, and debug overlays.
	/// </summary>
	/// <remarks>These extension methods simplify text rendering and measurement on PDF pages that use a grid
	/// layout. They offer overloads for specifying fonts, bounds, and formatting options, and include features for
	/// debugging such as font detail overlays and outlines. Methods are designed to work with the grid structure, allowing
	/// precise placement and sizing of text sections.</remarks>
	public static class PdfGridPageTextExtensions
	{
		/// <summary>
		/// Measures the size of the specified text when rendered on the given PDF grid page using the provided font family,
		/// size, and style.
		/// </summary>
		/// <param name="source">The PDF grid page on which the text measurement is performed.</param>
		/// <param name="familyName">The name of the font family to use for measuring the text.</param>
		/// <param name="emSize">The font size, in points, used for the measurement. Must be a positive value.</param>
		/// <param name="style">The font style to apply, such as bold or italic, when measuring the text.</param>
		/// <param name="text">The text to measure. If not specified, the default value is "Test".</param>
		/// <returns>A PdfSize structure representing the width and height of the measured text in PDF units.</returns>
		public static PdfSize MeasureText(this PdfGridPage source, string familyName, double emSize, XFontStyleEx style, string text = "Test")
		{
			PdfSize returnValue = new();

			XFont font = new(familyName, emSize, style);
			returnValue = source.MeasureText(font, text);

			return returnValue;
		}

		/// <summary>
		/// Calculates the number of grid rows and columns required to display the specified text using the given font on the
		/// provided PDF grid page.
		/// </summary>
		/// <remarks>The calculation is based on the current row height and column width of the grid. This method is
		/// useful for determining layout requirements before rendering text.</remarks>
		/// <param name="source">The PDF grid page on which the text will be measured. Must not be null.</param>
		/// <param name="font">The font used to render the text. Determines the size and style of the measured text. Must not be null.</param>
		/// <param name="text">The text to measure. If null, an empty string is used. Defaults to "Test".</param>
		/// <returns>A PdfSize object containing the number of rows and columns needed to fit the text within the grid, based on the
		/// font and page settings.</returns>
		public static PdfSize MeasureText(this PdfGridPage source, XFont font, string text = "Test")
		{
			PdfSize returnValue = new();

			XSize pixelSize = source.Graphics.MeasureString(text ?? string.Empty, font);

			returnValue.Rows = (int)(pixelSize.Height / source.Grid.RowHeight);
			returnValue.Columns = (int)(pixelSize.Width / source.Grid.ColumnWidth);

			return returnValue;
		}

		/// <summary>
		/// Draws the specified text within the given bounds on the PDF page using the provided font, formatting, and color.
		/// Optionally disables debug overlays such as font details and outline.
		/// </summary>
		/// <remarks>When debug overlays are enabled and not suppressed, the method may draw additional visual
		/// sections such as the font name and an outline around the text area. These overlays are useful for debugging layout
		/// and font selection.</remarks>
		/// <param name="source">The PDF page on which the text will be drawn.</param>
		/// <param name="text">The text to render. If null, an empty string is drawn.</param>
		/// <param name="font">The font used to render the text.</param>
		/// <param name="bounds">The area within which the text will be drawn, specified as PDF bounds.</param>
		/// <param name="formats">The formatting options that control text alignment and layout.</param>
		/// <param name="color">The color used to render the text.</param>
		/// <param name="forceNoDebug">If set to <see langword="true"/>, disables debug overlays such as font details and outline regardless of the
		/// page's debug mode.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		/// <returns>A <see cref="PdfSize"/> representing the measured size of the rendered text.</returns>
		public static PdfSize DrawText(this PdfGridPage source, string text, XFont font, PdfBounds bounds, XStringFormat formats, XColor color, bool clipDrawing = false, bool forceNoDebug = false)
		{
			PdfSize returnValue = source.MeasureText(font, text);

			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				if (clipDrawing)
				{
					source.ClipDrawing(bounds);
				}

				//
				// Draw the text.
				//
				source.Graphics.DrawString(text ?? string.Empty, font, new XSolidBrush(color), source.GetRect(bounds), formats);

				//
				// Draw the name of the font over the text.
				//
				if (!forceNoDebug && source.DebugMode.HasFlag(DebugMode.RevealFontDetails))
				{
					XFont debugFont = new(GlobalPdfDocumentsSettings.DefaultFontName, 8, XFontStyleEx.Regular);
					PdfSize textSize = source.MeasureText(debugFont, font.FontFamily.Name);
					PdfBounds labelBounds = new(bounds.LeftColumn + (int)((bounds.Columns - textSize.Columns) / 2.0), bounds.TopRow + (int)((bounds.Rows - textSize.Rows) / 2.0), textSize.Columns + 2, textSize.Rows + 2);
					source.DrawFilledRectangle(labelBounds, XColors.Black);
					XRect labelLayout = source.GetRect(labelBounds);
					XBrush labelBrush = new XSolidBrush(XColors.Wheat);
					source.Graphics.DrawString(font.FontFamily.Name, debugFont, labelBrush, labelLayout, XStringFormats.Center);
				}

				//
				// Draw a dotted line around the area used to draw the text.
				//
				if (!forceNoDebug && source.DebugMode.HasFlag(DebugMode.OutlineText))
				{
					XPen pen = new(XColors.HotPink, .5)
					{
						DashStyle = XDashStyle.Dot
					};

					source.DrawRectangle(bounds, pen);
				}
			}
			finally
			{
				source.Graphics.Restore(state);
			}

			return returnValue;
		}

		/// <summary>
		/// Draws the specified text within the given bounds on the PDF page using the provided font family, size, style,
		/// formatting, and color.
		/// </summary>
		/// <param name="source">The PDF page on which the text will be drawn.</param>
		/// <param name="text">The text string to render on the page.</param>
		/// <param name="familyName">The name of the font family to use for rendering the text.</param>
		/// <param name="emSize">The font size, in points, used to render the text. Must be greater than zero.</param>
		/// <param name="style">The font style to apply, such as bold or italic.</param>
		/// <param name="bounds">The rectangular area, in PDF coordinates, within which the text will be drawn.</param>
		/// <param name="formats">The string formatting options that determine text alignment and layout within the bounds.</param>
		/// <param name="color">The color used to render the text.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		/// <returns>A PdfSize structure representing the size of the rendered text within the specified bounds.</returns>
		public static PdfSize DrawText(this PdfGridPage source, string text, string familyName, double emSize, XFontStyleEx style, PdfBounds bounds, XStringFormat formats, XColor color, bool clipDrawing = false)
		{
			XFont font = new(familyName, emSize, style);
			return source.DrawText(text, font, bounds, formats, color, clipDrawing);
		}

		/// <summary>
		/// Draws the specified text within the defined grid area on the PDF page using the given font and formatting options.
		/// </summary>
		/// <param name="source">The PDF grid page on which to draw the text.</param>
		/// <param name="text">The text to be drawn.</param>
		/// <param name="familyName">The name of the font family to use for rendering the text.</param>
		/// <param name="emSize">The font size, in points, to use for the text.</param>
		/// <param name="style">The font style to apply, such as bold or italic.</param>
		/// <param name="leftColumn">The zero-based index of the leftmost column of the grid area where the text will be drawn.</param>
		/// <param name="topRow">The zero-based index of the topmost row of the grid area where the text will be drawn.</param>
		/// <param name="columns">The number of columns spanning the area in which to draw the text. Must be greater than zero.</param>
		/// <param name="rows">The number of rows spanning the area in which to draw the text. Must be greater than zero.</param>
		/// <param name="formats">The string formatting options to apply when drawing the text.</param>
		/// <param name="color">The color to use for the text.</param>
		/// <param name="clipDrawing">true to clip the text to the bounds of the specified grid area; otherwise, false.</param>
		/// <returns>A PdfSize structure representing the size of the area occupied by the drawn text.</returns>
		public static PdfSize DrawText(this PdfGridPage source, string text, string familyName, double emSize, XFontStyleEx style, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, bool clipDrawing)
		{
			PdfBounds bounds = new(leftColumn, topRow, columns, rows);
			return source.DrawText(text, familyName, emSize, style, bounds, formats, color, clipDrawing);
		}

		/// <summary>
		/// Draws the specified text within a grid cell area on the PDF page using the provided font, formatting, and color.
		/// </summary>
		/// <param name="source">The PDF grid page on which the text will be drawn.</param>
		/// <param name="text">The text to render within the specified grid cell area.</param>
		/// <param name="font">The font to use when drawing the text.</param>
		/// <param name="leftColumn">The zero-based index of the leftmost column of the grid area where the text will be drawn.</param>
		/// <param name="topRow">The zero-based index of the topmost row of the grid area where the text will be drawn.</param>
		/// <param name="columns">The number of columns spanned by the grid area.</param>
		/// <param name="rows">The number of rows spanned by the grid area.</param>
		/// <param name="formats">The string formatting options to apply when rendering the text.</param>
		/// <param name="color">The color to use for the text.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		/// <returns>A PdfSize object representing the size of the drawn text within the specified grid area.</returns>
		public static PdfSize DrawText(this PdfGridPage source, string text, XFont font, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, bool clipDrawing = false)
		{
			PdfBounds bounds = new(leftColumn, topRow, columns, rows);
			return source.DrawText(text, font, bounds, formats, color, clipDrawing);
		}

		/// <summary>
		/// Draws the specified text within the given grid bounds on the PDF page, automatically wrapping lines as needed.
		/// </summary>
		/// <remarks>Text will be wrapped automatically to fit within the specified grid area. The method respects
		/// paragraph alignment and formatting options. Use this method to render multi-line or wrapped text in grid-based PDF
		/// layouts.</remarks>
		/// <param name="source">The PDF grid page on which the text will be drawn.</param>
		/// <param name="text">The text to render within the specified bounds. May contain line breaks or long lines that require wrapping.</param>
		/// <param name="font">The font to use when drawing the text.</param>
		/// <param name="bounds">The grid bounds defining the area where the text will be rendered. Specifies columns and rows for layout.</param>
		/// <param name="formats">The string formatting options to apply when rendering the text, such as alignment and trimming.</param>
		/// <param name="color">The color used to draw the text.</param>
		/// <param name="paragraphAlignment">The paragraph alignment to apply to the text. Defaults to <see cref="XParagraphAlignment.Default"/> if not
		/// specified.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		public static void DrawWrappingText(this PdfGridPage source, string text, XFont font, PdfBounds bounds, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default, bool clipDrawing = false)
		{
			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				XRect layout = new(source.Grid.Left(bounds.LeftColumn), source.Grid.Top(bounds.TopRow), source.Grid.ColumnsWidth(bounds.Columns), source.Grid.RowsHeight(bounds.Rows));

				//
				// Create a text formatter with the specified paragraph alignment.
				//
				XTextFormatter formatter = new(source.Graphics)
				{
					Alignment = paragraphAlignment
				};

				//
				// Create a brush with the specified color for drawing the text.
				//
				XBrush brush = new XSolidBrush(color);

				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				if (clipDrawing)
				{
					source.ClipDrawing(bounds);
				}

				//
				// Draw the text using the formatter, which will handle wrapping and alignment within the
				// specified layout rectangle.
				//
				formatter.DrawString(text, font, brush, layout, formats);
			}
			finally
			{
				source.Graphics.Restore(state);
			}
		}

		/// <summary>
		/// Draws the specified text onto the PDF grid page, wrapping it within the defined grid bounds and using the provided
		/// font settings and formatting options.
		/// </summary>
		/// <remarks>Text will automatically wrap within the specified grid area. Use appropriate font and formatting
		/// settings to ensure correct appearance. The method does not return a value and modifies the PDF grid page
		/// directly.</remarks>
		/// <param name="source">The PDF grid page on which the text will be drawn.</param>
		/// <param name="text">The text to be rendered and wrapped within the grid area.</param>
		/// <param name="familyName">The name of the font family to use for rendering the text.</param>
		/// <param name="emSize">The font size, in points, used to display the text.</param>
		/// <param name="style">The font style to apply, such as bold or italic.</param>
		/// <param name="leftColumn">The index of the leftmost column in the grid area where the text will be drawn.</param>
		/// <param name="topRow">The index of the topmost row in the grid area where the text will be drawn.</param>
		/// <param name="columns">The number of columns spanning the grid area for the text.</param>
		/// <param name="rows">The number of rows spanning the grid area for the text.</param>
		/// <param name="formats">The string formatting options that control text layout and alignment.</param>
		/// <param name="color">The color used to render the text.</param>
		/// <param name="paragraphAlignment">The paragraph alignment applied to the text. Defaults to <see cref="XParagraphAlignment.Default"/> if not
		/// specified.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		public static void DrawWrappingText(this PdfGridPage source, string text, string familyName, double emSize, XFontStyleEx style, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default, bool clipDrawing = false)
		{
			PdfBounds bounds = new(leftColumn, topRow, columns, rows);
			XFont font = new(familyName, emSize, style);
			source.DrawWrappingText(text, font, bounds, formats, color, paragraphAlignment, clipDrawing);
		}

		/// <summary>
		/// Draws the specified text within a rectangular grid area on the PDF page, automatically wrapping the text across
		/// multiple lines as needed.
		/// </summary>
		/// <remarks>If clipDrawing is set to true, text that exceeds the bounds of the specified grid area will be
		/// clipped. The method preserves and restores the graphics state of the page during drawing.</remarks>
		/// <param name="source">The PDF grid page on which to draw the text.</param>
		/// <param name="text">The text to be drawn and wrapped within the specified grid area.</param>
		/// <param name="font">The font to use when rendering the text.</param>
		/// <param name="leftColumn">The zero-based index of the leftmost column of the grid area where the text will be drawn.</param>
		/// <param name="topRow">The zero-based index of the topmost row of the grid area where the text will be drawn.</param>
		/// <param name="columns">The number of columns spanning the grid area for the text.</param>
		/// <param name="rows">The number of rows spanning the grid area for the text.</param>
		/// <param name="formats">The string formatting options to apply when drawing the text.</param>
		/// <param name="color">The color to use for the rendered text.</param>
		/// <param name="paragraphAlignment">The paragraph alignment to apply to the text. The default is XParagraphAlignment.Default.</param>
		/// <param name="clipDrawing">true to restrict drawing to the specified grid area; otherwise, false.</param>
		public static void DrawWrappingText(this PdfGridPage source, string text, XFont font, int leftColumn, int topRow, int columns, int rows, XStringFormat formats, XColor color, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Default, bool clipDrawing = false)
		{
			//
			// Save the current graphics state to restore it later.
			//
			XGraphicsState state = source.Graphics.Save();

			try
			{
				//
				// Calculate the bounds for the text based on the specified grid area.
				//
				PdfBounds bounds = new(leftColumn, topRow, columns, rows);

				//
				// If clipping is enabled, set a clipping region to restrict drawing to the specified bounds.
				//
				if (clipDrawing)
				{
					source.ClipDrawing(bounds);
				}

				source.DrawWrappingText(text, font, bounds, formats, color, paragraphAlignment);
			}
			finally
			{
				source.Graphics.Restore(state);
			}
		}

		/// <summary>
		/// Sets a clipping region on the PDF page so that subsequent drawing operations are restricted to the specified
		/// bounds.
		/// </summary>
		/// <remarks>This method modifies the graphics state of the page. Drawing outside the specified bounds will
		/// not appear until the clipping region is reset or changed.</remarks>
		/// <param name="source">The PDF grid page on which to set the clipping region.</param>
		/// <param name="bounds">The bounds that define the rectangular area to which drawing will be clipped.</param>
		public static void ClipDrawing(this PdfGridPage source, PdfBounds bounds)
		{
			//
			// Get the rectangle area corresponding to the grid bounds for drawing the text.
			//
			XRect rect = source.GetRect(bounds);

			//
			// Create a clipping path.
			//
			XGraphicsPath clipPath = new();
			clipPath.AddRectangle(rect);

			//
			// Set the clipping region to the combined path, ensuring that drawing operations only
			// affect the area between the outer and inner rectangles.
			//
			source.Graphics.IntersectClip(clipPath);
		}
	}
}
