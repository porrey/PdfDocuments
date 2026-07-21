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
using System.Reflection;

namespace PdfDocuments
{
	/// <summary>
	/// Represents a section in a PDF document that displays a data grid with configurable columns and items, supporting
	/// dynamic binding and formatting.
	/// </summary>
	/// <remarks>Use this class to define tabular data layouts within a PDF section. Columns and items can be bound
	/// to model properties, and column formatting, styles, and widths are customizable. Supports both static and dynamic
	/// binding scenarios for headers, cell values, and styles.</remarks>
	/// <typeparam name="TModel">The type of the PDF model used for binding and rendering section content.</typeparam>
	/// <typeparam name="TItem">The type of the items displayed in the data grid rows.</typeparam>
	public class PdfDataGridSection<TModel, TItem> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Gets the collection of data columns used to define the structure and content of the grid.
		/// </summary>
		/// <remarks>Each column in the collection represents a field or property from the model type and controls how
		/// data is displayed in the grid. The collection is read-only; to modify the columns, add or remove items from the
		/// existing list.</remarks>
		public virtual IList<PdfDataGridColumn<TModel>> DataColumns { get; } = [];

		/// <summary>
		/// Gets or sets the collection of items to be bound to the model.
		/// </summary>
		/// <remarks>Use this property to provide the set of items that will be displayed or processed in the context
		/// of the model. The property supports binding scenarios where the items are dynamically updated or retrieved from
		/// external sources.</remarks>
		public virtual BindProperty<IEnumerable<TItem>, TModel> Items { get; set; } = Array.Empty<TItem>();

		/// <summary>
		/// Adds a new data column to the grid with the specified header, binding expression, relative width, format, and
		/// style settings.
		/// </summary>
		/// <remarks>The column is added to the grid's collection of data columns. Use this method to configure column
		/// appearance and data binding in a flexible manner.</remarks>
		/// <typeparam name="TProperty">The type of the property to bind to the column's data cells.</typeparam>
		/// <param name="columnHeader">A bindable property representing the column header text. The value is displayed as the column's header.</param>
		/// <param name="expression">An expression that identifies the property of the data item to bind to the column's cells.</param>
		/// <param name="relativeWidth">A bindable property specifying the relative width of the column. The value determines the column's proportional
		/// size within the grid.</param>
		/// <param name="format">A bindable property specifying the string format to apply to the column's cell values. The format is used when
		/// rendering cell content.</param>
		/// <param name="headerStyleName">A bindable property specifying the style name to apply to the column header. The style controls the appearance of
		/// the header cell.</param>
		/// <param name="cellStyleName">A bindable property specifying the style name to apply to the column's data cells. The style controls the
		/// appearance of the cell content.</param>
		/// <returns>A PdfDataGridColumn<![CDATA[<TModel>]]>; instance representing the newly added data column.</returns>
		public virtual PdfDataGridColumn<TModel> AddDataColumn<TProperty>(BindProperty<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindProperty<double, TModel> relativeWidth, BindProperty<string, TModel> format, BindProperty<string, TModel> headerStyleName, BindProperty<string, TModel> cellStyleName)
		{
			PdfDataGridColumn<TModel> column = new()
			{
				HeaderStyleName = headerStyleName,
				DataStyleName = cellStyleName,
				ColumnHeader = columnHeader,
				MemberExpression = expression.Body as MemberExpression,
				StringFormat = format
			};

			this.DataColumns.Add(column);
			return column;
		}

		/// <summary>
		/// Adds a new data column to the grid with the specified header, binding expression, relative width, format, and
		/// style settings.
		/// </summary>
		/// <remarks>Use this method to dynamically configure columns in a data grid, including header text, binding,
		/// formatting, and styling. The delegates allow customization based on the model, enabling flexible column
		/// definitions.</remarks>
		/// <typeparam name="TProperty">The type of the property to bind to the column's data cells.</typeparam>
		/// <param name="columnHeader">A delegate that provides the column header text based on the model.</param>
		/// <param name="expression">An expression that identifies the property of the item to bind as the column's data source.</param>
		/// <param name="relativeWidth">A delegate that determines the relative width of the column based on the model.</param>
		/// <param name="format">A delegate that specifies the string format for displaying cell values in the column.</param>
		/// <param name="headerStyleName">A delegate that provides the style name to apply to the column header based on the model.</param>
		/// <param name="cellStyleName">A delegate that provides the style name to apply to the column's data cells based on the model.</param>
		/// <returns>A PdfDataGridColumn<![CDATA[<TModel>]]>; instance representing the newly added data column.</returns>
		public virtual PdfDataGridColumn<TModel> AddDataColumn<TProperty>(BindPropertyAction<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindPropertyAction<double, TModel> relativeWidth, BindPropertyAction<string, TModel> format, BindPropertyAction<string, TModel> headerStyleName, BindPropertyAction<string, TModel> cellStyleName)
		{
			PdfDataGridColumn<TModel> column = new()
			{
				HeaderStyleName = headerStyleName,
				DataStyleName = cellStyleName,
				ColumnHeader = columnHeader,
				MemberExpression = expression.Body as MemberExpression,
				StringFormat = format
			};

			this.DataColumns.Add(column);
			return column;
		}

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
				List<IPdfSection<TModel>> innerItems = [];

				int i = 0;
				string controlStyle = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : PdfStyleManager<TModel>.Default;

				this.StyleNames = [controlStyle];

				//
				// Add the column headers.
				//
				innerItems.Add(Pdf.HorizontalStackSection(
					[.. (from tbl in this.DataColumns
					     select Pdf.TextBlockSection<TModel>()
							.WithStyles(tbl.HeaderStyleName.Resolve(g, m))
							.WithText(tbl.ColumnHeader.Resolve(g, m))
					)]
				));
				
				//
				// Get the data items.
				//
				IEnumerable<TItem> items = this.Items.Resolve(g, m);

				//
				// Add the data rows.
				//
				foreach (TItem item in items)
				{
					//
					// Create a horizontal stack.
					//
					IPdfSection<TModel> horizontalStack = Pdf.HorizontalStackSection<TModel>();

					foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
					{
						//
						// Add a text block for each cell value.
						//
						horizontalStack.AddChildren(Pdf.TextBlockSection<TModel>()
							.WithStyles(column.DataStyleName.Resolve(g, m))
							.WithText(this.FormattedValue(g, m, column, item))
						);
					}

					//
					// Add the horizontal stack to the inner items.
					//
					innerItems.Add(horizontalStack);
				}

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

		/// <summary>
		/// Returns the formatted string representation of the specified property value for a grid cell.
		/// </summary>
		/// <remarks>If a string format is defined for the column, it is applied to the property value. Otherwise, the
		/// value is converted to a string using default formatting.</remarks>
		/// <param name="g">The grid page context used for formatting operations.</param>
		/// <param name="m">The model instance associated with the current grid row.</param>
		/// <param name="column">The column definition containing formatting and member information.</param>
		/// <param name="item">The data item from which the property value is retrieved.</param>
		/// <returns>A string containing the formatted value of the property for the specified grid cell.</returns>
		protected virtual string FormattedValue(PdfGridPage g, TModel m, PdfDataGridColumn<TModel> column, TItem item)
		{
			//
			// For the property value.
			//
			PropertyInfo property = column.MemberExpression.Member as PropertyInfo;
			object value = property.GetValue(item);
			return column.StringFormat != null ? string.Format(column.StringFormat.Resolve(g, m), value) : Convert.ToString(value);
		}
	}
}
