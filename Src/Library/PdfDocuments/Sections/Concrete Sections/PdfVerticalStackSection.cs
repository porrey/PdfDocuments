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
	/// Represents a PDF section template that arranges child sections vertically within a grid page layout.
	/// </summary>
	/// <remarks>Child sections are stacked from top to bottom, with their heights determined by relative height
	/// settings or evenly distributed if unspecified. This class is useful for creating composite PDF layouts where
	/// multiple sections need to be rendered in a vertical sequence.</remarks>
	/// <typeparam name="TModel">The type of model used for rendering the PDF section. Must implement the IPdfModel interface.</typeparam>
	public class PdfVerticalStackSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfVerticalStackSection class.
		/// </summary>
		public PdfVerticalStackSection()
		{
			this.SectionLayoutMode = PdfSectionsLayoutMode.VerticalStacking;
		}

		/// <summary>
		/// Initializes a new instance of the PdfVerticalStackSection class with the specified child sections arranged
		/// vertically.
		/// </summary>
		/// <remarks>Each child section is rendered in order, from top to bottom. Use this constructor to compose
		/// complex layouts by combining multiple sections.</remarks>
		/// <param name="children">An array of child sections to be stacked vertically within this section. Cannot be null; may be empty to create an
		/// empty stack.</param>
		public PdfVerticalStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
			this.SectionLayoutMode = PdfSectionsLayoutMode.VerticalStacking;
		}
	}
}
