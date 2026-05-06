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
	/// Represents a PDF section template for rendering a signature area, including date, image, and customizable labels.
	/// </summary>
	/// <remarks>This class is intended for use in PDF document generation scenarios where a signature section is
	/// required. It provides bindable properties for the date label, signature image, and date value, allowing
	/// customization based on the provided model. The section layout and rendering are handled according to the resolved
	/// style and model data.</remarks>
	/// <typeparam name="TModel">The model type used for binding section properties. Must implement the IPdfModel interface.</typeparam>
	public class PdfSignatureSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Gets or sets the binding options for configuring the signature input and output for the model.
		/// </summary>
		/// <remarks>Use this property to customize how signature data is handled and presented for the associated
		/// model. The options may affect validation, display, or processing of signature-related fields.</remarks>
		public virtual BindProperty<SignatureOptions<TModel>, TModel> SignatureOptions { get; set; } = new SignatureOptions<TModel>();

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
				string lineStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string signatureTextSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string signatureImageSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string dateLabelSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string dateSyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();

				this.StyleNames = [controlStyle];

				//
				// Get the signature options.
				//
				SignatureOptions<TModel> options = this.SignatureOptions.Resolve(g, m);

				if (options != null)
				{
					//
					// Get the date.
					//
					DateTimeOffset? date = options.Date.Resolve(g, m);

					IPdfSection<TModel>[] innerItems =
					[
						Pdf.HorizontalStackSection<TModel>(
							Pdf.TextBlockSection<TModel>()
								.WithText($"{options.SignatureText.Resolve(g, m)}:")
								.WithStyles(signatureTextSyle)
								.WithZOrder(2)
								.WithParentSection(this),
							Pdf.ImageSection<TModel>()
								.WithImage(options.SignatureImage.Resolve(g, m))
								.WithStyles(signatureImageSyle)
								.WithZOrder(4)
								.WithParentSection(this),
							Pdf.TextBlockSection<TModel>()
								.WithText($"{options.DateLabel.Resolve(g, m)}:")
								.WithStyles(dateLabelSyle)
								.WithZOrder(3)
								.WithParentSection(this),
							Pdf.TextBlockSection<TModel>()
								.WithText(date.HasValue ? $"{date.Value:d}" : string.Empty)
								.WithStyles(dateSyle)
								.WithZOrder(5)
								.WithParentSection(this)
						).WithStyles(PdfStyleManager<TModel>.Default)
							.WithZOrder(1)
							.WithParentSection(this),
						Pdf.HorizontalLineSection<TModel>()
							.WithStyles(lineStyle)
							.WithZOrder(0)
							.WithParentSection(this)
					];

					this.Children = innerItems;
					this.Text = string.Empty;

					this.IsInitialized = true;
				}
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

		//protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		//{
		//	bool returnValue = true;

		//	//
		//	// Get the signature options.
		//	//
		//	SignatureOptions<TModel> options = this.SignatureOptions.Resolve(g, m);
		//	PdfSize offset = options.Offset.Resolve(g, m);

		//	//
		//	// Get the style.
		//	//
		//	PdfStyle<TModel> style = this.ResolveStyle(0);
		//	PdfSpacing padding = style.Padding.Resolve(g, m);

		//	//
		//	// Use the standard small body font.
		//	//
		//	string signatureText = $"{options.SignatureText.Resolve(g, m)}:";
		//	XFont bodyFont = style.Font.Resolve(g, m);
		//	PdfSize bodyFontSize = g.MeasureText(bodyFont, signatureText);

		//	//
		//	// Draw the signature line.
		//	//
		//	int top = bounds.BottomRow - (2 * padding.Bottom);
		//	g.DrawHorizontalLine(top, bounds.LeftColumn, bounds.RightColumn, PdfRowEdge.Bottom, style.BorderWidth.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

		//	//
		//	// Get the relative width of the sections.
		//	//
		//	double[] widths = style.RelativeWidths.Resolve(g, m);
		//	double width = widths.Length > 0 ? widths[0] : .4;

		//	//
		//	// Draw the text.
		//	//
		//	top -= bodyFontSize.Rows + padding.Bottom;

		//	g.DrawText(signatureText, bodyFont,
		//		bounds.LeftColumn + padding.Left,
		//		top,
		//		bounds.Columns - (padding.Left + padding.Right),
		//		bodyFontSize.Rows,
		//		style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

		//	int left = bounds.RightColumn - (int)(bounds.Columns * width);

		//	//
		//	// Draw the date label.
		//	//
		//	string dateLabel = $"{options.DateLabel.Resolve(g, m)}:";

		//	g.DrawText(dateLabel, bodyFont,
		//		left,
		//		top,
		//		bounds.Columns - (padding.Left + padding.Right),
		//		bodyFontSize.Rows,
		//		style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));

		//	//
		//	// Get the date.
		//	//
		//	DateTimeOffset? date = options.Date.Resolve(g, m);

		//	if (date.HasValue)
		//	{
		//		//
		//		// Draw the date.
		//		//
		//		PdfSize dateSize = g.MeasureText(bodyFont, dateLabel);

		//		g.DrawText(date.Value.ToString("D"), bodyFont,
		//			left + dateSize.Columns + offset.Columns,
		//			top + offset.Rows,
		//			bounds.Columns - (padding.Left + padding.Right),
		//			bodyFontSize.Rows,
		//			style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));
		//	}

		//	//
		//	// Draw the image .
		//	//
		//	string path = options.SignatureImage.Resolve(g, m);
		//	PdfSize size = g.MeasureText(bodyFont, signatureText);

		//	if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
		//	{
		//		g.DrawImageWithFixedHeight(path, bounds.LeftColumn + padding.Left + size.Columns + offset.Columns, bounds.TopRow + padding.Top + offset.Rows, bounds.Rows - (padding.Top + padding.Bottom));
		//	}

		//	return Task.FromResult(returnValue);
		//}
	}
}
