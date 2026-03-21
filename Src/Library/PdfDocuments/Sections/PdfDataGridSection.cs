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
				RelativeWidth = relativeWidth,
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
				RelativeWidth = relativeWidth,
				StringFormat = format
			};

			this.DataColumns.Add(column);
			return column;
		}

		/// <summary>
		/// Renders the grid page content asynchronously using the specified model and layout bounds.
		/// </summary>
		/// <remarks>This method renders both header and data rows based on the provided model and column definitions.
		/// The rendering respects the specified bounds and column widths. Override this method to customize rendering
		/// behavior for derived grid types.</remarks>
		/// <param name="g">The PDF grid page to render content onto.</param>
		/// <param name="m">The model instance providing data for rendering.</param>
		/// <param name="bounds">The layout bounds defining the area and columns available for rendering.</param>
		/// <returns>A task that represents the asynchronous rendering operation. The task result is <see langword="true"/> if
		/// rendering was successful; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Keep track of the current row.
			//
			int topRow = bounds.TopRow;

			//
			// Keep track of the current column.
			//
			int leftColumn = bounds.LeftColumn;

			//
			// Keep track of the current row height.
			//
			int currentRowHeight = 0;

			//
			// Determine the column widths.
			//
			double sum = this.DataColumns.Sum(t => t.RelativeWidth.Resolve(g, m));

			//
			// The total of the column widths must be less
			// than or equal to bounds.Columns
			//
			int[] columnWidth = [.. (from tbl in this.DataColumns
								 select (int)(bounds.Columns * (tbl.RelativeWidth.Resolve(g, m) / sum)))];

			//
			// Never under allocate the width.
			//
			if (columnWidth.Length > 0 && columnWidth.Sum() < bounds.Columns)
			{
				//
				// Allocate the missing width to the last column.
				//
				columnWidth[^1] += (bounds.Columns - columnWidth.Sum());
			}

			//
			// Render the headers.
			//
			int i = 0;

			foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
			{
				PdfTextElement<TModel> headerElement = new(column.ColumnHeader.Resolve(g, m));
				PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName.Resolve(g, m));
				PdfSize headerSize = headerElement.Measure(g, m, headerStyle);
				PdfBounds headerBounds = new PdfBounds(leftColumn, topRow, columnWidth[i], headerSize.Rows).SubtractBounds(g, m, headerStyle.Margin.Resolve(g, m));
				this.OnRenderHeaderColumn(g, m, headerBounds, headerStyle, headerElement);
				leftColumn += columnWidth[i];
				currentRowHeight = headerSize.Rows;
				i++;
			}

			//
			// Go to the next row.
			//
			topRow += currentRowHeight;

			//
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(g, m);

			if (items.Any())
			{
				//
				// Get the maximum row height so that all rows can be displayed at the same
				// height no matter what data they contain.
				//
				int rowHeight = 0;

				foreach (TItem item in items)
				{
					leftColumn = bounds.LeftColumn;
					int j = 0;

					foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
					{
						PdfTextElement<TModel> dataElement = new(this.FormattedValue(g, m, column, item));
						PdfStyle<TModel> dataStyle = this.StyleManager.GetStyle(column.DataStyleName.Resolve(g, m));
						PdfSize dataSize = dataElement.Measure(g, m, dataStyle);
						PdfBounds dataBounds = new PdfBounds(leftColumn, topRow, columnWidth[j], dataSize.Rows).SubtractBounds(g, m, dataStyle.Margin.Resolve(g, m));

						if (dataBounds.Rows > rowHeight)
						{
							rowHeight = dataBounds.Rows;
						}

						j++;
					}
				}

				//
				// Render the data.
				//
				foreach (TItem item in items)
				{
					leftColumn = bounds.LeftColumn;
					int k = 0;

					foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
					{
						PdfTextElement<TModel> dataElement = new(this.FormattedValue(g, m, column, item));
						PdfStyle<TModel> dataStyle = this.StyleManager.GetStyle(column.DataStyleName.Resolve(g, m));
						PdfSize dataSize = dataElement.Measure(g, m, dataStyle);
						PdfBounds dataBounds = new PdfBounds(leftColumn, topRow, columnWidth[k], dataSize.Rows).SubtractBounds(g, m, dataStyle.Margin.Resolve(g, m));
						this.OnRenderDataColumn(g, m, dataBounds, dataStyle, dataElement, item);
						leftColumn += columnWidth[k];
						currentRowHeight = dataSize.Rows;
						k++;
					}

					topRow += currentRowHeight;
				}
			}

			return Task.FromResult(returnValue);
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

		/// <summary>
		/// Renders a header column in the PDF grid using the specified model, bounds, style, and text element.
		/// </summary>
		/// <param name="g">The PDF grid page on which the header column will be rendered.</param>
		/// <param name="m">The data model instance used to provide content for the header column.</param>
		/// <param name="dataBounds">The bounds within which the header column will be rendered.</param>
		/// <param name="dataStyle">The style to apply when rendering the header column.</param>
		/// <param name="dataElement">The text element responsible for rendering the header column content.</param>
		protected virtual void OnRenderHeaderColumn(PdfGridPage g, TModel m, PdfBounds dataBounds, PdfStyle<TModel> dataStyle, PdfTextElement<TModel> dataElement)
		{
			dataElement.Render(g, m, dataBounds, dataStyle);
		}

		/// <summary>
		/// Renders a data column within the grid using the specified text element, style, and bounds.
		/// </summary>
		/// <param name="g">The grid page on which the data column will be rendered.</param>
		/// <param name="m">The model instance providing data context for rendering.</param>
		/// <param name="dataBounds">The bounds that define the area where the data column will be rendered.</param>
		/// <param name="dataStyle">The style to apply when rendering the data column.</param>
		/// <param name="dataElement">The text element responsible for rendering the content of the data column.</param>
		/// <param name="item">The item representing the data to be rendered in the column.</param>
		protected virtual void OnRenderDataColumn(PdfGridPage g, TModel m, PdfBounds dataBounds, PdfStyle<TModel> dataStyle, PdfTextElement<TModel> dataElement, TItem item)
		{
			dataElement.Render(g, m, dataBounds, dataStyle, item);
		}

		/// <summary>
		/// Asynchronously calculates the total height, in rows, required to render the grid page with its headers and data
		/// rows.
		/// </summary>
		/// <remarks>The calculated height accounts for the tallest header and data row in each column, multiplied by
		/// the number of data items. The result may be used to determine page layout or pagination when rendering the
		/// grid.</remarks>
		/// <param name="g">The PDF grid page context used for layout calculations.</param>
		/// <param name="m">The data model instance providing values for the grid.</param>
		/// <param name="bounds">The bounds within which the grid content should be measured.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the total height, in rows, needed to
		/// render the grid including headers and data rows.</returns>
		protected override Task<int> OnCalculateHeightAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			int returnValue = 0;

			//
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(g, m);

			//
			// Get the header height.
			//
			int headerHeight = 0;

			foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
			{
				PdfTextElement<TModel> headerElement = new(column.ColumnHeader.Resolve(g, m));
				PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName.Resolve(g, m));
				PdfSize headerSize = headerElement.Measure(g, m, headerStyle);
				PdfBounds headerBounds = new PdfBounds(0, 0, 100, headerSize.Rows).SubtractBounds(g, m, headerStyle.Margin.Resolve(g, m));
				headerBounds = headerBounds.AddBounds(g, m, headerStyle.Margin.Resolve(g, m));

				if (headerBounds.Rows > headerHeight)
				{
					headerHeight = headerBounds.Rows;
				}
			}

			//
			// Get the data row height.
			//
			int rowHeight = 0;
			TItem item = items.FirstOrDefault();

			if ((item != null))
			{
				foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
				{
					PdfTextElement<TModel> dataElement = new(this.FormattedValue(g, m, column, item));
					PdfStyle<TModel> dataStyle = this.StyleManager.GetStyle(column.DataStyleName.Resolve(g, m));
					PdfSize dataSize = dataElement.Measure(g, m, dataStyle);
					PdfBounds dataBounds = new PdfBounds(0, 0, 100, dataSize.Rows).SubtractBounds(g, m, dataStyle.Margin.Resolve(g, m));
					dataBounds = dataBounds.AddBounds(g, m, dataStyle.Margin.Resolve(g, m));

					if (dataBounds.Rows > rowHeight)
					{
						rowHeight = dataBounds.Rows;
					}
				}
			}

			//
			// Calculate the total height based on the header and data row heights.
			//
			returnValue = headerHeight + (rowHeight * items.Count());

			return Task.FromResult(returnValue);
		}
	}
}
