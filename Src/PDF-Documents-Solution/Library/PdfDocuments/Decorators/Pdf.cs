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
using System.Linq.Expressions;

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

		public static IPdfSection<TModel> OverlayStackSection<TModel>()
			where TModel : IPdfModel
		{
			return new PdfOverlayStackSection<TModel>();
		}

		public static IPdfSection<TModel> OverlayStackSection<TModel>(params IPdfSection<TModel>[] children)
			where TModel : IPdfModel
		{
			return new PdfOverlayStackSection<TModel>(children);
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

		public static IPdfSection<TModel> ContentSection<TModel>(IPdfSection<TModel> value)
			where TModel : IPdfModel
		{
			return (new PdfContentSection<TModel>()).AddChildSection(value);
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

		public static IPdfSection<TModel> WithStyles<TModel>(this IPdfSection<TModel> section, params string[] styleNames)
			where TModel : IPdfModel
		{
			section.StyleNames = styleNames;
			return section;
		}

		public static IPdfSection<TModel> DataGridSection<TModel, TItem>()
			where TModel : IPdfModel
		{
			return new PdfDataGridSection<TModel, TItem>();
		}

		public static IPdfSection<TModel> AddColumn<TModel, TItem, TProperty>(this IPdfSection<TModel> section, BindProperty<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindProperty<double, TModel> relativeWidth, BindProperty<string, TModel> format, BindProperty<string, TModel> headerStyleName, BindProperty<string, TModel> cellStyleName)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
			}
			return section;
		}

		public static IPdfSection<TModel> AddColumn<TModel, TItem, TProperty>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> columnHeader, Expression<Func<TItem, TProperty>> expression, BindPropertyAction<double, TModel> relativeWidth, BindPropertyAction<string, TModel> format, BindPropertyAction<string, TModel> headerStyleName, BindPropertyAction<string, TModel> cellStyleName)
			where TModel : IPdfModel
		{
			if (section is PdfDataGridSection<TModel, TItem> s)
			{
				s.AddDataColumn(columnHeader, expression, relativeWidth, format, headerStyleName, cellStyleName);
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

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogo<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Logo = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogo<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageHeaderSection<TModel> headerSection)
			{
				headerSection.Logo = value;
			}

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

		public static IPdfSection<TModel> WithTopLeftText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopLeftText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTopLeftText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopLeftText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTopRightText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopRightText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTopRightText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.TopRightText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBottomLeftText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomLeftText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBottomLeftText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomLeftText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBottomRightText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomRightText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBottomRightText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
			where TModel : IPdfModel
		{
			if (section is PdfPageFooterSection<TModel> footerSection)
			{
				footerSection.BottomRightText = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithStackedItems<TModel>(this IPdfSection<TModel> section, params BindProperty<string, TModel>[] values)
			where TModel : IPdfModel
		{
			if (section is PdfStackedTextSection<TModel> stackedSection)
			{
				stackedSection.StackedItems = values;
			}

			return section;
		}

		public static IPdfSection<TModel> WithStackedItems<TModel>(this IPdfSection<TModel> section, params BindPropertyAction<string, TModel>[] values)
			where TModel : IPdfModel
		{
			if (section is PdfStackedTextSection<TModel> stackedSection)
			{
				IList<BindProperty<string, TModel>> items = new List<BindProperty<string, TModel>>();

				foreach (var value in values)
				{
					items.Add(value);
				}

				stackedSection.StackedItems = items;
			}

			return section;
		}
	}
}
