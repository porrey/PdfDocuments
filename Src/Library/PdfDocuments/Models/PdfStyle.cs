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
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	/// <summary>
	/// Provides a configurable set of style properties for PDF sections, supporting data binding to a model type.
	/// </summary>
	/// <remarks>Use this class to define visual appearance and layout for PDF components, such as fonts, colors,
	/// spacing, and alignment. Each property supports binding to values from the specified model type, enabling dynamic
	/// styling based on model data.</remarks>
	/// <typeparam name="TModel">The type of model to which style properties are bound. Must implement <see cref="IPdfModel"/>.</typeparam>
	public class PdfStyle<TModel> : IStyleBuilder<TModel>
			where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the relative height value for the model, expressed as a proportion of the total available height.
		/// </summary>
		public virtual BindProperty<double, TModel> RelativeHeight { get; set; } = 1;

		/// <summary>
		/// Gets or sets the fixed height for the model, specified in rows. This value takes precedence over relative height.
		/// </summary>
		public virtual BindProperty<int?, TModel> FixedHeight { get; set; } = null;

		/// <summary>
		/// Gets or sets the relative widths for each column in the layout.
		/// </summary>
		/// <remarks>Each value in the array represents the proportional width of a column. The sum of all values
		/// determines the overall distribution. Values must be non-negative. Adjusting this property changes how space is
		/// allocated among columns.</remarks>
		public virtual BindProperty<double[], TModel> RelativeWidths { get; set; } = new double[] { 1 };

		/// <summary>
		/// Gets or sets the fixed widths for each column in the layout.
		/// </summary>
		/// <remarks>Each value in the array represents the fixed width of a column. The sum of all values
		/// determines the overall distribution. Values must be non-negative. Adjusting this property changes how space is
		/// allocated among columns. This value takes precedence over relative widths.</remarks>
		public virtual BindProperty<int[], TModel> FixedWidths { get; set; } = Array.Empty<int>();

		/// <summary>
		/// Gets or sets the font used for rendering text in the bound model.
		/// </summary>
		public virtual BindProperty<XFont, TModel> Font { get; set; } = new XFont(GlobalPdfDocumentsSettings.DefaultFontName, 12);

		/// <summary>
		/// Gets or sets the margin spacing applied to the PDF content.
		/// </summary>
		/// <remarks>Use this property to specify the amount of space between the content and the edges of the PDF
		/// page. Adjusting the margin can affect layout and readability.</remarks>
		public virtual BindProperty<PdfSpacing, TModel> Margin { get; set; } = new PdfSpacing(0, 0, 0, 0);

		/// <summary>
		/// Gets or sets the padding applied to the content, specifying the space between the content and its boundaries.
		/// </summary>
		/// <remarks>The padding is defined using the four sides: left, top, right, and bottom. Adjusting the padding
		/// affects the layout and spacing of the content within its container.</remarks>
		public virtual BindProperty<PdfSpacing, TModel> Padding { get; set; } = new PdfSpacing(0, 0, 0, 0);

		/// <summary>
		/// Gets or sets the foreground color used for rendering the associated model.
		/// </summary>
		public virtual BindProperty<XColor, TModel> ForegroundColor { get; set; } = XColors.Black;

		/// <summary>
		/// Gets or sets the background color for the element.
		/// </summary>
		public virtual BindProperty<XColor, TModel> BackgroundColor { get; set; } = XColors.Transparent;

		/// <summary>
		/// Gets or sets the width of the border, in pixels, for the associated model element.
		/// </summary>
		public virtual BindProperty<double, TModel> BorderWidth { get; set; } = 0;

		/// <summary>
		/// Gets or sets the border color for the element.
		/// </summary>
		/// <remarks>The border color determines the visual appearance of the element's border. Setting this property
		/// to a transparent color will render the border invisible. The value can be data-bound to dynamically update the
		/// border color based on the model.</remarks>
		public virtual BindProperty<XColor, TModel> BorderColor { get; set; } = XColors.Transparent;

		/// <summary>
		/// Gets or sets the text alignment format applied to the content.
		/// </summary>
		/// <remarks>Use this property to control how text is aligned within the element. The default value aligns
		/// text to the left and centers it vertically.</remarks>
		public virtual BindProperty<XStringFormat, TModel> TextAlignment { get; set; } = XStringFormats.CenterLeft;

		/// <summary>
		/// Gets or sets the paragraph alignment for the bound model.
		/// </summary>
		/// <remarks>Use this property to specify how text within a paragraph is aligned, such as left, right, center,
		/// or justified when text wrapping is true. The default alignment is justified. Changing the alignment 
		/// affects the visual layout of paragraph content.</remarks>
		public virtual BindProperty<XParagraphAlignment, TModel> ParagraphAlignment { get; set; } = XParagraphAlignment.Justify;

		/// <summary>
		/// Gets or sets a value indicating whether text content should wrap within the section.
		/// </summary>
		public virtual BindProperty<bool, TModel> WrapText { get; set; } = false;

		/// <summary>
		/// Gets or sets the padding applied to each cell within the table.
		/// </summary>
		/// <remarks>Cell padding determines the space between the cell content and its borders. Adjusting this
		/// property affects the visual layout and spacing of table cells.</remarks>
		public virtual BindProperty<PdfSpacing, TModel> CellPadding { get; set; } = new PdfSpacing(0, 0, 0, 0);
	}
}
