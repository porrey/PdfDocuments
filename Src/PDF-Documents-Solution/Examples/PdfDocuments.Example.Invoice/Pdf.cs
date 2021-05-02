using System.Collections.Generic;
using PdfSharp.Drawing;

namespace PdfDocuments.Example
{
	public static class Pdf
	{
		public static IPdfSection<TModel> VerticalStack<TModel>()
		{
			return new PdfVerticalStackSection<TModel>();
		}

		public static IPdfSection<TModel> HorizontalStack<TModel>()
		{
			return new PdfHorizontalStackSection<TModel>();
		}

		public static IPdfSection<TModel> SignatureSection<TModel>()
		{
			return new PdfSignatureSection<TModel>();
		}

		public static IPdfSection<TModel> EmptySection<TModel>()
		{
			return new PdfEmptySection<TModel>();
		}

		public static IPdfSection<TModel> HeaderContent<TModel>()
		{
			return new PdfHeaderContentSection<TModel>();
		}

		public static IPdfSection<TModel> NameValue<TModel>()
		{
			return new PdfNameValueSection<TModel>();
		}

		public static IPdfSection<TModel> StackedText<TModel>()
		{
			return new PdfStackedTextSection<TModel>();
		}

		public static IPdfSection<TModel> TextBlock<TModel>()
		{
			return new PdfTextBlockSection<TModel>();
		}

		public static IPdfSection<TModel> WrappingText<TModel>()
		{
			return new PdfWrappingTextSection<TModel>();
		}

		public static IPdfSection<TModel> PageHeader<TModel>()
		{
			return new PdfPageHeaderSection<TModel>();
		}

		public static IPdfSection<TModel> WithKey<TModel>(this IPdfSection<TModel> section, string value)
		{
			section.Key = value;
			return section;
		}

		

		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		public static IPdfSection<TModel> WithWatermark<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
		{
			section.WaterMarkImagePath = value;
			return section;
		}

		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
		{
			section.Text = value;
			return section;
		}

		public static IPdfSection<TModel> WithText<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
		{
			section.Text = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeHeight<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
		{
			section.RelativeHeight = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeHeight<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
		{
			section.RelativeHeight = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeWidth<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
		{
			section.RelativeWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithRelativeWidth<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
		{
			section.RelativeWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
		{
			if (section is IPdfTitle<TModel> titleSection)
			{
				titleSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithTitle<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
		{
			if (section is IPdfTitle<TModel> titleSection)
			{
				titleSection.Title = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogoPath<TModel>(this IPdfSection<TModel> section, BindProperty<string, TModel> value)
		{
			if (section is IPdfLogoPath<TModel> titleSection)
			{
				titleSection.LogoPath = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithLogoPath<TModel>(this IPdfSection<TModel> section, BindPropertyAction<string, TModel> value)
		{
			if (section is IPdfLogoPath<TModel> titleSection)
			{
				titleSection.LogoPath = value;
			}

			return section;
		}

		public static IPdfSection<TModel> WithBackgroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
		{
			section.BackgroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBackgroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
		{
			section.BackgroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithForegroundColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
		{
			section.ForegroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithForegroundColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
		{
			section.ForegroundColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithFont<TModel>(this IPdfSection<TModel> section, BindProperty<XFont, TModel> value)
		{
			section.Font = value;
			return section;
		}

		public static IPdfSection<TModel> WithFont<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XFont, TModel> value)
		{
			section.Font = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderColor<TModel>(this IPdfSection<TModel> section, BindProperty<XColor, TModel> value)
		{
			section.BorderColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderColor<TModel>(this IPdfSection<TModel> section, BindPropertyAction<XColor, TModel> value)
		{
			section.BorderColor = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderWidth<TModel>(this IPdfSection<TModel> section, BindProperty<double, TModel> value)
		{
			section.BorderWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithBorderWidth<TModel>(this IPdfSection<TModel> section, BindPropertyAction<double, TModel> value)
		{
			section.BorderWidth = value;
			return section;
		}

		public static IPdfSection<TModel> WithPadding<TModel>(this IPdfSection<TModel> section, IPdfSpacing value)
		{
			section.Padding = value;
			return section;
		}

		public static IPdfSection<TModel> WithMargin<TModel>(this IPdfSection<TModel> section, IPdfSpacing value)
		{
			section.Margin = value;
			return section;
		}

		public static IPdfSection<TModel> AssignParentSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
		{
			section.ParentSection = value;
			return section;
		}

		public static IPdfSection<TModel> AddChildSection<TModel>(this IPdfSection<TModel> section, IPdfSection<TModel> value)
		{
			if (value != null)
			{
				value.ParentSection = section;
				section.Children.Add(value);
			}

			return section;
		}

		public static IPdfSection<TModel> WithParent<TModel>(this IPdfSection<TModel> section)
		{
			return section.ParentSection;
		}
	}
}
