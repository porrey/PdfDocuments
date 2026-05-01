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
	/// Provides static methods for creating and configuring PDF style builders for document sections. Enables fluent
	/// customization of visual properties such as font, color, spacing, alignment, and borders for models implementing the
	/// IPdfModel interface.
	/// </summary>
	/// <remarks>Use the methods in this class to construct and modify style definitions for PDF sections in a
	/// type-safe and composable manner. Styles can be built incrementally by chaining configuration methods, allowing for
	/// flexible reuse and adaptation. All methods are thread-safe when used with separate style builder instances. The
	/// class supports both property binding and direct value assignment for style attributes.</remarks>
	public static class Style
	{
		/// <summary>
		/// Creates a new style builder instance for the specified PDF model type.
		/// </summary>
		/// <typeparam name="TModel">The type of PDF model for which the style builder is created. Must implement the IPdfModel interface.</typeparam>
		/// <returns>A style builder instance for the specified PDF model type.</returns>
		public static IStyleBuilder<TModel> Create<TModel>()
				where TModel : IPdfModel
		{
			return new PdfStyle<TModel>();
		}

		/// <summary>
		/// Creates a new style builder instance by copying all style properties from the specified style.
		/// </summary>
		/// <remarks>Use this method to duplicate an existing style configuration for further customization without
		/// modifying the original style.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="style">The style to copy. Cannot be null.</param>
		/// <returns>A new <see cref="IStyleBuilder{TModel}"/> instance with properties copied from the specified style.</returns>
		public static IStyleBuilder<TModel> Copy<TModel>(this PdfStyle<TModel> style)
				where TModel : IPdfModel
		{
			return new PdfStyle<TModel>()
			{
				RelativeHeight = style.RelativeHeight,
				RelativeWidths = style.RelativeWidths,
				FixedHeight = style.FixedHeight,
				FixedWidths = style.FixedWidths,
				Font = style.Font,
				Margin = style.Margin,
				Padding = style.Padding,
				ForegroundColor = style.ForegroundColor,
				BackgroundColor = style.BackgroundColor,
				BorderWidth = style.BorderWidth,
				BorderColor = style.BorderColor,
				TextAlignment = style.TextAlignment,
				ParagraphAlignment = style.ParagraphAlignment,
				CellPadding = style.CellPadding
			};
		}

		/// <summary>
		/// Sets the font style for the PDF model using the specified font binding.
		/// </summary>
		/// <remarks>Use this method to specify the font for PDF rendering when building styles for a model. The font
		/// is set for subsequent style operations on the builder.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">The font binding to apply to the PDF model. Cannot be null.</param>
		/// <returns>The style builder instance with the font style applied, enabling further style configuration.</returns>
		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XFont, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the font style for the PDF model using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to define the font for PDF rendering when building styles for a model. This
		/// method supports fluent chaining of style configuration.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">A binding action that specifies the font to use for the PDF model. Cannot be null.</param>
		/// <returns>The style builder instance with the font style applied, enabling further style configuration.</returns>
		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the font family, size, and style for the PDF style builder.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the font for.</param>
		/// <param name="familyName">The name of the font family to use. Must be available on the system.</param>
		/// <param name="emSize">The font size, in em units, to apply.</param>
		/// <param name="style">The font style to apply, such as bold or italic.</param>
		/// <returns>The style builder instance with the specified font settings applied.</returns>
		/// <exception cref="Exception">Thrown if the specified font family or style is not available on the system.</exception>
		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize, XFontStyleEx style)
			where TModel : IPdfModel
		{
			try
			{
				((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize, style));
			}
			catch (InvalidOperationException)
			{
				throw new Exception("$The font '{familyName}' with style '{style}' is not available on the system.");
			}

			return styleBuilder;
		}

		/// <summary>
		/// Sets the font family and size for the style builder, allowing text rendering with the specified font in PDF
		/// documents.
		/// </summary>
		/// <remarks>This method updates the font settings for subsequent text rendering operations. If the specified
		/// font family is not available, PDF rendering may fall back to a default font.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the font for.</param>
		/// <param name="familyName">The name of the font family to use for text rendering. Cannot be null or empty.</param>
		/// <param name="emSize">The font size, in em units, to apply. Must be greater than zero.</param>
		/// <returns>The style builder instance with the font configuration applied, enabling fluent chaining of style settings.</returns>
		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize));
			return styleBuilder;
		}

		/// <summary>
		/// Sets the border width for the style builder using the specified binding value.
		/// </summary>
		/// <remarks>Use this method to specify the border width for PDF sections when building styles. The border
		/// width is applied to subsequent style configurations for the associated model.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the border width for.</param>
		/// <param name="value">A binding property representing the border width to apply. The value must be a non-negative double.</param>
		/// <returns>The style builder instance with the updated border width setting.</returns>
		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the border width for the style builder using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to specify the border width dynamically based on the model's properties. The
		/// border width is applied when rendering the PDF section.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the border width for.</param>
		/// <param name="value">A binding action that determines the border width value for the model. Cannot be null.</param>
		/// <returns>The style builder instance with the updated border width configuration.</returns>
		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the border color for the style builder using the specified binding property.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the border color for.</param>
		/// <param name="value">A binding property that specifies the border color to apply. Cannot be null.</param>
		/// <returns>The style builder instance with the updated border color, enabling fluent configuration.</returns>
		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the border color for the style builder using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to bind the border color dynamically based on the model's properties. This
		/// enables flexible styling for PDF sections.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the border color for.</param>
		/// <param name="value">A binding action that determines the border color based on the model. Cannot be null.</param>
		/// <returns>The style builder instance with the updated border color configuration.</returns>
		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the foreground color for the style builder using the specified binding property.
		/// </summary>
		/// <remarks>Use this method to bind a foreground color to the style builder, allowing dynamic color
		/// assignment based on the model's state.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the foreground color for.</param>
		/// <param name="value">A binding property representing the foreground color to apply to the style builder.</param>
		/// <returns>The style builder instance with the foreground color configured. Enables fluent chaining of style configuration
		/// methods.</returns>
		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the foreground color for the style builder using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to bind the foreground color dynamically based on the model's properties. This
		/// enables flexible styling for PDF sections.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A binding action that determines the foreground color based on the model.</param>
		/// <returns>The style builder instance with the foreground color configured.</returns>
		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the background color for the style builder using the specified binding property.
		/// </summary>
		/// <remarks>Use this method to bind a background color to a PDF style, allowing dynamic color assignment
		/// based on model properties.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the background color for.</param>
		/// <param name="value">A binding property that specifies the background color to apply. Cannot be null.</param>
		/// <returns>The style builder instance with the updated background color setting.</returns>
		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the background color for the style builder using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to bind the background color dynamically based on the model's properties. The
		/// background color will be applied when rendering the PDF section.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the background color for.</param>
		/// <param name="value">A binding action that determines the background color based on the model.</param>
		/// <returns>The style builder instance with the background color configured.</returns>
		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the cell padding for table cells in the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to specify custom spacing around cell content in PDF tables. Cell padding affects
		/// the visual layout and spacing within each table cell.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure PDF table styles.</param>
		/// <param name="value">A bindable property representing the cell padding to apply. Cannot be null.</param>
		/// <returns>The style builder instance with updated cell padding settings.</returns>
		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the cell padding for table cells in the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to customize the spacing inside table cells when generating PDF documents. Cell
		/// padding affects the layout and readability of table content.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure PDF table styles.</param>
		/// <param name="value">A delegate that binds the cell padding value to the model. Specifies the spacing to apply as padding within each
		/// cell.</param>
		/// <returns>The style builder instance with the updated cell padding configuration.</returns>
		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the cell padding for the PDF style builder using the specified left, top, right, and bottom values.
		/// </summary>
		/// <remarks>Cell padding values must be non-negative. This method enables customization of cell spacing for
		/// PDF table layouts.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure cell padding for.</param>
		/// <param name="left">The amount of padding to apply to the left side of the cell, in points.</param>
		/// <param name="top">The amount of padding to apply to the top side of the cell, in points.</param>
		/// <param name="right">The amount of padding to apply to the right side of the cell, in points.</param>
		/// <param name="bottom">The amount of padding to apply to the bottom side of the cell, in points.</param>
		/// <returns>The style builder instance with updated cell padding settings.</returns>
		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		/// <summary>
		/// Sets the relative height for the style using the specified binding value.
		/// </summary>
		/// <remarks>Use this method to define the height of an section relative to its container or context within a
		/// PDF model. The relative height is determined by the provided binding property and can be used for responsive
		/// layout scenarios.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A binding property that specifies the relative height value to use for the style.</param>
		/// <returns>The style builder instance with the relative height configured.</returns>
		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use a relative height value for the PDF section.
		/// </summary>
		/// <remarks>Use this method to specify the height of a PDF section as a proportion relative to its container
		/// or context. This is useful for responsive layouts where absolute sizing is not appropriate.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that provides the relative height value based on the model. Cannot be null.</param>
		/// <returns>The style builder instance with the relative height configuration applied.</returns>
		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets a fixed height for the styled PDF section using the specified bindable value.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property representing the fixed height, in points, to apply to the section. The value can be null to
		/// indicate no fixed height.</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseFixedHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<int?, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).FixedHeight = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use a fixed height for the section, using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to set a fixed height for PDF sections when generating documents. If the value is
		/// null, the section's height is determined automatically.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A binding action that provides the fixed height value, in points, for the section. The value can be null to
		/// indicate no fixed height.</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseFixedHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<int?, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).FixedHeight = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the margin spacing for the PDF style builder using the specified binding value.
		/// </summary>
		/// <remarks>Use this method to configure margin spacing for PDF sections in a fluent style-building workflow.
		/// The margin value is applied to the style builder and can be further customized with additional style
		/// settings.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure the margin for.</param>
		/// <param name="value">A binding property that specifies the margin spacing to apply to the PDF style builder.</param>
		/// <returns>The style builder instance with the updated margin configuration.</returns>
		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the margin style for the PDF model using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to specify custom margin settings for PDF sections. The margin is set based on
		/// the provided binding action, allowing dynamic or model-driven spacing.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">A binding action that defines how the margin spacing is applied to the PDF model.</param>
		/// <returns>The style builder instance with the margin style configured. Enables fluent chaining of style configuration
		/// methods.</returns>
		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the margin values for the style builder using the specified left, top, right, and bottom spacing.
		/// </summary>
		/// <remarks>Use this method to specify individual margin values for each side of a PDF section. Margin values
		/// are applied in points and should be non-negative to ensure correct layout.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure margin values for.</param>
		/// <param name="left">The margin value, in points, to apply to the left side. Must be non-negative.</param>
		/// <param name="top">The margin value, in points, to apply to the top side. Must be non-negative.</param>
		/// <param name="right">The margin value, in points, to apply to the right side. Must be non-negative.</param>
		/// <param name="bottom">The margin value, in points, to apply to the bottom side. Must be non-negative.</param>
		/// <returns>The style builder instance with updated margin settings.</returns>
		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		/// <summary>
		/// Sets the padding style for the PDF model using the specified binding property.
		/// </summary>
		/// <remarks>Use this method to define padding for PDF sections in a fluent style-building workflow. The
		/// padding is set based on the provided binding property, allowing dynamic or model-driven spacing.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">The binding property that specifies the padding values to apply to the PDF model.</param>
		/// <returns>The style builder instance with the updated padding configuration.</returns>
		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the padding for the style builder using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to set custom padding values for PDF sections when building styles. The padding
		/// is applied according to the provided binding action, which allows dynamic configuration based on the
		/// model.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure padding for.</param>
		/// <param name="value">A binding action that defines the padding settings to apply to the style builder.</param>
		/// <returns>The style builder instance with the updated padding configuration.</returns>
		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the padding values for the left, top, right, and bottom edges of the style builder.
		/// </summary>
		/// <remarks>Padding values are applied in points and affect the layout of content within the PDF model.
		/// Negative values may result in unexpected layout behavior.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure padding for.</param>
		/// <param name="left">The padding value, in points, to apply to the left edge. Must be non-negative.</param>
		/// <param name="top">The padding value, in points, to apply to the top edge. Must be non-negative.</param>
		/// <param name="right">The padding value, in points, to apply to the right edge. Must be non-negative.</param>
		/// <param name="bottom">The padding value, in points, to apply to the bottom edge. Must be non-negative.</param>
		/// <returns>The style builder instance with updated padding settings.</returns>
		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		/// <summary>
		/// Sets the text alignment style for the PDF model using the specified binding value.
		/// </summary>
		/// <remarks>Use this method to configure text alignment for PDF sections when building styles. The alignment
		/// is determined by the provided binding value, which allows dynamic formatting based on the model's state.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">A binding property that specifies the text alignment format to apply. Cannot be null.</param>
		/// <returns>The style builder instance with the updated text alignment style applied.</returns>
		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XStringFormat, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the text alignment for the style builder using the specified binding action.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A binding action that specifies how to set the <see cref="XStringFormat"/> text alignment for the model.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance to allow for method chaining.</returns>
		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XStringFormat, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the paragraph alignment style for the PDF model using the specified binding value.
		/// </summary>
		/// <remarks>Use this method to configure paragraph alignment for PDF content. The alignment is applied to
		/// paragraphs rendered by the model. This method supports fluent chaining of style configuration.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">The binding property that specifies the paragraph alignment to apply. Cannot be null.</param>
		/// <returns>The style builder instance with the updated paragraph alignment setting.</returns>
		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XParagraphAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the paragraph alignment style for the PDF model using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to control the horizontal alignment of paragraphs within the PDF document. This
		/// setting affects how text is positioned in each paragraph and can be combined with other style
		/// configurations.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to configure styles for the PDF model.</param>
		/// <param name="value">A binding action that specifies the paragraph alignment to apply to the model.</param>
		/// <returns>The style builder instance with the paragraph alignment style applied, enabling further style configuration.</returns>
		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XParagraphAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures whether text wrapping is enabled for the style builder.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder to configure.</param>
		/// <param name="wrapText">true to enable text wrapping; otherwise, false. The default is true.</param>
		/// <returns>The same style builder instance with the text wrapping setting applied.</returns>
		public static IStyleBuilder<TModel> UseTextWrapping<TModel>(this IStyleBuilder<TModel> styleBuilder, bool wrapText = true)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).WrapText = wrapText;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use relative column widths for table layout.
		/// </summary>
		/// <remarks>Relative widths allow columns to be sized proportionally based on the values provided. This
		/// method is typically used when creating tables with flexible column sizing in PDF documents.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property representing the array of relative column widths to apply. The array values determine the
		/// proportional widths of each column.</param>
		/// <returns>The style builder instance with relative widths configuration applied.</returns>
		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use relative column widths for table layouts.
		/// </summary>
		/// <remarks>Use this method to specify column widths as proportions rather than absolute values. This is
		/// useful for creating tables that adapt to varying content or page sizes.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that binds an array of relative column widths to the model. Each value represents the proportional
		/// width of a column.</param>
		/// <returns>The style builder instance with relative widths configuration applied.</returns>
		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use relative column widths for layout purposes.
		/// </summary>
		/// <remarks>Relative widths are used to proportionally distribute available space among columns. The sum of
		/// the values does not need to equal 1; each value is interpreted relative to the others.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="values">An array of relative width values to apply to columns. Each value determines the proportional width of a column
		/// relative to others.</param>
		/// <returns>The style builder instance with relative widths applied, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, params double[] values)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = new BindProperty<double[], TModel>((g, m) => values);
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use fixed column widths for the associated PDF model.
		/// </summary>
		/// <remarks>If the number of widths provided does not match the number of columns, only the specified columns
		/// will have fixed widths applied. Remaining columns will use default sizing.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property representing the array of fixed widths, in points, to apply to columns. Each section
		/// corresponds to a column; a value of <see langword="null"/> indicates that the width is not set for that column.</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseFixedWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<int[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).FixedWidths = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use fixed column widths when rendering the PDF table.
		/// </summary>
		/// <remarks>If fixed widths are specified, they override any automatic or proportional column sizing for the
		/// affected columns.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that provides an array of nullable integers representing the fixed widths for each column. The array
		/// length should match the number of columns in the table. A value of <see langword="null"/> for a column indicates
		/// that its width is not fixed.</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseFixedWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<int[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).FixedWidths = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the style builder to use fixed column widths for the associated PDF table or layout.
		/// </summary>
		/// <remarks>If the number of values does not match the number of columns, excess values are ignored and
		/// missing values may result in default widths being used.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="values">An array of integers specifying the fixed widths, in points, for each column. The length of the array should match
		/// the number of columns to be styled.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance to allow for method chaining.</returns>
		public static IStyleBuilder<TModel> UseFixedWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, params int[] values)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).FixedWidths = new BindProperty<int[], TModel>((g, m) => values);
			return styleBuilder;
		}

		/// <summary>
		/// Configures the horizontal alignment for images in the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to specify how images should be horizontally aligned within the generated PDF
		/// content. This setting affects all images rendered using the configured style.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that binds a <see cref="PdfHorizontalAlignment"/> value to the model. Determines the horizontal
		/// alignment of images.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance to allow for method chaining.</returns>
		public static IStyleBuilder<TModel> UseHorizontalImageAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfHorizontalAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).HorizontalImageAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the horizontal alignment for images within the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to control how images are horizontally aligned within PDF elements. This setting
		/// affects all images rendered using the configured style.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder. Must implement IPdfModel.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property specifying the horizontal alignment to apply to images.</param>
		/// <returns>The same style builder instance with the horizontal image alignment configured.</returns>
		public static IStyleBuilder<TModel> UseHorizontalImageAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfHorizontalAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).HorizontalImageAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the vertical alignment for images within the style builder.
		/// </summary>
		/// <remarks>Use this method to specify how images should be vertically aligned when rendering PDF content.
		/// This setting affects only image elements within the styled content.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that binds the vertical alignment value for images. The delegate receives the current model and returns
		/// the desired <see cref="PdfVerticalAlignment"/>.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance to allow for method chaining.</returns>
		public static IStyleBuilder<TModel> UseVerticalImageAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfVerticalAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).VerticalImageAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the vertical alignment for images within the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to control how images are vertically aligned within PDF elements. This setting
		/// affects the rendering of images in the generated PDF output.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property specifying the vertical alignment to apply to images.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance to allow for method chaining.</returns>
		public static IStyleBuilder<TModel> UseVerticalImageAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfVerticalAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).VerticalImageAlignment = value;
			return styleBuilder;
		}

		/// <summary>
		/// Sets the opacity level to use when rendering images in the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to control the transparency of images rendered in the PDF output. Setting an
		/// opacity less than 1.0 will make images partially transparent.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that provides the opacity value for images. The value should be between 0.0 (fully transparent) and 1.0
		/// (fully opaque).</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseImageOpacity<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<float, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ImageOpacity = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the image opacity for the PDF style using the specified binding.
		/// </summary>
		/// <remarks>Setting the image opacity affects the transparency of images rendered in the PDF output. This
		/// method enables fluent configuration of image appearance.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A binding that specifies the opacity value for images. The value should be between 0.0 (fully transparent) and 1.0
		/// (fully opaque).</param>
		/// <returns>The same style builder instance, enabling method chaining.</returns>
		public static IStyleBuilder<TModel> UseImageOpacity<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<float, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ImageOpacity = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the image scale factor to be used when rendering images in the PDF output.
		/// </summary>
		/// <remarks>Use this method to control the scaling of images in the generated PDF. The scale factor is
		/// typically a value greater than 0, where 1.0 represents the original size. Setting an appropriate scale can help
		/// fit images within page boundaries or achieve desired visual effects.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A delegate that provides the image scale factor for the model. The value determines how images are scaled during
		/// rendering.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance so that additional configuration can be chained.</returns>
		public static IStyleBuilder<TModel> UseImageScale<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<float, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ImageScale = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures the image scale factor to be used when rendering images in the PDF style builder.
		/// </summary>
		/// <remarks>Use this method to specify a custom scaling factor for images in the generated PDF. This is
		/// useful for adjusting image sizes relative to other content. The provided value can be data-bound to model
		/// properties for dynamic scaling.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the style is applied. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance to configure.</param>
		/// <param name="value">A bindable property representing the image scale factor. The value determines how images are scaled when rendered.</param>
		/// <returns>The same <see cref="IStyleBuilder{TModel}"/> instance so that additional configuration can be chained.</returns>
		public static IStyleBuilder<TModel> UseImageScale<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<float, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ImageScale = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures whether drawing operations should be clipped to the current path for the specified style builder.
		/// </summary>
		/// <remarks>When clip drawing is enabled, subsequent drawing operations are restricted to the current
		/// clipping path. This can be used to control rendering within specific regions of the PDF.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder to configure.</param>
		/// <param name="value">An action that binds a Boolean value indicating whether to enable clip drawing for the model.</param>
		/// <returns>The same style builder instance with clip drawing configuration applied.</returns>
		public static IStyleBuilder<TModel> UseClipDrawing<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<bool, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ClipDrawing = value;
			return styleBuilder;
		}

		/// <summary>
		/// Configures whether drawing operations should be clipped according to the specified binding for the style builder.
		/// </summary>
		/// <remarks>Use this method to enable or disable clipping of drawing operations based on a model property.
		/// This is useful when conditional rendering is required in PDF generation scenarios.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the style builder.</typeparam>
		/// <param name="styleBuilder">The style builder to configure.</param>
		/// <param name="value">A binding that determines whether drawing operations are clipped. The value is applied to the style builder.</param>
		/// <returns>The same style builder instance with the clip drawing setting applied.</returns>
		public static IStyleBuilder<TModel> UseClipDrawing<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<bool, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ClipDrawing = value;
			return styleBuilder;
		}



		/// <summary>
		/// Creates a strongly-typed PDF style from the specified style builder.
		/// </summary>
		/// <remarks>Use this method to convert an <see cref="IStyleBuilder{TModel}"/> into a reusable PDF style for
		/// rendering or formatting operations.</remarks>
		/// <typeparam name="TModel">The type of the PDF model for which the style is built. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="styleBuilder">The style builder instance used to construct the PDF style. Cannot be null.</param>
		/// <returns>A <see cref="PdfStyle{TModel}"/> representing the style configuration for the specified model type.</returns>
		public static PdfStyle<TModel> Build<TModel>(this IStyleBuilder<TModel> styleBuilder)
			where TModel : IPdfModel
		{
			return (PdfStyle<TModel>)styleBuilder;
		}
	}
}
