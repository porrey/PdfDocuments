/*
 *	MIT License
 *
 *	Copyright (c) 2021-2024 Daniel Porrey
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfDataGridSection<TModel, TItem> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public virtual IList<PdfDataGridColumn<TModel>> DataColumns { get; } = new List<PdfDataGridColumn<TModel>>();
		public virtual BindProperty<IEnumerable<TItem>, TModel> Items { get; set; } = new TItem[0];

		public virtual PdfDataGridColumn<TModel> AddDataColumn<TProperty>(BindProperty<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindProperty<double, TModel> relativeWidth, BindProperty<string, TModel> format, BindProperty<string, TModel> headerStyleName, BindProperty<string, TModel> cellStyleName)
		{
			PdfDataGridColumn<TModel> column = new PdfDataGridColumn<TModel>()
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

		public virtual PdfDataGridColumn<TModel> AddDataColumn<TProperty>(BindPropertyAction<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindPropertyAction<double, TModel> relativeWidth, BindPropertyAction<string, TModel> format, BindPropertyAction<string, TModel> headerStyleName, BindPropertyAction<string, TModel> cellStyleName)
		{
			PdfDataGridColumn<TModel> column = new PdfDataGridColumn<TModel>()
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
			int rowHeight = 0;

			//
			// Determine the column widths.
			//
			double sum = this.DataColumns.Sum(t => t.RelativeWidth.Resolve(g, m));

			//
			// The total of the column widths must be less
			// than or equal to bounds.Columns
			//
			int[] columnWidth = (from tbl in this.DataColumns
								 select (int)(bounds.Columns * (tbl.RelativeWidth.Resolve(g, m) / sum))).ToArray();

			//
			// Never under allocate the width.
			//
			if (columnWidth.Sum() < bounds.Columns)
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
				PdfTextElement<TModel> headerElement = new PdfTextElement<TModel>(column.ColumnHeader.Resolve(g, m));
				PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName.Resolve(g, m));
				PdfSize headerSize = headerElement.Measure(g, m, headerStyle);
				PdfBounds headerBounds = new PdfBounds(leftColumn, topRow, columnWidth[i], headerSize.Rows).SubtractBounds(g, m, headerStyle.Margin.Resolve(g, m));
				this.OnRenderHeaderColumn(g, m, headerBounds, headerStyle, headerElement);
				leftColumn += columnWidth[i];
				rowHeight = headerSize.Rows;
				i++;
			}

			//
			// Go to the next row.
			//
			topRow += rowHeight;

			//
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(g, m);

			if (items.Any())
			{
				//
				// Render the data.
				//
				foreach (TItem item in items)
				{
					leftColumn = bounds.LeftColumn;
					i = 0;

					foreach (PdfDataGridColumn<TModel> column in this.DataColumns)
					{
						PdfTextElement<TModel> dataElement = new PdfTextElement<TModel>(this.FormattedValue(g, m, column, item));
						PdfStyle<TModel> dataStyle = this.StyleManager.GetStyle(column.DataStyleName.Resolve(g, m));
						PdfSize dataSize = dataElement.Measure(g, m, dataStyle);
						PdfBounds dataBounds = new PdfBounds(leftColumn, topRow, columnWidth[i], dataSize.Rows).SubtractBounds(g, m, dataStyle.Margin.Resolve(g, m));
						this.OnRenderDataColumn(g, m, dataBounds, dataStyle, dataElement, item);
						leftColumn += columnWidth[i];
						rowHeight = dataSize.Rows;
						i++;
					}

					topRow += rowHeight;
				}
			}

			return Task.FromResult(returnValue);
		}

		protected virtual string FormattedValue(PdfGridPage g, TModel m, PdfDataGridColumn<TModel> column, TItem item)
		{
			//
			// For the property value.
			//
			PropertyInfo property = column.MemberExpression.Member as PropertyInfo;
			object value = property.GetValue(item);
			return column.StringFormat != null ? string.Format(column.StringFormat.Resolve(g, m), value) : Convert.ToString(value);
		}

		protected virtual void OnRenderHeaderColumn(PdfGridPage g, TModel m, PdfBounds dataBounds, PdfStyle<TModel> dataStyle, PdfTextElement<TModel> dataElement)
		{
			dataElement.Render(g, m, dataBounds, dataStyle);
		}

		protected virtual void OnRenderDataColumn(PdfGridPage g, TModel m, PdfBounds dataBounds, PdfStyle<TModel> dataStyle,  PdfTextElement<TModel> dataElement, TItem item)
		{
			dataElement.Render(g, m, dataBounds, dataStyle, item);
		}
	}
}
