/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfDataGridSection<TModel, TItem> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfDataGridSection()
		{
			this.ColumnHeaderFont = new BindProperty<XFont, TModel>((g, m) => g.BodyMediumFont(XFontStyle.Regular).WithSize(10));
			this.ColumnValueFont = new BindProperty<XFont, TModel>((g, m) => g.BodyLightFont(XFontStyle.Bold).WithSize(10));
			this.ColumnHeaderColor = new BindProperty<XColor, TModel>((g, m) => g.Theme.Color.BodyColor);
			this.ColumnValueColor = new BindProperty<XColor, TModel>((g, m) => g.Theme.Color.BodyColor);
		}

		public PdfSpacing CellPadding { get; set; } = new PdfSpacing(1, 1, 1, 1);
		public BindProperty<XFont, TModel> ColumnHeaderFont { get; set; }
		public BindProperty<XFont, TModel> ColumnValueFont { get; set; }
		public BindProperty<XColor, TModel> ColumnHeaderColor { get; set; }
		public BindProperty<XColor, TModel> ColumnValueColor { get; set; }
		public IList<PdfDataGridColumn> DataColumns { get; } = new List<PdfDataGridColumn>();

		public BindProperty<IEnumerable<TItem>, TModel> Items { get; set; } = new TItem[0];

		public virtual PdfDataGridColumn AddDataColumn<TProperty>(string columnHeader, Expression<Func<TItem, TProperty>> expression, double relativeWidth, string format, XStringFormat alignment)
		{
			PdfDataGridColumn column = new PdfDataGridColumn()
			{
				ColumnHeader = columnHeader,
				MemberExpression = expression.Body as MemberExpression,
				RelativeWidth = relativeWidth,
				Format = format,
				Alignment = alignment
			};

			this.DataColumns.Add(column);
			return column;
		}

		protected override Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the items.
			//
			IEnumerable<TItem> items = this.Items.Resolve(gridPage, model);

			if (items.Any())
			{
				//
				// Get the font size.
				//
				PdfSize headerSize = gridPage.MeasureText(this.ColumnValueFont.Resolve(gridPage, model), this.DataColumns[0].ColumnHeader);
				PdfSize valueSize = gridPage.MeasureText(this.ColumnValueFont.Resolve(gridPage, model), this.FormattedValue(this.DataColumns[0], items.First()));

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
							Rows = headerSize.Rows + this.Padding.Top + this.Padding.Bottom,
							Columns = columnIndex < (this.DataColumns.Count - 1) ? (int)(bounds.Columns * dataColumn.RelativeWidth) : remainingColumns
						};

						remainingColumns -= headerCellBounds[columnIndex].Columns;
						columnIndex++;
					}
				}

				//
				// Calculate the column header sizes.
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
							Rows = valueSize.Rows + this.Padding.Top + this.Padding.Bottom,
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
						PdfBounds paddedBounds = this.ApplyPadding(gridPage, model, textBounds, this.Padding);
						PdfBounds cellPaddedBounds = this.ApplyPadding(gridPage, model, paddedBounds, this.CellPadding);

						gridPage.DrawFilledRectangle(paddedBounds, XColors.LightGray);

						//
						// Draw the text.
						//
						gridPage.DrawText(column.ColumnHeader, this.ColumnHeaderFont.Resolve(gridPage, model), cellPaddedBounds, column.Alignment, this.ColumnHeaderColor.Resolve(gridPage, model));
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
							PdfBounds paddedBounds = this.ApplyPadding(gridPage, model, textBounds, this.Padding);
							PdfBounds cellPaddedBounds = this.ApplyPadding(gridPage, model, paddedBounds, this.CellPadding);

							//
							// Draw the text.
							//
							gridPage.DrawText(this.FormattedValue(column, item), this.ColumnValueFont.Resolve(gridPage, model), cellPaddedBounds, column.Alignment, this.ColumnValueColor.Resolve(gridPage, model));
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
			return column.Format != null ? string.Format(column.Format, value) : Convert.ToString(value);
		}
	}
}
