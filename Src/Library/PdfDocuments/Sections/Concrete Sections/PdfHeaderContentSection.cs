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
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Performs asynchronous initialization logic for the grid element before rendering.
		/// </summary>
		/// <param name="g">The PDF grid page on which the element will be rendered.</param>
		/// <param name="m">The model containing data relevant to the grid element.</param>
		/// <param name="bounds">The bounds within which the element should be rendered.</param>
		/// <returns>A task that represents the asynchronous initialization operation.</returns>
		protected override Task OnInitializeAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (!this.IsInitialized)
			{
				int i = 0;
				string controlStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string headerStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string containerStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;

				this.StyleNames = [controlStyle];

				IPdfSection<TModel>[] innerItems = 
				[
					Pdf.TextBlockSection<TModel>()
						.WithText(this.Text)
						.WithStyles(headerStyle)
						.WithZOrder(2)
						.WithParentSection(this),
					Pdf.ContentSection<TModel>()
						.WithStyles(containerStyle)
						.WithZOrder(1)
						.AddChildren(this.Children)
						.WithParentSection(this)
				];

				this.Children = innerItems;
				this.Text = string.Empty;

				this.IsInitialized = true;
			}

			return Task.CompletedTask;
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
				return PdfSectionsLayoutMode.VerticalStacking;
			}
			set
			{
			}
		}
	}
}
