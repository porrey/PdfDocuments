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
using System.Linq.Expressions;
using PdfDocuments.Exceptions;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for the IPdfSection<![CDATA[<TModel>]]> interface to support margin application and style resolution
	/// in PDF generation scenarios.
	/// </summary>
	/// <remarks>These extension methods simplify common operations when working with PDF sections, such as
	/// adjusting bounds for margins and resolving styles based on section configuration. Intended for use with
	/// implementations of IPdfSection<![CDATA[<TModel>]]> in PDF rendering workflows.</remarks>
	public static class PdfSectionExtensions
	{
		/// <summary>
		/// Assigns the specified style manager to the PDF section and returns the updated section.
		/// </summary>
		/// <remarks>This method enables fluent configuration of a PDF section's style manager. The section's existing
		/// style manager, if any, will be replaced.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the style manager will be assigned. Cannot be null.</param>
		/// <param name="styleManager">The style manager to assign to the section. Cannot be null.</param>
		/// <returns>The same PDF section instance with the specified style manager assigned.</returns>
		public static IPdfSection<TModel> WithStyleManager<TModel>(this IPdfSection<TModel> section, IPdfStyleManager<TModel> styleManager)
			where TModel : IPdfModel
		{
			section.StyleManager = styleManager;
			return section;
		}

		/// <summary>
		/// Sets the section's key to a string derived from its type name, excluding common suffixes.
		/// </summary>
		/// <remarks>This method removes the 'Pdf', 'Section', and generic type suffixes from the section's type name
		/// to generate a concise key. The key is intended to uniquely identify the section based on its type.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <param name="section">The PDF section whose key will be set. Cannot be null.</param>
		public static void SetKey<TModel>(this IPdfSection<TModel> section)
			where TModel : IPdfModel
		{
			section.Key = section.GetType().Name.Replace("Pdf", "").Replace("Section", "").Replace("`1", "");
		}

		/// <summary>
		/// Calculates the bounds of a PDF section after applying the specified margins.
		/// </summary>
		/// <typeparam name="TModel">The type of the model used by the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section whose bounds will be adjusted.</param>
		/// <param name="g">The PDF grid page context used for calculating the bounds.</param>
		/// <param name="m">The model instance associated with the section.</param>
		/// <param name="margin">The spacing values to apply as margins to the section's bounds.</param>
		/// <returns>A PdfBounds instance representing the section's bounds after margins have been applied.</returns>
		public static PdfBounds ApplyPadding<TModel>(this IPdfSection<TModel> section, PdfGridPage g, TModel m, PdfSpacing margin)
			where TModel : IPdfModel
		{
			return section.RenderArea.SubtractSpacing(g, m, margin);
		}

		/// <summary>
		/// Applies the specified margin to the section's bounds for the given model and PDF page.
		/// </summary>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the margin will be applied.</param>
		/// <param name="g">The PDF grid page that provides context for calculating the bounds.</param>
		/// <param name="m">The model instance used to determine the section's bounds.</param>
		/// <param name="margin">The margin to apply to the section's bounds.</param>
		/// <returns>A <see cref="PdfBounds"/> representing the section's bounds after applying the specified margin.</returns>
		public static PdfBounds ApplyMargin<TModel>(this IPdfSection<TModel> section, PdfGridPage g, TModel m, PdfSpacing margin)
			where TModel : IPdfModel
		{
			return section.RenderArea.AddSpacing(g, m, margin);
		}

		/// <summary>
		/// Resolves and returns the style associated with the specified index for the given PDF section.
		/// </summary>
		/// <remarks>If the section contains no style names, the default style is returned. If the index exceeds the
		/// available style names, the first style is used.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section for which to resolve the style. Cannot be null.</param>
		/// <param name="index">The zero-based index of the style to resolve. If the index is out of range, the first style is used.</param>
		/// <returns>A <see cref="PdfStyle{TModel}"/> instance corresponding to the specified style index, or the default style if no
		/// styles are defined.</returns>
		public static PdfStyle<TModel> ResolveStyle<TModel>(this IPdfSection<TModel> section, int index)
			where TModel : IPdfModel
		{
			PdfStyle<TModel> returnValue = null;

			if (section.StyleManager != null)
			{
				if (section.StyleOverridden && section.Style != null)
				{
					returnValue = section.Style;
				}
				else
				{
					if (section.StyleNames.Any())
					{
						if (index < section.StyleNames.Count())
						{
							string styleName = section.StyleNames.ElementAt(index);
							returnValue = section.StyleManager.GetStyle(styleName);
						}
						else
						{
							string styleName = section.StyleNames.First();
							returnValue = section.StyleManager.GetStyle(styleName);
						}
					}
					else
					{
						section.StyleManager.GetStyle(PdfStyleManager<TModel>.Default);
					}
				}
			}
			else
			{
				throw new MissingStyleManagerException();
			}

			return returnValue;
		}

		/// <summary>
		/// Adds a data column to the PDF section with the specified header, binding expression, relative width, format, and
		/// style names.
		/// </summary>
		/// <remarks>If the section does not support data columns, no column is added and the section is returned
		/// unchanged. This method is intended for use with sections implementing PdfDataGridSection.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <typeparam name="TItem">The type of the data item represented in the section.</typeparam>
		/// <typeparam name="TProperty">The type of the property to bind for the column's cell values.</typeparam>
		/// <param name="section">The PDF section to which the column will be added.</param>
		/// <param name="columnHeader">A bindable property representing the column header text.</param>
		/// <param name="expression">An expression that specifies the property of TItem to bind as the column's cell value.</param>
		/// <param name="relativeWidth">A bindable property specifying the relative width of the column within the section.</param>
		/// <param name="format">A bindable property specifying the format string to apply to the column's cell values.</param>
		/// <param name="headerStyleName">A bindable property specifying the style name to apply to the column header.</param>
		/// <param name="cellStyleName">A bindable property specifying the style name to apply to the column's cells.</param>
		/// <returns>The PDF section instance with the new column added. Returns the original section if it does not support data
		/// columns.</returns>
		public static IPdfSection<TModel> AddColumn<TModel, TItem, TProperty>(this IPdfSection<TModel> section, BindProperty<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindProperty<double, TModel> relativeWidth, BindProperty<string, TModel> format, BindProperty<string, TModel> headerStyleName, BindProperty<string, TModel> cellStyleName)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s1)
			{
				s1.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
			}
			else if (section is PdfDataRowsSection<TModel, TItem> s2)
			{
				s2.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
			}

			return section;
		}

		/// <summary>
		/// Adds a data column to the PDF section with customizable header, binding, width, format, and styles.
		/// </summary>
		/// <remarks>This method is intended for use with sections that support data grid columns, such as
		/// PdfDataGridSection. If the section does not support columns, it is returned unchanged.</remarks>
		/// <typeparam name="TModel">The type of the model used for binding section data. Must implement IPdfModel.</typeparam>
		/// <typeparam name="TItem">The type of the item represented in each row of the data grid.</typeparam>
		/// <typeparam name="TProperty">The type of the property to bind for the column's cell values.</typeparam>
		/// <param name="section">The PDF section to which the column will be added. Cannot be null.</param>
		/// <param name="columnHeader">A binding action that provides the column header text based on the model.</param>
		/// <param name="expression">An expression that specifies the property of TItem to bind as the column's cell value.</param>
		/// <param name="relativeWidth">A binding action that determines the relative width of the column based on the model.</param>
		/// <param name="format">A binding action that specifies the format string for the column's cell values.</param>
		/// <param name="headerStyleName">A binding action that provides the style name for the column header based on the model.</param>
		/// <param name="cellStyleName">A binding action that provides the style name for the column cells based on the model.</param>
		/// <returns>The PDF section instance with the new column added. Returns the original section if it does not support data
		/// columns.</returns>
		public static IPdfSection<TModel> AddColumn<TModel, TItem, TProperty>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindPropertyAction<double, TModel> relativeWidth, BindPropertyAction<string, TModel> format, BindPropertyAction<string, TModel> headerStyleName, BindPropertyAction<string, TModel> cellStyleName)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s1)
			{
				s1.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
			}
			else if (section is PdfDataRowsSection<TModel, TItem> s2)
			{
				s2.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
			}

			return section;
		}

		/// <summary>
		/// Assigns a collection of items to the section for use in data binding scenarios.
		/// </summary>
		/// <remarks>If the section is a data grid section, the items will be set for data binding. Otherwise, the
		/// section remains unchanged.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <typeparam name="TItem">The type of items to be bound to the section.</typeparam>
		/// <param name="section">The PDF section to which the items will be assigned.</param>
		/// <param name="items">An action that binds a collection of items to the section's model.</param>
		/// <returns>The original section instance, allowing for method chaining.</returns>
		public static IPdfSection<TModel> UseItems<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<IEnumerable<TItem>, TModel> items)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s1)
			{
				s1.Items = items;
			}
			else if (section is PdfDataRowsSection<TModel, TItem> s2)
			{
				s2.Items = items;
			}

			return section;
		}

		/// <summary>
		/// Sets the key for the specified PDF section and returns the updated section.
		/// </summary>
		/// <remarks>This method enables fluent configuration of PDF sections by allowing key assignment in a chained
		/// manner.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section whose key will be set. Cannot be null.</param>
		/// <param name="value">The key value to assign to the section. Cannot be null.</param>
		/// <returns>The same PDF section instance with its key property updated to the specified value.</returns>
		public static IPdfSection<TModel> WithKey<TModel>(this IPdfSection<TModel> section, string value)
			where TModel : IPdfModel
		{
			section.Key = value;
			return section;
		}

		/// <summary>
		/// Updates the key of each child section to reflect its hierarchical path within the parent section.
		/// </summary>
		/// <remarks>This method recursively updates the keys of all child sections to include their parent section's
		/// key, forming a dot-separated path. Use this method to ensure that each section's key uniquely identifies its
		/// position within the section hierarchy.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <param name="section">The PDF section whose child keys will be updated to include their hierarchical path. Cannot be null.</param>
		/// <returns>The original section with updated child keys reflecting their hierarchical paths.</returns>
		public static IPdfSection<TModel> ApplyKeyPath<TModel>(this IPdfSection<TModel> section)
			where TModel : IPdfModel
		{
			foreach (IPdfSection<TModel> childSection in section.Children)
			{
				childSection.Key = $"{section.Key}.{childSection.Key}";
				childSection.ApplyKeyPath();
			}

			return section;
		}

		/// <summary>
		/// Adds a watermark image to the PDF section using the specified image path binding.
		/// </summary>
		/// <remarks>Use this method to dynamically set a watermark image for a PDF section. The watermark image path
		/// is determined by the provided binding and can vary based on the model's state.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the watermark image will be applied.</param>
		/// <param name="value">A binding that provides the file path of the watermark image based on the model.</param>
		/// <returns>The PDF section instance with the watermark image applied.</returns>
		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		/// <summary>
		/// Adds a watermark image to the PDF section using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to dynamically set a watermark image for a PDF section based on model data. The
		/// watermark image path is determined by the provided binding action.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the watermark will be applied.</param>
		/// <param name="value">A binding action that provides the file path of the watermark image based on the model.</param>
		/// <returns>The PDF section instance with the watermark image applied.</returns>
		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		/// <summary>
		/// Adds a text binding to the specified PDF section.
		/// </summary>
		/// <remarks>This method enables fluent configuration of PDF sections by allowing text content to be bound to
		/// a model property. The returned section can be further configured or used in PDF generation workflows.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the text binding will be applied.</param>
		/// <param name="value">The binding representing the text content for the section. Cannot be null.</param>
		/// <returns>The PDF section instance with the text binding applied.</returns>
		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			section.Text = value;
			return section;
		}

		/// <summary>
		/// Sets the text content for the PDF section using the specified binding action.
		/// </summary>
		/// <remarks>Use this method to bind dynamic or model-driven text to a PDF section. The binding action allows
		/// the text to be determined at runtime based on the model's state.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the text content will be applied.</param>
		/// <param name="value">A binding action that provides the text value for the section based on the model.</param>
		/// <returns>The PDF section instance with the updated text content.</returns>
		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			section.Text = value;
			return section;
		}

		/// <summary>
		/// Sets the title of a PDF section using the specified binding value.
		/// </summary>
		/// <remarks>If the section is a page header, its title is updated with the provided value. For other section
		/// types, this method has no effect.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the title will be applied.</param>
		/// <param name="value">The binding property representing the title value to set for the section.</param>
		/// <returns>The original PDF section instance with the title set if applicable.</returns>
		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Title = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the title for a PDF section using the specified binding action.
		/// </summary>
		/// <remarks>If the section is a page header, the title is set using the provided binding action. For other
		/// section types, this method has no effect.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the title will be applied.</param>
		/// <param name="value">A binding action that provides the title value for the section based on the model.</param>
		/// <returns>The original PDF section instance with the title set if applicable.</returns>
		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Title = value;
			}

			return section;
		}

		/// <summary>
		/// Adds a logo binding to a PDF section if it is a page header section.
		/// </summary>
		/// <remarks>If the section is not a page header, this method has no effect. Use this extension to add a logo
		/// to header sections in generated PDF documents.</remarks>
		/// <typeparam name="TModel">The type of the model used in the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the logo binding will be applied. If the section is a page header, the logo will be set.</param>
		/// <param name="value">The binding representing the logo image source for the section. Cannot be null.</param>
		/// <returns>The original PDF section instance, with the logo binding applied if applicable.</returns>
		public static IPdfSection<TModel> WithLogo<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Logo = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the image property of the section if it is an image section.
		/// </summary>
		/// <remarks>This method only modifies sections that are image sections. Other section types are
		/// unaffected.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to modify. If the section is an image section, its image property will be set.</param>
		/// <param name="value">The binding representing the image source to assign to the section.</param>
		/// <returns>The original section instance, with the image property set if applicable.</returns>
		public static IPdfSection<TModel> WithImage<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfImageSection<TModel> imageSection)
			{
				imageSection.Image = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the signature options for a PDF section if it supports signatures.
		/// </summary>
		/// <remarks>If the section does not support signatures, this method has no effect. Use this method to
		/// configure signature-related options for sections that implement signature functionality.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which signature options may be applied.</param>
		/// <param name="value">The binding property containing signature options to set for the section. Can be null if no options are to be
		/// applied.</param>
		/// <returns>The original PDF section instance, with signature options set if applicable.</returns>
		public static IPdfSection<TModel> WithOptions<TModel>(this IPdfSection<TModel> section, BindProperty<SignatureOptions<TModel>, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfSignatureSection<TModel> imageSection)
			{
				imageSection.SignatureOptions = value;
			}

			return section;
		}

		/// <summary>
		/// Assigns a logo to the page header section of a PDF document using the specified binding action.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageHeaderSection<![CDATA[<TModel>]]>. Other section types are
		/// returned unchanged. Use this method to add branding or identification to PDF headers.</remarks>
		/// <typeparam name="TModel">The type of the model used for PDF section binding. Must implement the IPdfModel interface.</typeparam>
		/// <param name="section">The PDF section to which the logo will be assigned. If the section is a page header, the logo will be set;
		/// otherwise, the section remains unchanged.</param>
		/// <param name="value">A binding action that provides the logo value for the section. The action defines how the logo string is obtained
		/// from the model.</param>
		/// <returns>The original PDF section instance, with the logo assigned if applicable.</returns>
		public static IPdfSection<TModel> WithLogo<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Logo = value;
			}

			return section;
		}

		/// <summary>
		/// Assigns the specified parent section to the given PDF section and returns the updated section.
		/// </summary>
		/// <remarks>Use this method to establish or update the parent-child relationship between PDF sections. The
		/// method modifies the ParentSection property of the provided section.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the parent section will be assigned.</param>
		/// <param name="value">The parent section to assign to the specified PDF section. Can be null if the section has no parent.</param>
		/// <returns>The PDF section with its parent section set to the specified value.</returns>
		public static IPdfSection<TModel> AssignParentSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			section.ParentSection = value;
			return section;
		}

		/// <summary>
		/// Adds a child section to the specified parent section.
		/// </summary>
		/// <remarks>The child section's ParentSection property is set to the specified parent section. The method
		/// does nothing if the child section is null.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The parent PDF section to which the child section will be added.</param>
		/// <param name="value">The child PDF section to add. If null, no section is added.</param>
		/// <returns>The parent section after the child section has been added.</returns>
		public static IPdfSection<TModel> AddChildSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			if (value != null)
			{
				value.ParentSection = section;
				section.Children.Add(value);
			}

			return section;
		}

		/// <summary>
		/// Adds the specified child sections to the given PDF section.
		/// </summary>
		/// <remarks>Each child section's <c>ParentSection</c> property is set to the provided section. The method
		/// does not modify the collection if <paramref name="values"/> is <see langword="null"/>.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the child sections will be added.</param>
		/// <param name="values">The collection of child sections to add. If <see langword="null"/>, no sections are added.</param>
		/// <returns>The original PDF section with the specified child sections added.</returns>
		public static IPdfSection<TModel> AddChildren<TModel>(this IPdfSection<TModel> section, IEnumerable<IPdfSection<TModel>> values)
			where TModel : IPdfModel
		{
			if (values != null)
			{
				foreach (IPdfSection<TModel> value in values)
				{
					value.ParentSection = section;
					section.Children.Add(value);
				}
			}

			return section;
		}

		/// <summary>
		/// Adds the specified child sections to the current section.
		/// </summary>
		/// <remarks>Each child section's <see cref="IPdfSection{TModel}.ParentSection"/> property is set to the
		/// specified parent section. The method supports adding multiple child sections in a single call.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The parent section to which the child sections will be added.</param>
		/// <param name="values">The child sections to add to the parent section. Can be empty.</param>
		/// <returns>The parent section with the added child sections.</returns>
		public static IPdfSection<TModel> AddChildren<TModel>(this IPdfSection<TModel> section, params IPdfSection<TModel>[] values)
			where TModel : IPdfModel
		{
			if (values != null)
			{
				foreach (IPdfSection<TModel> value in values)
				{
					value.ParentSection = section;
					section.Children.Add(value);
				}
			}

			return section;
		}

		/// <summary>
		/// Returns the parent section of the specified PDF section.
		/// </summary>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section for which to retrieve the parent section. Cannot be null.</param>
		/// <returns>The parent section of the specified PDF section, or null if the section has no parent.</returns>
		public static IPdfSection<TModel> WithParent<TModel>(this IPdfSection<TModel> section)
			where TModel : IPdfModel
		{
			return section.ParentSection;
		}

		/// <summary>
		/// Sets a render condition for the PDF section, determining whether the section should be rendered based on the
		/// provided binding action.
		/// </summary>
		/// <remarks>Use this method to control the visibility of a PDF section dynamically based on model data. The
		/// render condition is evaluated each time the section is rendered.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the render condition will be applied.</param>
		/// <param name="value">A binding action that evaluates to a Boolean value indicating whether the section should be rendered for the given
		/// model.</param>
		/// <returns>The PDF section with the specified render condition applied.</returns>
		public static IPdfSection<TModel> WithRenderCondition<TModel>(this IPdfSection<TModel> section, BindPropertyAction<bool, TModel> value)
			where TModel : IPdfModel
		{
			section.ShouldRender = value;
			return section;
		}

		/// <summary>
		/// Adds the specified content section as a child to the current PDF section.
		/// </summary>
		/// <remarks>Use this method to compose complex PDF documents by nesting sections. The method modifies the
		/// section by adding the specified child, and returns the same section instance for fluent chaining.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the content section will be added. Cannot be null.</param>
		/// <param name="value">The content section to add as a child. Cannot be null.</param>
		/// <returns>The original PDF section with the content section added as a child.</returns>
		public static IPdfSection<TModel> WithContentSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			section.AddChildSection(value);
			return section;
		}

		/// <summary>
		/// Sets the text displayed in the top-left corner of a PDF section, typically used for page footers.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// not modified.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the top-left text will be applied.</param>
		/// <param name="value">A bindable property representing the text to display in the top-left corner. Cannot be null.</param>
		/// <returns>The original PDF section instance with the top-left text set.</returns>
		public static IPdfSection<TModel> WithTopLeftText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopLeftText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the top-left corner of a PDF section, typically used for page footers.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// not modified.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the top-left text will be applied.</param>
		/// <param name="value">A binding action that provides the text to display in the top-left corner, based on the model.</param>
		/// <returns>The original PDF section instance with the top-left text configured.</returns>
		public static IPdfSection<TModel> WithTopLeftText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopLeftText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the top-right corner of a PDF section, typically used for page footers.
		/// </summary>
		/// <remarks>If the section is a page footer, the top-right text will be updated. For other section types,
		/// this method has no effect.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the top-right text will be applied.</param>
		/// <param name="value">A bindable property representing the text to display in the top-right corner. Cannot be null.</param>
		/// <returns>The original PDF section instance with the top-right text set.</returns>
		public static IPdfSection<TModel> WithTopRightText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopRightText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the top-right corner of a PDF section, typically used for page footers.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// not modified.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement the IPdfModel interface.</typeparam>
		/// <param name="section">The PDF section to which the top-right text will be applied.</param>
		/// <param name="value">A delegate that binds the top-right text value to the section's model.</param>
		/// <returns>The original PDF section instance with the top-right text configured.</returns>
		public static IPdfSection<TModel> WithTopRightText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopRightText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the bottom-left corner of a PDF footer section.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// returned unchanged.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the bottom-left text will be applied.</param>
		/// <param name="value">A bindable property representing the text to display in the bottom-left corner. Cannot be null.</param>
		/// <returns>The original PDF section instance with the bottom-left text set if applicable.</returns>
		public static IPdfSection<TModel> WithBottomLeftText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomLeftText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the bottom-left corner of a PDF section footer.
		/// </summary>
		/// <remarks>This method only affects sections of type <see cref="PdfPageFooterSection{TModel}"/>. Other
		/// section types are not modified.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the bottom-left text will be applied.</param>
		/// <param name="value">A binding action that provides the text to display in the bottom-left corner, based on the model.</param>
		/// <returns>The original PDF section instance with the bottom-left text binding applied.</returns>
		public static IPdfSection<TModel> WithBottomLeftText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomLeftText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the bottom-right corner of a PDF footer section.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// unchanged.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the bottom-right text will be applied. If the section is not a footer, this method has no
		/// effect.</param>
		/// <param name="value">A bindable property representing the text to display in the bottom-right corner of the footer.</param>
		/// <returns>The original PDF section instance, allowing for fluent configuration.</returns>
		public static IPdfSection<TModel> WithBottomRightText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomRightText = value;
			}

			return section;
		}

		/// <summary>
		/// Sets the text displayed in the bottom-right corner of a PDF section footer.
		/// </summary>
		/// <remarks>This method only affects sections of type PdfPageFooterSection<![CDATA[<TModel>]]>. Other section types are
		/// not modified.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which the bottom-right text will be applied.</param>
		/// <param name="value">A delegate that binds the bottom-right text value to the model. Cannot be null.</param>
		/// <returns>The original PDF section instance with the bottom-right text configured.</returns>
		public static IPdfSection<TModel> WithBottomRightText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomRightText = value;
			}

			return section;
		}

		/// <summary>
		/// Configures the section to display multiple stacked text items using the specified bindings.
		/// </summary>
		/// <remarks>If the section supports stacked text items, the specified bindings are applied. Otherwise, the
		/// section remains unchanged.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to configure with stacked items.</param>
		/// <param name="values">An array of property bindings that define the text items to be stacked in the section.</param>
		/// <returns>The original section instance, configured with the specified stacked items.</returns>
		public static IPdfSection<TModel> WithStackedItems<TModel>(this IPdfSection<TModel> section, params BindProperty<string, TModel>[] values)
			where TModel : IPdfModel
		{
			if (section is PdfStackedTextSection<TModel> stackedSection)
			{
				stackedSection.StackedItems = values;
			}

			return section;
		}

		/// <summary>
		/// Adds stacked text items to a PDF section by assigning the specified binding actions.
		/// </summary>
		/// <remarks>If the section is not a stacked text section, the method has no effect. This method is intended
		/// for use with sections that support stacked text items.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section to which stacked items will be added.</param>
		/// <param name="values">An array of binding actions representing the stacked text items to assign to the section.</param>
		/// <returns>The original PDF section instance with stacked items assigned if applicable.</returns>
		public static IPdfSection<TModel> WithStackedItems<TModel>(this IPdfSection<TModel> section, params BindPropertyAction<string, TModel>[] values)
			where TModel : IPdfModel
		{
			if (section is PdfStackedTextSection<TModel> stackedSection)
			{
				IList<BindProperty<string, TModel>> items = new List<BindProperty<string, TModel>>();

				foreach (BindPropertyAction<string, TModel> value in values)
				{
					items.Add(value);
				}

				stackedSection.StackedItems = items;
			}

			return section;
		}

		/// <summary>
		/// Assigns one or more style names to the specified PDF section and returns the updated section.
		/// </summary>
		/// <remarks>This method enables fluent configuration of section styles. The section's existing styles will be
		/// replaced by the provided style names.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section to which the styles will be applied.</param>
		/// <param name="styleNames">An array of style names to assign to the section. Each name represents a style to be applied.</param>
		/// <returns>The same PDF section instance with the specified styles assigned.</returns>
		public static IPdfSection<TModel> WithStyles<TModel>(this IPdfSection<TModel> section, params string[] styleNames)
			where TModel : IPdfModel
		{
			section.StyleNames = styleNames;
			return section;
		}

		/// <summary>
		/// Sets a fixed height for the PDF section using the specified binding.
		/// </summary>
		/// <remarks>This method overrides any previously set height styles for the section. Use this method when a
		/// section must have a constant height regardless of its content.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the section.</typeparam>
		/// <param name="section">The PDF section to configure.</param>
		/// <param name="value">A binding that provides the fixed height value, in points, for the section. May be null to clear the fixed height.</param>
		/// <returns>The same PDF section instance with the fixed height style applied.</returns>
		public static IPdfSection<TModel> WithFixedHeight<TModel>(this IPdfSection<TModel> section, BindProperty<int?, TModel> value)
			where TModel : IPdfModel
		{
			section.StyleOverridden = true;
			section.Style.FixedHeight = value;
			return section;
		}

		/// <summary>
		/// Sets a fixed height for the section using the specified binding action.
		/// </summary>
		/// <remarks>This method overrides any previously set height style for the section. Use this method when a
		/// section must maintain a constant height regardless of its content.</remarks>
		/// <typeparam name="TModel">The type of the PDF model associated with the section.</typeparam>
		/// <param name="section">The PDF section to configure.</param>
		/// <param name="value">A binding action that provides the fixed height, in points, for the section. The value can be null to indicate no
		/// fixed height.</param>
		/// <returns>The same section instance with the fixed height style applied.</returns>
		public static IPdfSection<TModel> WithFixedHeight<TModel>(this IPdfSection<TModel> section, BindPropertyAction<int?, TModel> value)
			where TModel : IPdfModel
		{
			section.StyleOverridden = true;
			section.Style.FixedHeight = value;
			return section;
		}
	}
}