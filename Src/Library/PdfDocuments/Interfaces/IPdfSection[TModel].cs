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
		/// Gets or sets a value indicating whether the style has been explicitly overridden.
		/// </summary>
		bool StyleOverridden { get; set; }

		/// <summary>
		/// Gets the style settings applied to this instance of a PDF section.
		/// </summary>
		PdfStyle<TModel> Style { get; }

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
		/// Gets or sets the sizing mode used to determine how the control adjusts its size.
		/// </summary>
		SectionSizingMode SizingMode(PdfGridPage g, TModel m);

		/// <summary>
		/// Gets or sets the layout mode used for arranging child sections.
		/// </summary>
		/// <remarks>Use this property to control how child sections are positioned and displayed within the parent
		/// container. The selected layout mode determines the arrangement behavior for all immediate child
		/// sections.</remarks>
		ChildSectionsLayoutMode ChildLayoutMode { get; set; }

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
		PdfBounds ActualBounds { get; }

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
		/// Asynchronously renders the specified model onto the provided PDF grid page within the given bounds.
		/// </summary>
		/// <param name="gridPage">The PDF grid page on which the model will be rendered. Cannot be null.</param>
		/// <param name="model">The data model to render onto the grid page. Cannot be null.</param>
		/// <param name="bounds">The bounds within the grid page that define the area for rendering the model.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if rendering was
		/// successful; otherwise, <see langword="false"/>.</returns>
		Task RenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds);

		/// <summary>
		/// Asynchronously calculates the bounding rectangle for the specified PDF grid page and data model.
		/// </summary>
		/// <param name="gridPage">The PDF grid page for which to calculate bounds. Cannot be null.</param>
		/// <param name="model">The data model used to determine the layout and bounds. Cannot be null.</param>
		/// <param name="parentBounds">The bounds of the parent section, which can be used to calculate relative positioning. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a PdfBounds object representing the
		/// calculated bounds for the specified grid page and model.</returns>
		Task<PdfSize> CalculateBoundsAsync(PdfGridPage gridPage, TModel model, PdfBounds parentBounds);

		/// <summary>
		/// Asynchronously renders debug information onto the specified PDF grid page using the provided model.
		/// </summary>
		/// <param name="gridPage">The PDF grid page on which debug information will be rendered. Cannot be null.</param>
		/// <param name="model">The model containing data used to generate debug output. Cannot be null.</param>
		/// <param name="bounds">The bounds within the grid page that define the area for rendering the model.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if debug information
		/// was successfully rendered; otherwise, <see langword="false"/>.</returns>
		Task RenderDebugAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds);
	}
}