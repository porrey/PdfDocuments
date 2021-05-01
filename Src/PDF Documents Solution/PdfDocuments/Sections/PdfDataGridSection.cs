/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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
		public IList<PdfDataGridColumn> DataColumns { get; } = new List<PdfDataGridColumn>();
		public BindProperty<IEnumerable<TItem>, TModel> Items { get; set; } = new TItem[0];

		public virtual PdfDataGridColumn AddDataColumn<TProperty>(string columnHeader, Expression<Func<TItem, TProperty>> expression, double relativeWidth, string format, string headerStyleName, string cellStyleName)
		{
			PdfDataGridColumn column = new PdfDataGridColumn()
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
			// Determine the column width.
			//
			int columnWidth = bounds.Columns / this.DataColumns.Count();

			//
			// Render the headers.
			//
			foreach (PdfDataGridColumn column in this.DataColumns)
			{
				PdfTextElement<TModel> headerElement = new PdfTextElement<TModel>(column.ColumnHeader);
				PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName);
				PdfSize headerSize = headerElement.Measure(g, m, headerStyle);
				PdfBounds headerBounds = new PdfBounds(leftColumn, topRow, columnWidth, headerSize.Rows).SubtractBounds(g, m, headerStyle.Margin.Resolve(g, m));
				headerElement.Render(g, m, headerBounds, headerStyle);
				leftColumn += columnWidth;
				rowHeight = headerSize.Rows;
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
				foreach (var item in items)
				{
					leftColumn = bounds.LeftColumn;

					foreach (PdfDataGridColumn column in this.DataColumns)
					{
						PdfTextElement<TModel> dataElement = new PdfTextElement<TModel>(this.FormattedValue(column, item));
						PdfStyle<TModel> dataStyle = this.StyleManager.GetStyle(column.DataStyleName);
						PdfSize dataSize = dataElement.Measure(g, m, dataStyle);
						PdfBounds dataBounds = new PdfBounds(leftColumn, topRow, columnWidth, dataSize.Rows).SubtractBounds(g, m, dataStyle.Margin.Resolve(g, m));
						dataElement.Render(g, m, dataBounds, dataStyle);
						leftColumn += columnWidth;
						rowHeight = dataSize.Rows;
					}

					topRow += rowHeight;
				}
			}

			return Task.FromResult(returnValue);
		}

		protected Task<bool> OnRenderAsync1(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(g, m);

			if (items.Any())
			{
				//
				// Calculate the column header sizes.
				//
				PdfBounds[] headerCellBounds = new PdfBounds[this.DataColumns.Count];
				{
					int columnIndex = 0;
					int remainingColumns = bounds.Columns;

					foreach (PdfDataGridColumn column in this.DataColumns)
					{
						//
						// Get the cell style.
						//
						PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName);
						PdfSpacing headerPadding = headerStyle.Padding.Resolve(g, m);
						PdfSpacing headerCellPadding = headerStyle.CellPadding.Resolve(g, m);
						PdfSize headerSize = g.MeasureText(headerStyle.Font.Resolve(g, m), column.ColumnHeader);

						headerCellBounds[columnIndex] = new PdfBounds()
						{
							LeftColumn = columnIndex > 0 ? headerCellBounds[columnIndex - 1].RightColumn + 1 : bounds.LeftColumn,
							TopRow = bounds.TopRow,
							Rows = headerSize.Rows + headerPadding.Top + headerPadding.Bottom + headerCellPadding.Top + headerCellPadding.Bottom,
							Columns = columnIndex < (this.DataColumns.Count - 1) ? (int)(bounds.Columns * column.RelativeWidth) : remainingColumns
						};

						remainingColumns -= headerCellBounds[columnIndex].Columns;
						columnIndex++;
					}
				}

				//
				// Calculate the column body sizes.
				//
				PdfBounds[] valueCellBounds = new PdfBounds[this.DataColumns.Count];
				{
					int columnIndex = 0;
					int remainingColumns = bounds.Columns;

					foreach (PdfDataGridColumn column in this.DataColumns)
					{
						PdfStyle<TModel> bodyStyle = this.StyleManager.GetStyle(column.DataStyleName);
						PdfSpacing bodyPadding = bodyStyle.Padding.Resolve(g, m);
						PdfSpacing bodyCellPadding = bodyStyle.CellPadding.Resolve(g, m);
						PdfSize valueSize = g.MeasureText(bodyStyle.Font.Resolve(g, m), this.FormattedValue(column, items.First()));

						valueCellBounds[columnIndex] = new PdfBounds()
						{
							LeftColumn = columnIndex > 0 ? valueCellBounds[columnIndex - 1].RightColumn + 1 : bounds.LeftColumn,
							TopRow = headerCellBounds[0].BottomRow,
							Rows = valueSize.Rows + bodyPadding.Top + bodyPadding.Bottom + bodyPadding.Top + bodyPadding.Bottom,
							Columns = columnIndex < (this.DataColumns.Count - 1) ? (int)(bounds.Columns * column.RelativeWidth) : remainingColumns
						};

						remainingColumns -= headerCellBounds[columnIndex].Columns;
						columnIndex++;
					}
				}

				//
				// Print the data.
				//
				{
					int columnIndex = 0;

					foreach (PdfDataGridColumn column in this.DataColumns)
					{
						//
						// Get the cell style.
						//
						PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(column.HeaderStyleName);
						PdfSpacing headerPadding = headerStyle.Padding.Resolve(g, m);
						PdfSpacing headerCellPadding = headerStyle.CellPadding.Resolve(g, m);

						//
						// Determine the text rectangle.
						//
						PdfBounds textBounds = headerCellBounds[columnIndex];
						PdfBounds paddedBounds = textBounds.SubtractBounds(g, m, headerPadding);
						PdfBounds cellPaddedBounds = paddedBounds.SubtractBounds(g, m, headerCellPadding);

						//
						// Draw the background
						//
						g.DrawFilledRectangle(paddedBounds, headerStyle.BackgroundColor.Resolve(g, m));

						//
						// Draw the text.
						//
						g.DrawText(column.ColumnHeader, headerStyle.Font.Resolve(g, m), cellPaddedBounds, headerStyle.TextAlignment.Resolve(g, m), headerStyle.ForegroundColor.Resolve(g, m));
						columnIndex++;
					}

					//
					// Print the data.
					//
					int topRow = valueCellBounds[0].TopRow;

					foreach (TItem item in items)
					{
						columnIndex = 0;

						foreach (PdfDataGridColumn column in this.DataColumns)
						{
							PdfStyle<TModel> bodyStyle = this.StyleManager.GetStyle(column.DataStyleName);
							PdfSpacing bodyPadding = bodyStyle.Padding.Resolve(g, m);
							PdfSpacing bodyCellPadding = bodyStyle.CellPadding.Resolve(g, m);

							//
							// Determine the text rectangle.
							//
							PdfBounds textBounds = valueCellBounds[columnIndex].WithTopRow(topRow);
							PdfBounds paddedBounds = textBounds.SubtractBounds(g, m, bodyPadding);
							PdfBounds cellPaddedBounds = paddedBounds.SubtractBounds(g, m, bodyCellPadding);

							//
							// Draw the background
							//
							g.DrawFilledRectangle(paddedBounds, bodyStyle.BackgroundColor.Resolve(g, m));

							//
							// Draw the text.
							//
							g.DrawText(this.FormattedValue(column, item), bodyStyle.Font.Resolve(g, m), cellPaddedBounds, bodyStyle.TextAlignment.Resolve(g, m), bodyStyle.ForegroundColor.Resolve(g, m));
							columnIndex++;
						}

						topRow += valueCellBounds[0].Rows;
					}
				}
			}

			return Task.FromResult(returnValue);
		}

		private string FormattedValue(PdfDataGridColumn column, TItem item)
		{
			//
			// For the property value.
			//
			PropertyInfo property = column.MemberExpression.Member as PropertyInfo;
			object value = property.GetValue(item);
			return column.StringFormat != null ? string.Format(column.StringFormat, value) : Convert.ToString(value);
		}
	}
}
