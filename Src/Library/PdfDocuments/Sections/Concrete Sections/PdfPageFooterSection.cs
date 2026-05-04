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
			if (this.Children.Count == 0)
			{
				int i = 1;
				string topLeftSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string topRightSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string bottomLeftSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string bottomRightSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();

				this.AddChildren
				(
					Pdf.VerticalStackSection<TModel>
					(
						Pdf.HorizontalStackSection<TModel>(
							Pdf.TextBlockSection<TModel>()
								.WithText(this.TopLeftText)
								.WithStyles(topLeftSyle),
							Pdf.TextBlockSection<TModel>()
								.WithText(this.TopRightText)
								.WithStyles(topRightSyle)
						),
						Pdf.HorizontalStackSection<TModel>(
							Pdf.TextBlockSection<TModel>()
								.WithText(this.BottomLeftText)
								.WithStyles(bottomLeftSyle),
							Pdf.TextBlockSection<TModel>()
								.WithText(this.BottomRightText)
								.WithStyles(bottomRightSyle)
						)
					)
				);
			}

			return Task.CompletedTask;
		}
	}
}
