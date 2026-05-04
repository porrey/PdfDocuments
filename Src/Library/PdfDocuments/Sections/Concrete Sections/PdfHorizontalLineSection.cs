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
	/// Represents a PDF section that renders a horizontal line, with support for model-based customization and binding to
	/// a specific row edge in a PDF table.
	/// </summary>
	/// <remarks>Use this section to insert a horizontal line into a PDF document, typically as a visual separator
	/// within a table or layout. The section allows customization of the line's position by binding to a specific row
	/// edge, and supports asynchronous rendering and initialization. Inherit from this class to further customize
	/// rendering behavior or integrate with additional PDF generation features.</remarks>
	/// <typeparam name="TModel">The type of the model associated with the section. Must implement the IPdfModel interface.</typeparam>
	public class PdfHorizontalLineSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the binding for the row edge of the PDF table associated with the model.
		/// </summary>
		/// <remarks>Use this property to specify or retrieve how the row edge is determined for each model instance
		/// when generating a PDF table. The binding defines which edge of the row (such as top or bottom) is targeted during
		/// rendering or processing.</remarks>
		public virtual BindProperty<PdfRowEdge, TModel> RowEdge { get; set; } = PdfRowEdge.Top;

		/// <summary>
		/// Performs asynchronous initialization logic for the grid page using the specified model and bounds.
		/// </summary>
		/// <param name="g">The grid page to initialize.</param>
		/// <param name="m">The model containing data or state used for initialization.</param>
		/// <param name="bounds">The bounds within which the grid page should be initialized.</param>
		/// <returns>A task that represents the asynchronous initialization operation.</returns>
		protected override Task OnInitializeAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (this.Children.Count != 0)
			{
				this.Children.Clear();
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Asynchronously renders content for the specified model within the given bounds on the provided PDF grid page.
		/// </summary>
		/// <param name="g">The PDF grid page on which to render the content.</param>
		/// <param name="m">The model containing the data to be rendered.</param>
		/// <param name="bounds">The bounds within which the content should be rendered on the page.</param>
		/// <returns>A task that represents the asynchronous render operation.</returns>
		protected override async Task OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Check if this section should be rendered or not.
			//
			if (this.ShouldRender.Resolve(g, m))
			{
				//
				// Perform any necessary initialization before rendering the section.
				//
				await this.OnInitializeAsync(g, m, bounds);

				//
				// Resolve the style for the line.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

				//
				// Apply padding.
				//
				PdfSpacing padding = style.Padding.Resolve(g, m);
				PdfBounds paddedBounds = this.ApplyPadding(g, m, padding);

				//
				// Render the background with padding.
				//
				await this.OnRenderBackgroundAsync(g, m, paddedBounds);

				//
				// Resolve the row edge based on the provided model and the RowEdge binding.
				//
				PdfRowEdge rowEdge = this.RowEdge.Resolve(g, m);

				//
				// Create a line element based on the resolved row edge.
				//
				PdfLineElement<TModel> line = new(rowEdge);

				//
				// Render the line.
				//
				await line.RenderAsync(g, m, paddedBounds, style);

				//
				// Render the watermark on top of all other sections.
				//
				await this.OnRenderWaterMarkAsync(g, m, paddedBounds);
			}
		}
	}
}
