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

		public virtual PdfDataGridColumn AddDataColumn<TProperty>(string columnHeader, Expression<Func<TItem, TProperty>> expression, double relativeWidth, string format, string styleName = null)
		{
			PdfDataGridColumn column = new PdfDataGridColumn()
			{
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
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(g, m);

			if (items.Any())
			{
				//
				// Get the cell style.
				//
				PdfStyle<TModel> headerStyle = this.StyleManager.GetStyle(this.StyleNames.ElementAt(1));
				PdfSpacing headerPadding = headerStyle.Padding.Resolve(g, m);
				PdfSpacing headerCellPadding = headerStyle.CellPadding.Resolve(g, m);

				PdfStyle<TModel> bodyStyle = this.StyleManager.GetStyle(this.StyleNames.ElementAt(2));
				PdfSpacing bodyPadding = bodyStyle.Padding.Resolve(g, m);
				PdfSpacing bodyCellPadding = bodyStyle.CellPadding.Resolve(g, m);

				//
				// Get the font size.
				//
				PdfSize headerSize = g.MeasureText(headerStyle.Font.Resolve(g, m), this.DataColumns[0].ColumnHeader);
				PdfSize valueSize = g.MeasureText(headerStyle.Font.Resolve(g, m), this.FormattedValue(this.DataColumns[0], items.First()));

				//
				// Calculate the column header sizes.
				//
				PdfBounds[] headerCellBounds = new PdfBounds[this.DataColumns.Count];
				{
					int columnIndex = 0;
					int remainingColumns = bounds.Columns;

					foreach (PdfDataGridColumn dataColumn in this.DataColumns)
					{
						headerCellBounds[columnIndex] = new PdfBounds()
						{
							LeftColumn = columnIndex > 0 ? headerCellBounds[columnIndex - 1].RightColumn + 1 : bounds.LeftColumn,
							TopRow = bounds.TopRow,
							Rows = headerSize.Rows + headerPadding.Top + headerPadding.Bottom + headerCellPadding.Top + headerCellPadding.Bottom,
							Columns = columnIndex < (this.DataColumns.Count - 1) ? (int)(bounds.Columns * dataColumn.RelativeWidth) : remainingColumns
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

					foreach (PdfDataGridColumn dataColumn in this.DataColumns)
					{
						valueCellBounds[columnIndex] = new PdfBounds()
						{
							LeftColumn = columnIndex > 0 ? valueCellBounds[columnIndex - 1].RightColumn + 1 : bounds.LeftColumn,
							TopRow = headerCellBounds[0].BottomRow,
							Rows = valueSize.Rows + bodyPadding.Top + bodyPadding.Bottom + bodyPadding.Top + bodyPadding.Bottom,
							Columns = columnIndex < (this.DataColumns.Count - 1) ? (int)(bounds.Columns * dataColumn.RelativeWidth) : remainingColumns
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
						// Determine the text rectangle.
						//
						PdfBounds textBounds = headerCellBounds[columnIndex];
						PdfBounds paddedBounds = this.ApplyPadding(g, m, textBounds, headerPadding);
						PdfBounds cellPaddedBounds = this.ApplyPadding(g, m, paddedBounds, headerCellPadding);

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
							//
							// Determine the text rectangle.
							//
							PdfBounds textBounds = valueCellBounds[columnIndex].WithTopRow(topRow);
							PdfBounds paddedBounds = this.ApplyPadding(g, m, textBounds, bodyPadding);
							PdfBounds cellPaddedBounds = this.ApplyPadding(g, m, paddedBounds, bodyCellPadding);

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
