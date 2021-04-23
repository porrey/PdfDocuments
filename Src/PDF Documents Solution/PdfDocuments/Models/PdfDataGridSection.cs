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
using PdfDocuments.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public interface IDataGridColumn
	{
		MemberExpression MemberExpression { get; set; }
		double RelativeWidth { get; set; }
		string Format { get; set; }
		XStringFormat Alignment { get; set; }
		IPdfBounds ActualBounds { get; set; }
	}

	public class DataGridColumn : IDataGridColumn
	{
		public MemberExpression MemberExpression { get; set; }
		public double RelativeWidth { get; set; }
		public string Format { get; set; }
		public XStringFormat Alignment { get; set; }
		public IPdfBounds ActualBounds { get; set; }
	}

	public abstract class PdfDataGridSection<TModel, TInterface> : PdfSection<TModel>
	{
		public PdfDataGridSection()
			: base()
		{
		}

		public PdfDataGridSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

		protected XFont ColumnHeaderFont { get; set; }
		protected XFont ColumnFont { get; set; }
		protected XColor ColumnHeaderColor { get; set; }
		protected XColor ColumnColor { get; set; }
		protected IEnumerable<TInterface> Items { get; set; } = new TInterface[0];
		protected IList<IDataGridColumn> Columns { get; } = new List<IDataGridColumn>();

		protected virtual IDataGridColumn AddColumn<TItem>(Expression<Func<TInterface, TItem>> expression, double relativeWidth, string format, XStringFormat alignment)
		{
			IDataGridColumn column = new DataGridColumn()
			{
				MemberExpression = expression.Body as MemberExpression,
				RelativeWidth = relativeWidth,
				Format = format,
				Alignment = alignment
			};

			this.Columns.Add(column);
			return column;
		}

		public override Task<bool> LayoutAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			if (this.Items.Count() > 0)
			{
				//
				//
				//
				this.ColumnHeaderFont = this.OnGetColumnHeaderFont(gridPage);
				this.ColumnFont = this.OnGetColumnFont(gridPage);

				//
				//
				//
				IPdfSize size = gridPage.MeasureText(this.ColumnFont, "Test");

				//
				//
				//
				IDataGridColumn previousColumn = null;
				foreach (IDataGridColumn column in this.Columns)
				{
					column.ActualBounds.LeftColumn = this.ActualBounds.LeftColumn + (previousColumn != null ? previousColumn.ActualBounds.Columns : 0);
					column.ActualBounds.Columns = (int)(this.ActualBounds.Columns * column.RelativeWidth);
					column.ActualBounds.Rows = size.Rows + this.Padding.Top;
					column.ActualBounds.TopRow = this.ActualBounds.TopRow;

					previousColumn = column;
				}
			}

			return Task.FromResult(returnValue);
		}

		public override Task<bool> RenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			this.ColumnColor = this.OnGetColumnColor(gridPage);
			this.ColumnHeaderColor = this.OnGetColumnHeaderColor(gridPage);

			if (this.Items.Count() > 0)
			{
				int top = this.ActualBounds.TopRow;

				foreach (TInterface item in this.Items)
				{
					//
					//
					//
					foreach (IDataGridColumn column in this.Columns)
					{
						PropertyInfo property = column.MemberExpression.Member as PropertyInfo;
						string value = property.GetValue(item).ToString();
						string formattedValue = column.Format != null ? string.Format(column.Format, value) : value;
						gridPage.DrawText(formattedValue, this.ColumnFont, column.ActualBounds.WithTopRow(top + column.ActualBounds.Rows), column.Alignment, this.ColumnColor);
					}
				}
			}

			return Task.FromResult(returnValue);
		}

		protected virtual XFont OnGetColumnFont(IPdfGridPage gridPage)
		{
			return gridPage.BodyMediumFont(XFontStyle.Bold);
		}

		protected virtual XFont OnGetColumnHeaderFont(IPdfGridPage gridPage)
		{
			return gridPage.BodyFont();
		}

		protected virtual XColor OnGetColumnHeaderColor(IPdfGridPage gridPage)
		{
			return gridPage.Theme.Color.BodyColor;
		}

		protected virtual XColor OnGetColumnColor(IPdfGridPage gridPage)
		{
			return gridPage.Theme.Color.BodyColor;
		}
	}
}
