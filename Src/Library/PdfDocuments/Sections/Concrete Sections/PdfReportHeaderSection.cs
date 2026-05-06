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
	/// Specifies the horizontal alignment options for displaying a logo.
	/// </summary>
	public enum PdfLogoPosition
	{
		/// <summary>
		/// Gets or sets the left coordinate of the element.
		/// </summary>
		Left,
		/// <summary>
		/// Represents the right direction or alignment.
		/// </summary>
		Right
	}

	/// <summary>
	/// Represents a header section for a PDF, supporting customizable logo and title rendering using a model-driven
	/// template.
	/// </summary>
	/// <remarks>Use this class to define and render a report header in a PDF document, with support for binding logo
	/// and title values from the provided model. The header section can be styled and positioned according to template
	/// settings. This class is typically used as part of a larger PDF generation workflow.</remarks>
	/// <typeparam name="TModel">The type of model used to bind data for the header section. Must implement the IPdfModel interface.</typeparam>
	public class PdfReportHeaderSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Gets or sets the logo text associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> Logo { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the title associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> Title { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the title associated with the model.
		/// </summary>
		public virtual BindProperty<PdfLogoPosition, TModel> LogoPosition { get; set; } = PdfLogoPosition.Left;

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
				string logoStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;
				string titleStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;

				this.StyleNames = [controlStyle];

				IPdfSection<TModel>[] innerItems = [];

				if (this.LogoPosition.Resolve(g,m) == PdfLogoPosition.Left)
				{
					innerItems =
					[
						Pdf.ImageSection<TModel>()
							.WithImage(this.Logo)
							.WithStyles(logoStyle)
							.WithZOrder(2)
							.WithParentSection(this),
						Pdf.TextBlockSection<TModel>()
							.WithText(this.Title)
							.WithStyles(titleStyle)
							.WithZOrder(1)
							.WithParentSection(this)
					];
				}
				else
				{
					innerItems =
					[
						Pdf.TextBlockSection<TModel>()
							.WithText(this.Title)
							.WithStyles(titleStyle)
							.WithZOrder(1)
							.WithParentSection(this),
						Pdf.ImageSection<TModel>()
							.WithImage(this.Logo)
							.WithStyles(logoStyle)
							.WithZOrder(2)
							.WithParentSection(this)
					];
				}
				
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
				return PdfSectionsLayoutMode.HorizontalStacking;
			}
			set
			{
			}
		}
	}
}
