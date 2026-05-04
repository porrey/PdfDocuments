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
	/// Provides extension methods for debugging and visualizing layout information on PdfGridPage instances during PDF
	/// generation.
	/// </summary>
	/// <remarks>These methods are intended to assist with development and troubleshooting by rendering visual cues,
	/// such as colored highlights and labels, onto PDF grid pages. They are typically used in debug builds or diagnostic
	/// scenarios to inspect layout boundaries, section sizes, and font usage. The extensions do not affect the final
	/// output in production and are safe to use for non-destructive visual feedback.</remarks>
	public static class PdfGridPageDebugExtensions
	{
		/// <summary>
		/// Asynchronously generates a random color and determines an appropriate contrasting label color for use in PDF grid
		/// debugging scenarios.
		/// </summary>
		/// <remarks>The label color is selected to provide sufficient contrast against the generated color, ensuring
		/// readability when used as a label or overlay.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the grid page. Must implement the IPdfModel interface.</typeparam>
		/// <param name="source">The PdfGridPage instance for which to generate debug colors.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the generated random
		/// color and its corresponding contrasting label color.</returns>
		public static Task<(XColor, XColor)> GetDebugColorsAsync<TModel>(this PdfGridPage source)
			where TModel : IPdfModel
		{
			//
			// Create a random color
			//
			XColor color = XColorExtensions.RandomColor();

			//
			// Set the label color.
			//
			XColor labelColor = color.Contrast(XColors.White) > color.Contrast(XColors.Black) ? XColors.White : XColors.Black;

			return Task.FromResult((color, labelColor));
		}

		/// <summary>
		/// Highlights the margin area of the specified PDF section by drawing a colored border and a semi-transparent,
		/// diagonally-lined background within the margin.
		/// </summary>
		/// <remarks>The highlight is applied only if the section's render area does not cover the entire section
		/// area. The background fill uses a lighter, semi-transparent version of the specified color and includes diagonal
		/// lines to visually distinguish the margin. This method does not block and completes synchronously.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="source">The PdfGridPage on which to draw the highlighted margin background.</param>
		/// <param name="section">The section whose margin area will be highlighted. The section must provide render and section area information.</param>
		/// <param name="color">The base color used for the border and background highlight.</param>
		/// <param name="outlineWidth">The width, in points, of the border outline. The default is 1.5.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		public static Task HighlightMarginBackgroundAsync<TModel>(this PdfGridPage source, IPdfSection<TModel> section, XColor color, double outlineWidth = 1.5)
			where TModel : IPdfModel
		{
			//
			// Draw a border around the renderable area and fill the it
			// with a lighter version of the color using alpha so the
			// fill does not cover up the rendered section. The background
			// will have diagonal lines through it. This fill will
			// highlight the margin area only.
			//
			if (section.RenderArea != section.SectionArea)
			{
				XColor backgroundColor = color.WithLuminosity(.65).WithAlpha(.15);
				XColor lineColor = color.WithLuminosity(.55).WithAlpha(.15);
				source.DrawFilledRectangle(section.SectionArea, section.RenderArea, backgroundColor, lineColor);
			}

			source.DrawRectangle(section.RenderArea, outlineWidth, color);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Draws a dashed rectangular highlight around the specified section area on the PDF page asynchronously.
		/// </summary>
		/// <remarks>The highlight is rendered as a dashed border around the section's area. This method does not
		/// modify the section content, only its visual outline.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="source">The PDF page on which to draw the highlighted section area.</param>
		/// <param name="section">The section whose area will be highlighted. Must not be null.</param>
		/// <param name="color">The color to use for the highlight border.</param>
		/// <param name="outlineWidth">The width, in points, of the highlight border. The default is 1.0.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		public static Task HighlightSectionAreaAsync<TModel>(this PdfGridPage source, IPdfSection<TModel> section, XColor color, double outlineWidth = 1.0)
			where TModel : IPdfModel
		{
			//
			// Get a pen for the border around the section.
			//
			XPen pen = new(color, outlineWidth)
			{
				DashStyle = XDashStyle.Dash
			};

			//
			// Draw a border around the section area.
			//
			source.DrawRectangle(section.SectionArea, pen);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Draws a debug label displaying the section key and render area size in the upper-left corner of the specified
		/// section on the PDF grid page.
		/// </summary>
		/// <remarks>This method is intended for debugging and visualization purposes. The label includes the section
		/// key and the coordinates and dimensions of the render area. The operation completes synchronously.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the section.</typeparam>
		/// <param name="source">The PDF grid page on which to draw the size label.</param>
		/// <param name="model">The model instance providing context for rendering and style resolution.</param>
		/// <param name="section">The PDF section whose key and render area size will be displayed in the label.</param>
		/// <param name="color">The background color to use for the label rectangle.</param>
		/// <param name="labelColor">The color to use for the label text.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		public static Task DrawSizeLabelAsync<TModel>(this PdfGridPage source, TModel model, IPdfSection<TModel> section, XColor color, XColor labelColor)
			where TModel : IPdfModel
		{
			//
			// Get the size of the text.
			//
			PdfStyle<TModel> style = section.StyleManager.GetStyle(PdfStyleManager<TModel>.Debug);

			//
			// Get the font for the section.
			//
			XFont font = style.Font.Resolve(source, model);

			//
			// Create a label with the section key and the size of the render area.
			//
			string label = $"{section.Key} [x{section.RenderArea.LeftColumn}, y{section.RenderArea.TopRow}, w{section.RenderArea.Columns}, h{section.RenderArea.Rows}]";

			//
			// Measure the size of the text to be drawn so we can size the background rectangle.
			//
			PdfSize textSize = source.MeasureText(font, label);

			//
			// Create padding for the box and text.
			//
			PdfSpacing padding = (1, 1, 1, 1);

			//
			// Calculate the bounds for the label by applying the padding to the text size and
			// placing it in the upper left corner of the section area.
			//
			PdfBounds labelBounds = new(section.RenderArea.LeftColumn, section.RenderArea.TopRow, textSize.Columns + padding.Left + padding.Right, textSize.Rows + padding.Top + padding.Bottom);

			//
			// Draw a small filled rectangle behind the text.
			//
			source.DrawFilledRectangle(labelBounds, color);

			//
			// Draw the text label.
			//			
			source.DrawText(label, font, labelBounds, XStringFormats.Center, labelColor);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Draws a label displaying the font family name in the upper right corner of the specified section on the PDF page,
		/// using the provided font and outline colors.
		/// </summary>
		/// <remarks>This method is typically used for debugging or visualization purposes to indicate the font family
		/// applied to a section. The label is rendered with a background for visibility and is centered within its designated
		/// area.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="source">The PDF grid page on which to draw the font label.</param>
		/// <param name="model">The model instance providing data for the section.</param>
		/// <param name="section">The PDF section for which the font label is drawn.</param>
		/// <param name="fontColor">The color to use for the font label text.</param>
		/// <param name="outlineColor">The color to use for the label's background rectangle.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		public static Task DrawFontLabelAsync<TModel>(this PdfGridPage source, TModel model, IPdfSection<TModel> section, XColor fontColor, XColor outlineColor)
			where TModel : IPdfModel
		{
			//
			// Get the style for the section.
			//
			PdfStyle<TModel> style = section.ResolveStyle(0);

			//
			// Get the debug style for the section.
			//
			PdfStyle<TModel> debugStyle = section.StyleManager.GetStyle(PdfStyleManager<TModel>.Debug);
			XFont debugFont = debugStyle.Font.Resolve(source, model);

			//
			// Get the padding and cell padding for the section.
			//
			PdfSpacing padding = style.Padding.Resolve(source, model);
			PdfSpacing cellPadding = style.CellPadding.Resolve(source, model);

			//
			// Calculate the text bounds by applying the padding and cell padding to the render area.
			//
			PdfBounds textBounds = section.RenderArea
									.SubtractSpacing(source, model, padding)
									.SubtractSpacing(source, model, cellPadding);

			//
			// Get the font for the section.
			//
			XFont font = style.Font.Resolve(source, model);

			//
			// Measure the size of the text to be drawn so we can center it within the text area.
			//
			PdfSize textSize = source.MeasureText(debugFont, font.FontFamily.Name);

			//
			// Create padding for the box and text.
			//
			PdfSpacing debugPadding = (1, 1, 1, 1);

			//
			// Place the label in the upper right corner.
			//
			PdfBounds labelBounds = new(textBounds.RightColumn - textSize.Columns - debugPadding.Right, textBounds.TopRow, textSize.Columns + debugPadding.Left + debugPadding.Right, textSize.Rows + debugPadding.Top + debugPadding.Bottom);

			//
			// Draw a filled rectangle behind the label text to make it more visible.
			//
			source.DrawFilledRectangle(labelBounds, outlineColor);

			//
			// Draw the label text centered within the text area.
			//
			XRect labelLayout = source.GetRect(labelBounds);
			XBrush labelBrush = new XSolidBrush(fontColor);
			source.Graphics.DrawString(font.FontFamily.Name, debugFont, labelBrush, labelLayout, XStringFormats.Center);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Draws a dotted outline around the text area of the specified PDF section on the page.
		/// </summary>
		/// <remarks>The outline is drawn using a dotted line style and reflects the section's resolved padding and
		/// cell padding. This method does not modify the content of the section; it only draws a visual border for
		/// highlighting purposes.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the section.</typeparam>
		/// <param name="source">The PDF grid page on which to draw the outline.</param>
		/// <param name="model">The model instance providing data for the section.</param>
		/// <param name="section">The PDF section whose text area will be outlined.</param>
		/// <param name="outlineColor">The color of the outline to draw around the text area.</param>
		/// <param name="outlineWidth">The width, in points, of the outline. The default is 0.3.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task HighlightFontAreaAsync<TModel>(this PdfGridPage source, TModel model, IPdfSection<TModel> section, XColor outlineColor, double outlineWidth = .3)
			where TModel : IPdfModel
		{
			//
			// Get the style for the section.
			//
			PdfStyle<TModel> style = section.ResolveStyle(0);

			//
			// Get the padding and cell padding for the section.
			//
			PdfSpacing padding = style.Padding.Resolve(source, model);
			PdfSpacing cellPadding = style.CellPadding.Resolve(source, model);

			//
			// Calculate the text bounds by applying the padding and cell padding to the render area.
			//
			PdfBounds textBounds = section.RenderArea
									.SubtractSpacing(source, model, padding)
									.SubtractSpacing(source, model, cellPadding);

			//
			// Get a pen for the border around the text area.
			//
			XPen pen = new(outlineColor.WithAlpha(.65), outlineWidth)
			{
				DashStyle = XDashStyle.Dot
			};

			//
			// Draw a border around the text area.
			//
			source.DrawRectangle(textBounds, pen);

			return Task.CompletedTask;
		}
	}
}
