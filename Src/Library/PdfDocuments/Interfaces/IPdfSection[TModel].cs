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
	/// Defines the contract for a section within a PDF document, supporting layout, rendering, and style management for a
	/// model of type TModel.
	/// </summary>
	/// <remarks>Implementations of this interface provide mechanisms for rendering, layout, and styling of PDF
	/// sections. Sections can be nested, allowing for hierarchical document structures. The interface supports binding
	/// properties to model data, enabling dynamic content and conditional rendering. Thread safety is not guaranteed;
	/// access from multiple threads should be synchronized as needed.</remarks>
	/// <typeparam name="TModel">The type of model data associated with the PDF section. Must implement IPdfModel.</typeparam>
	public interface IPdfSection<TModel>
			where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the unique identifier associated with the current instance.
		/// </summary>
		string Key { get; set; }

		/// <summary>
		/// Gets or sets the style manager used to configure PDF rendering styles for the model.
		/// </summary>
		/// <remarks>Use this property to customize appearance settings such as fonts, colors, and layout when
		/// generating PDF documents. Assigning a different style manager allows for flexible styling across various
		/// models.</remarks>
		IPdfStyleManager<TModel> StyleManager { get; set; }

		/// <summary>
		/// Gets or sets the collection of style names applied to the element.
		/// </summary>
		IEnumerable<string> StyleNames { get; set; }

		/// <summary>
		/// Gets or sets the text value bound to the model.
		/// </summary>
		BindProperty<string, TModel> Text { get; set; }

		/// <summary>
		/// Gets or sets the actual bounds of the PDF element after layout calculations are applied.
		/// </summary>
		/// <remarks>The value reflects the final position and size of the element within the PDF document, which may
		/// differ from initial settings due to layout adjustments. Use this property to determine the rendered area for tasks
		/// such as hit testing or custom drawing.</remarks>
		PdfBounds ActualBounds { get; set; }

		/// <summary>
		/// Gets or sets the parent section of the current PDF section.
		/// </summary>
		IPdfSection<TModel> ParentSection { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the component should be rendered.
		/// </summary>
		BindProperty<bool, TModel> ShouldRender { get; set; }

		/// <summary>
		/// Gets the collection of child sections contained within this PDF section.
		/// </summary>
		/// <remarks>The returned list provides access to all immediate child sections. The collection is read-only;
		/// to modify the hierarchy, use the appropriate methods on the parent section.</remarks>
		IList<IPdfSection<TModel>> Children { get; }

		/// <summary>
		/// Gets or sets the file path to the image used as a watermark.
		/// </summary>
		BindProperty<string, TModel> WaterMarkImagePath { get; set; }

		/// <summary>
		/// Asynchronously renders the specified PDF grid page using the provided model.
		/// </summary>
		/// <param name="gridPage">The PDF grid page to render. Must not be null.</param>
		/// <param name="model">The data model used to populate the grid page. Must not be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if rendering
		/// succeeds; otherwise, <see langword="false"/>.</returns>
		Task<bool> RenderAsync(PdfGridPage gridPage, TModel model);

		/// <summary>
		/// Asynchronously arranges the specified grid page using the provided model data.
		/// </summary>
		/// <param name="gridPage">The PDF grid page to be laid out. Cannot be null.</param>
		/// <param name="model">The model containing data used to layout the grid page. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the layout was
		/// successful; otherwise, <see langword="false"/>.</returns>
		Task<bool> LayoutAsync(PdfGridPage gridPage, TModel model);

		/// <summary>
		/// Asynchronously renders debug information onto the specified PDF grid page using the provided model.
		/// </summary>
		/// <param name="gridPage">The PDF grid page on which debug information will be rendered. Cannot be null.</param>
		/// <param name="model">The model containing data used to generate debug output. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if debug information
		/// was successfully rendered; otherwise, <see langword="false"/>.</returns>
		Task<bool> RenderDebugAsync(PdfGridPage gridPage, TModel model);

		/// <summary>
		/// Sets the actual number of rows processed or affected by the operation asynchronously.
		/// </summary>
		/// <param name="rows">The number of rows to set. Must be a non-negative integer.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task SetActualRows(int rows);

		/// <summary>
		/// Sets the actual number of columns to be used for subsequent operations.
		/// </summary>
		/// <param name="columns">The number of columns to set. Must be a positive integer.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task SetActualColumns(int columns);

		/// <summary>
		/// Gets the relative height value bound to the model.
		/// </summary>
		BindProperty<double, TModel> RelativeHeight { get; }

		/// <summary>
		/// Gets the relative widths for each column in the layout as a bindable property.
		/// </summary>
		/// <remarks>The array represents proportional widths, where each value determines the relative size of a
		/// column compared to others. The sum of all values does not need to equal 1; proportions are calculated based on the
		/// array contents. This property is typically used to control column sizing in dynamic layouts.</remarks>
		BindProperty<double[], TModel> RelativeWidths { get; }
	}
}