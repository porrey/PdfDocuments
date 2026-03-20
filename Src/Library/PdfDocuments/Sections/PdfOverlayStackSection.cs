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
	/// Represents a PDF section that overlays multiple child sections in a stack, rendering them within the same bounds on
	/// a page.
	/// </summary>
	/// <remarks>Use this class to group and overlay multiple PDF sections so that each child section is rendered on
	/// top of the others within the specified bounds. Only child sections whose ShouldRender condition evaluates to true
	/// will be included in the overlay. This is useful for scenarios where layered content or composite visual elements
	/// are required in a PDF document.</remarks>
	/// <typeparam name="TModel">The type of model data used for rendering the section. Must implement the IPdfModel interface.</typeparam>
	public class PdfOverlayStackSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfOverlayStackSection class.
		/// </summary>
		public PdfOverlayStackSection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfOverlayStackSection class with the specified child sections.
		/// </summary>
		/// <param name="children">An array of child sections to be included in the overlay stack. Cannot be null; may be empty if no child sections
		/// are required.</param>
		public PdfOverlayStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

		/// <summary>
		/// Performs asynchronous layout of all child sections within the specified bounds on the given PDF grid page.
		/// </summary>
		/// <remarks>Only sections that are determined to be renderable are included in the layout process. The method
		/// stops processing further sections if any section fails to layout successfully.</remarks>
		/// <param name="g">The PDF grid page on which the child sections will be laid out.</param>
		/// <param name="m">The model data used to determine rendering and layout for each section.</param>
		/// <param name="bounds">The bounds within which the child sections should be arranged.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all sections were
		/// successfully laid out; otherwise, <see langword="false"/>.</returns>
		protected override async Task<bool> OnLayoutChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get a list of each section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(g, m)).ToArray();

			foreach (IPdfSection<TModel> section in sections)
			{
				section.ActualBounds = bounds;
			}

			//
			// Allow each section to perform a layout.
			//
			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.LayoutAsync(g, m))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}
	}
}
