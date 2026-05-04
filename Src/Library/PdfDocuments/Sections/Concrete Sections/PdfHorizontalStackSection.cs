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
	/// Represents a section in a PDF document that arranges its child sections horizontally, distributing available space
	/// according to relative widths or evenly when unspecified.
	/// </summary>
	/// <remarks>Child sections are laid out side by side within the section's bounds. Sections with specified
	/// relative widths receive proportional space, while others share remaining space equally. Padding and layout are
	/// applied to each child section. This class is typically used to compose complex PDF layouts by stacking multiple
	/// sections horizontally.</remarks>
	/// <typeparam name="TModel">The model type used for rendering and layout operations. Must implement the IPdfModel interface.</typeparam>
	public class PdfHorizontalStackSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfHorizontalStackSection class, which represents a section that arranges PDF
		/// sections horizontally.
		/// </summary>
		/// <remarks>Use this constructor to create a horizontal stack section for organizing PDF content side by
		/// side. This section can be added to a PDF document to group sections in a horizontal layout.</remarks>
		public PdfHorizontalStackSection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfHorizontalStackSection class with the specified child sections.
		/// </summary>
		/// <remarks>Each child section will be rendered side by side in the order provided. Use this constructor to
		/// compose complex layouts from multiple sections.</remarks>
		/// <param name="children">An array of child sections to be arranged horizontally within this section. Cannot be null or contain null
		/// sections.</param>
		public PdfHorizontalStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

		/// <summary>
		/// Gets or sets the layout mode used for arranging sections within the PDF document.
		/// </summary>
		/// <remarks>The layout mode determines how sections are visually organized when rendering the document.
		/// Setting this property may have no effect if the implementation does not support changing the layout
		/// mode.</remarks>
		public override PdfSectionsLayoutMode SectionLayoutMode
		{
			get
			{
				return PdfSectionsLayoutMode.HorizontalStacking;
			}
			set
			{
			}
		}
	}
}
