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
	/// Represents a template section for rendering header content within a PDF document, using a specified model type.
	/// </summary>
	/// <remarks>This class is intended for use in PDF generation scenarios where a header section needs to be
	/// rendered with customizable content and layout. It manages the layout and rendering of header text and background,
	/// and positions child sections directly below the header. The header appearance is determined by resolved styles and
	/// model data. Thread safety is not guaranteed; instances should not be shared across threads without external
	/// synchronization.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for the header content. Must implement the IPdfModel interface.</typeparam>
	public class PdfHeaderContentSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Arranges the child elements of the grid section asynchronously, positioning them relative to the header within the
		/// specified bounds.
		/// </summary>
		/// <remarks>This method positions child elements directly below the header section, adjusting their bounds
		/// based on the header's size. Only the first child is affected if multiple children exist.</remarks>
		/// <param name="g">The PDF grid page on which the layout operation is performed.</param>
		/// <param name="m">The model containing data used for layout calculations.</param>
		/// <param name="bounds">The bounds within which the child elements should be arranged.</param>
		/// <returns>A task that represents the asynchronous layout operation. The result is <see langword="true"/> if the layout was
		/// applied; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnLayoutChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			if (this.Children.Any())
			{
				//
				// Get the header rectangle.
				//
				PdfBounds headerRect = this.GetHeaderRect(g, m, bounds);

				//
				// Set the bound of the child section to be just
				// below the header section.
				//
				this.Children.Single().ActualBounds.LeftColumn = headerRect.LeftColumn;
				this.Children.Single().SetActualColumns(headerRect.Columns);
				this.Children.Single().ActualBounds.TopRow = headerRect.BottomRow + 1;
				this.Children.Single().SetActualRows(bounds.Rows - headerRect.Rows);

				//
				// Apply the layout.
				//
				this.Children.Single().LayoutAsync(g, m);
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Determines whether the background should be drawn for the current control.
		/// </summary>
		/// <remarks>Override this method to customize background rendering behavior for derived controls.</remarks>
		/// <returns>Always returns <see langword="false"/>, indicating that the background will not be drawn.</returns>
		protected override bool OnShouldDrawBackground()
		{
			return false;
		}

		/// <summary>
		/// Renders the header content asynchronously onto the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <param name="g">The PDF grid page on which the header will be rendered.</param>
		/// <param name="m">The model containing data used for rendering the header content.</param>
		/// <param name="bounds">The bounds within which the header should be rendered on the page.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the header was
		/// rendered successfully.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the styles.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the header rectangle.
			//
			PdfBounds headerRect = this.GetHeaderRect(g, m, bounds);

			//
			// Draw the filled rectangle.
			//
			g.DrawFilledRectangle(headerRect, style.BackgroundColor.Resolve(g, m));

			//
			// Draw the text.
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);

			g.DrawText(this.Text.Resolve(g, m).ToUpper(),
						style.Font.Resolve(g, m),
						headerRect.LeftColumn + padding.Left,
						headerRect.TopRow + padding.Top,
						headerRect.Columns - (padding.Left + padding.Right),
						headerRect.Rows - (padding.Top + padding.Bottom),
						style.TextAlignment.Resolve(g, m), 
						style.ForegroundColor.Resolve(g, m));

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Calculates the size required to render the header text, including padding, for the specified page and model.
		/// </summary>
		/// <remarks>Override this method to customize header sizing logic for different grid layouts or model
		/// types.</remarks>
		/// <param name="g">The page context used to measure the header text and resolve styles.</param>
		/// <param name="m">The model instance used to resolve the header text and style information.</param>
		/// <returns>A PdfSize structure representing the total size needed to display the header, including text and padding.</returns>
		protected virtual PdfSize GetHeaderSize(PdfGridPage g, TModel m)
		{
			//
			// Get the styles.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the text.
			//
			string text = this.Text.Resolve(g, m).ToUpper();

			//
			// Get the size of the text.
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);
			PdfSize size = g.MeasureText(style.Font.Resolve(g, m), text);
			size.Rows += padding.Top + padding.Bottom;
			size.Columns += padding.Left + padding.Right;

			return size;
		}

		/// <summary>
		/// Calculates the bounding rectangle for the header area of a grid page based on the specified model and layout
		/// bounds.
		/// </summary>
		/// <param name="g">The grid page for which the header rectangle is being calculated.</param>
		/// <param name="m">The data model used to determine header layout and content.</param>
		/// <param name="bounds">The layout bounds representing the area available for the grid on the page.</param>
		/// <returns>A PdfBounds object representing the position and size of the header area within the specified bounds.</returns>
		protected virtual PdfBounds GetHeaderRect(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			PdfSize size = this.GetHeaderSize(g, m);
			return (new PdfBounds(bounds.LeftColumn, bounds.TopRow, bounds.Columns, size.Rows));
		}
	}
}
