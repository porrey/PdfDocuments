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
	/// Provides static helper methods for constructing and configuring PDF section objects in a composable manner.
	/// </summary>
	/// <remarks>The Pdf class offers a fluent API for building complex PDF document structures by composing,
	/// styling, and configuring sections. It includes methods for creating common section types, setting styles, managing
	/// section hierarchies, and applying content or layout options. These extension methods are intended to simplify the
	/// creation and customization of PDF documents using strongly-typed models. All methods are static and are designed to
	/// be used with types implementing IPdfSection<![CDATA[<TModel>]]>;.</remarks>
	public static class Pdf
	{
		/// <summary>
		/// Creates a section that arranges PDF content vertically in a stack.
		/// </summary>
		/// <remarks>Use this method to compose multiple PDF elements in a vertical layout. The section can be further
		/// configured or populated with child elements as needed.</remarks>
		/// <typeparam name="TModel">The type of the model used to bind data to the PDF section. Must implement IPdfModel.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing a vertically stacked section in the PDF document.</returns>
		public static IPdfSection<TModel> VerticalStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfVerticalStackSection<TModel>();
		}

		/// <summary>
		/// Creates a section that arranges its child sections vertically in the PDF document.
		/// </summary>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <param name="children">An array of child sections to be stacked vertically. Cannot be null; each element represents a section to include
		/// in the vertical stack.</param>
		/// <returns>A PDF section that displays the specified child sections in a vertical arrangement.</returns>
		public static IPdfSection<TModel> VerticalStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfVerticalStackSection<TModel>(children);
		}

		/// <summary>
		/// Creates a section that arranges PDF elements horizontally in a stack.
		/// </summary>
		/// <remarks>Use this method to build layouts where elements are placed side by side within a PDF document.
		/// The section can be customized by adding child elements and configuring their properties.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance that stacks its child elements horizontally.</returns>
		public static IPdfSection<TModel> HorizontalStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfHorizontalStackSection<TModel>();
		}

		/// <summary>
		/// Creates a section that arranges child PDF sections horizontally in a stack.
		/// </summary>
		/// <remarks>The order of the child sections determines their placement from left to right. This method is
		/// useful for building composite layouts where multiple sections need to appear in a single row.</remarks>
		/// <typeparam name="TModel">The type of the model associated with each PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="children">An array of child sections to be arranged horizontally. Cannot be null.</param>
		/// <returns>A section that displays the specified child sections side by side in a horizontal layout.</returns>
		public static IPdfSection<TModel> HorizontalStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfHorizontalStackSection<TModel>(children);
		}

		/// <summary>
		/// Creates a new overlay stack section for PDF generation using the specified model type.
		/// </summary>
		/// <typeparam name="TModel">The type of model used for the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing an overlay stack section for the given model type.</returns>
		public static IPdfSection<TModel> OverlayStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfOverlayStackSection<TModel>();
		}

		/// <summary>
		/// Creates a section that overlays multiple PDF sections in a stack, allowing their content to be rendered together.
		/// </summary>
		/// <remarks>Use this method to combine multiple sections visually in a single layer. The resulting section
		/// renders all child sections together, which can be useful for composite layouts or layered content.</remarks>
		/// <typeparam name="TModel">The type of the model used by the PDF sections. Must implement the IPdfModel interface.</typeparam>
		/// <param name="children">An array of PDF sections to be overlaid in the stack. The order of sections determines their stacking sequence.</param>
		/// <returns>A PDF section representing the overlay of the specified child sections.</returns>
		public static IPdfSection<TModel> OverlayStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfOverlayStackSection<TModel>(children);
		}

		/// <summary>
		/// Creates a section for capturing a digital signature within a PDF document model.
		/// </summary>
		/// <remarks>Use this method to add a digital signature area to a PDF document. The returned section can be
		/// integrated into the document's structure as needed.</remarks>
		/// <typeparam name="TModel">The type of PDF model to which the signature section will be added. Must implement IPdfModel.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing the signature section for the specified PDF model.</returns>
		public static IPdfSection<TModel> SignatureSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfSignatureSection<TModel>();
		}

		/// <summary>
		/// Creates an empty PDF section for the specified model type.
		/// </summary>
		/// <remarks>Use this method to generate a placeholder or no-content section within a PDF document when no
		/// data is available or required.</remarks>
		/// <typeparam name="TModel">The type of model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <returns>An <see cref="IPdfSection{TModel}"/> instance representing an empty section for the given model type.</returns>
		public static IPdfSection<TModel> EmptySection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfEmptySection<TModel>();
		}

		/// <summary>
		/// Creates a new content section and adds the specified child section to it.
		/// </summary>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="value">The child section to be added to the new content section. Cannot be null.</param>
		/// <returns>A new <see cref="IPdfSection{TModel}"/> containing the specified child section.</returns>
		public static IPdfSection<TModel> ContentSection<TModel>(IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			return (new PdfContentSection<TModel>()).AddChildSection(value);
		}

		/// <summary>
		/// Creates a section representing the header content for a PDF document model.
		/// </summary>
		/// <remarks>Use this method to add a header content section to a PDF document when building or customizing
		/// document layouts. The returned section can be further configured or added to a document structure as
		/// needed.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing the header content section for the specified PDF model type.</returns>
		public static IPdfSection<TModel> HeaderContentSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfHeaderContentSection<TModel>();
		}

		/// <summary>
		///	Creates a PDF section that displays a collection of key-value pairs for the specified model type.
		/// </summary>
		/// <remarks>Use this method to generate a section in a PDF document that presents multiple key-value pairs,
		/// typically for displaying structured data related to the model. The order of items in the array determines their
		/// display order in the section.</remarks>
		/// <typeparam name="TModel">The type of model associated with each key-value item. Must implement IPdfModel.</typeparam>
		/// <param name="values">An array of key-value items to include in the section. Cannot be null.</param>
		/// <returns>A PDF section containing the provided key-value items for the specified model type.</returns>
		public static IPdfSection<TModel> KeyValueSection<TModel>(params PdfKeyValueItem<TModel>[] values)
			where TModel : IPdfModel
		{
			return new PdfKeyValueSection<TModel>(values);
		}

		/// <summary>
		/// Creates a new PDF section that displays multiple text elements stacked vertically.
		/// </summary>
		/// <remarks>Use this method to add a section to a PDF document where each text element is rendered in its own
		/// row. This is useful for displaying lists or grouped textual content. The returned section can be further
		/// configured or added to a PDF layout as needed.</remarks>
		/// <typeparam name="TModel">The type of the model used to bind data to the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing a vertically stacked text section in the PDF document.</returns>
		public static IPdfSection<TModel> StackedTextSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfStackedTextSection<TModel>();
		}

		/// <summary>
		/// Creates a new section for rendering a block of text in a PDF document using the specified model type.
		/// </summary>
		/// <remarks>Use this method to add a text block section to a PDF layout when building documents
		/// programmatically. The returned section can be customized and added to a PDF template as needed.</remarks>
		/// <typeparam name="TModel">The type of model used to provide data for the text block section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing a text block section configured for the specified model type.</returns>
		public static IPdfSection<TModel> TextBlockSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfTextBlockSection<TModel>();
		}

		/// <summary>
		/// Creates a section that wraps text within a PDF document for the specified model type.
		/// </summary>
		/// <remarks>Use this method to add text content to a PDF section with automatic line wrapping. The returned
		/// section can be customized and added to a PDF document as needed.</remarks>
		/// <typeparam name="TModel">The type of model used to generate the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance that wraps text for the given model type.</returns>
		public static IPdfSection<TModel> WrappingTextSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfWrappingTextSection<TModel>();
		}

		/// <summary>
		/// Creates a new page header section for a PDF document model.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model associated with the section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing the page header section for the specified PDF model type.</returns>
		public static IPdfSection<TModel> PageHeaderSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfPageHeaderSection<TModel>();
		}

		/// <summary>
		/// Creates a new page footer section for a PDF document using the specified model type.
		/// </summary>
		/// <remarks>Use this method to add a footer section to a PDF page when building documents with custom models.
		/// The returned section can be further configured or populated as needed.</remarks>
		/// <typeparam name="TModel">The type of model used to configure the footer section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance representing the page footer section for the PDF document.</returns>
		public static IPdfSection<TModel> PageFooterSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfPageFooterSection<TModel>();
		}

		/// <summary>
		/// Creates a new PDF section that displays an image based on the specified model type.
		/// </summary>
		/// <typeparam name="TModel">The type of model used to provide data for the image section. Must implement the IPdfModel interface.</typeparam>
		/// <returns>A PDF section instance that renders an image using the provided model type.</returns>
		public static IPdfSection<TModel> ImageSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfImageSection<TModel>();
		}

		/// <summary>
		/// Creates a new section for displaying a data grid in a PDF document.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model that defines the document's structure.</typeparam>
		/// <typeparam name="TItem">The type of the items to be displayed in the data grid.</typeparam>
		/// <returns>A section instance that renders a data grid for the specified item type within the PDF model.</returns>
		public static IPdfSection<TModel> DataGridSection<TModel, TItem>()
			where TModel : IPdfModel
		{
			return new PdfDataGridSection<TModel, TItem>();
		}

		/// <summary>
		/// Creates a new PDF section for rendering a data row based on the specified model and item types.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model. Must implement the IPdfModel interface.</typeparam>
		/// <typeparam name="TItem">The type of the data item represented in the row.</typeparam>
		/// <returns>An IPdfSection<![CDATA[<TModel>]]> instance configured to render a data row for the specified item type.</returns>
		public static IPdfSection<TModel> DataRowsSection<TModel, TItem>()
			where TModel : IPdfModel
		{
			return new PdfDataRowsSection<TModel, TItem>();
		}
	}
}
