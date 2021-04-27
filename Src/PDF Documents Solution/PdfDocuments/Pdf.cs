﻿/*
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
using System.Linq.Expressions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public static class Pdf
	{
		public static void SetKey<TModel>(this IPdfSection<TModel> section)
			where TModel : IPdfModel
		{
			section.Key = section.GetType().Name.Replace("Pdf", "").Replace("Section", "").Replace("`1", "");
		}

		public static IPdfSection<TModel> VerticalStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfVerticalStackSection<TModel>();
		}

		public static IPdfSection<TModel> VerticalStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfVerticalStackSection<TModel>(children);
		}

		public static IPdfSection<TModel> HorizontalStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfHorizontalStackSection<TModel>();
		}

		public static IPdfSection<TModel> HorizontalStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfHorizontalStackSection<TModel>(children);
		}

		public static IPdfSection<TModel> SignatureSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfSignatureSection<TModel>();
		}

		public static IPdfSection<TModel> EmptySection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfEmptySection<TModel>();
		}

		public static IPdfSection<TModel> HeaderContentSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfHeaderContentSection<TModel>();
		}

		public static IPdfSection<TModel> KeyValueSection<TModel>(params PdfKeyValueItem<TModel>[] values)
			where TModel : IPdfModel
		{
			return new PdfKeyValueSection<TModel>(values);
		}

		public static IPdfSection<TModel> StackedTextSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfStackedTextSection<TModel>();
		}

		public static IPdfSection<TModel> TextBlockSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfTextBlockSection<TModel>();
		}

		public static IPdfSection<TModel> WrappingTextSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfWrappingTextSection<TModel>();
		}

		public static IPdfSection<TModel> PageHeaderSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfPageHeaderSection<TModel>();
		}

		public static IPdfSection<TModel> PageFooterSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfPageFooterSection<TModel>();
		}

		public static IPdfSection<TModel> DataGridSection<TModel, TItem>()
			where TModel : IPdfModel
		{
			return new PdfDataGridSection<TModel, TItem>();
		}

		public static IPdfSection<TModel> AddColumn<TModel, TItem, TProperty>(this IPdfSection<TModel> section, string columnHeader, Expression<Func<TItem, TProperty>> expression, double relativeWidth, string format, XStringFormat alignment)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.AddDataColumn(columnHeader, expression, relativeWidth, format, alignment);
			}
			return section;
		}

		public static IPdfSection<TModel> UseItems<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<IEnumerable<TItem>, TModel> items)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.Items = items;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnHeaderFont<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnHeaderFont = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnValueFont<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnValueFont = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnHeaderForegroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnHeaderForegroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnHeaderForegroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnHeaderForegroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnValueForegroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnValueForegroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnValueForegroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnValueForegroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnHeaderBackgroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnHeaderBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnHeaderBackgroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnHeaderBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnValueBackgroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnValueBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithColumnValueBackgroundColor<TModel, TItem>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.ColumnValueBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithCellPadding<TModel, TItem>(this IPdfSection<TModel> section, PdfSpacing value)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.CellPadding = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithCellPadding<TModel, TItem>(this IPdfSection<TModel> section, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.CellPadding = (left, top, right, bottom);
			}
			return section;
		}

		public static IPdfSection<TModel> WithKey<TModel>(this IPdfSection<TModel> section, string value)
			where TModel : IPdfModel
		{
			section.Key = value;
			return section;
		}

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

		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			section.Text = value;
			return section;
		}

		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			section.Text = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeHeight<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			section.RelativeHeight = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeHeight<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			section.RelativeHeight = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeWidth<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			section.RelativeWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeWidth<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			section.RelativeWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfTitle<TModel> titleSection)
			{
				titleSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfTitle<TModel> titleSection)
			{
				titleSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogoPath<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfLogoPath<TModel> titleSection)
			{
				titleSection.LogoPath = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogoPath<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfLogoPath<TModel> titleSection)
			{
				titleSection.LogoPath = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBackgroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.BackgroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBackgroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.BackgroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithForegroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.ForegroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithForegroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.ForegroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithFont<TModel>(this IPdfSection<TModel> section, BindProperty<XFont, TModel> value)
			where TModel : IPdfModel
		{
			section.Font = value;
			return section;
		}

		public static IPdfSection<TModel> WithFont<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			section.Font = value;
			return section;
		}

		public static IPdfSection<TModel> WithValueFont<TModel>(this IPdfSection<TModel> section, BindProperty<XFont, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfValueFont<TModel> s)
			{
				s.ValueFont = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithValueFont<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IPdfValueFont<TModel> s)
			{
				s.ValueFont = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithBorderColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.BorderColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			section.BorderColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderWidth<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			section.BorderWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderWidth<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			section.BorderWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithPadding<TModel>(this IPdfSection<TModel> section, PdfSpacing value)
			where TModel : IPdfModel
		{
			section.Padding = value;
			return section;
		}

		public static IPdfSection<TModel> WithPadding<TModel>(this IPdfSection<TModel> section, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			section.Padding = (left, top, right, bottom);
			return section;
		}

		public static IPdfSection<TModel> WithMargin<TModel>(this IPdfSection<TModel> section, PdfSpacing value)
			where TModel : IPdfModel
		{
			section.Margin = value;
			return section;
		}

		public static IPdfSection<TModel> WithMargin<TModel>(this IPdfSection<TModel> section, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			section.Margin = (left, top, right, bottom);
			return section;
		}

		public static IPdfSection<TModel> AssignParentSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			section.ParentSection = value;
			return section;
		}

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

		public static IPdfSection<TModel> WithParent<TModel>(this IPdfSection<TModel> section)
			where TModel : IPdfModel
		{
			return section.ParentSection;
		}

		public static IPdfSection<TModel> WithRenderCondition<TModel>(this IPdfSection<TModel> section, BindPropertyAction<bool, TModel> value)
			where TModel : IPdfModel
		{
			section.ShouldRender = value;
			return section;
		}

		public static IPdfSection<TModel> WithContentSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			section.AddChildSection(value);
			return section;
		}

		public static IPdfSection<TModel> WithHeaderBackgroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IHeaderBackgroundColor<TModel> s)
			{
				s.HeaderBackgroundColor = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithHeaderBackgroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IHeaderBackgroundColor<TModel> s)
			{
				s.HeaderBackgroundColor = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithHeaderForegroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IHeaderForegroundColor<TModel> s)
			{
				s.HeaderForegroundColor = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithHeaderForegroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is IHeaderForegroundColor<TModel> s)
			{
				s.HeaderForegroundColor = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTextAlignment<TModel>(this IPdfSection<TModel> section, BindProperty<XStringFormat, TModel> value)
		where TModel : IPdfModel
		{
			section.TextAlignment = value;
			return section;
		}

		public static IPdfSection<TModel> WithTextAlignment<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XStringFormat, TModel> value)
			where TModel : IPdfModel
		{
			section.TextAlignment = value;
			return section;
		}

		public static IPdfSection<TModel> WithCellBackgroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfKeyValueSection<TModel> s)
			{
				s.CellBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithCellBackgroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfKeyValueSection<TModel> s)
			{
				s.CellBackgroundColor = value;
			}
			return section;
		}

		public static IPdfSection<TModel> WithKeyRelativeWidth<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfKeyValueSection<TModel> s)
			{
				s.KeyRelativeWidth = value;
			}
			return section;
		}

	}
}