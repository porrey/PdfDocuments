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
	/// Represents a footer section template for a PDF page, allowing customizable text content in each corner of the
	/// footer.
	/// </summary>
	/// <remarks>Use this class to define footer content for PDF pages, with support for binding text to the
	/// top-left, top-right, bottom-left, and bottom-right corners. Footer content and styles can be dynamically resolved
	/// based on the provided model. This section is typically rendered at the bottom of each PDF page and can be
	/// customized for different document layouts.</remarks>
	/// <typeparam name="TModel">The model type used to bind footer content and styles. Must implement the IPdfModel interface.</typeparam>
	public class PdfPageFooterSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Gets or sets the text displayed in the top-left area of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> TopLeftText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the top-right corner of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> TopRightText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the bottom-left area of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> BottomLeftText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the text displayed in the bottom-right corner of the control.
		/// </summary>
		public virtual BindProperty<string, TModel> BottomRightText { get; set; } = string.Empty;

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
				string topLeftSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string topRightSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string bottomLeftSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string bottomRightSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;

				this.StyleNames = [controlStyle];

				IPdfSection<TModel>[] innerItems =
				[
					Pdf.HorizontalStackSection<TModel>(
						Pdf.TextBlockSection<TModel>()
							.WithText(this.TopLeftText)
							.WithStyles(topLeftSyle)
							.WithZOrder(4),
						Pdf.TextBlockSection<TModel>()
							.WithText(this.TopRightText)
							.WithStyles(topRightSyle)
							.WithZOrder(3)
					).WithParentSection(this)
					 .WithZOrder(2),
					Pdf.HorizontalStackSection<TModel>(
						Pdf.TextBlockSection<TModel>()
							.WithText(this.BottomLeftText)
							.WithStyles(bottomLeftSyle)
							.WithZOrder(2),
						Pdf.TextBlockSection<TModel>()
							.WithText(this.BottomRightText)
							.WithStyles(bottomRightSyle)
							.WithZOrder(1)
					).WithParentSection(this)
					 .WithZOrder(1)
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
