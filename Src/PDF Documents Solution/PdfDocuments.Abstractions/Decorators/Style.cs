using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	public interface IStyleBuilder<TModel>
	{
	}

	public static class Style
	{
		public static IStyleBuilder<TModel> Create<TModel>()
		{
			return new PdfStyle<TModel>();
		}

		public static IStyleBuilder<TModel> Copy<TModel>(this PdfStyle<TModel> style)
		{
			return new PdfStyle<TModel>()
			{
				RelativeHeight = style.RelativeHeight,
				Font = style.Font,
				Margin = style.Margin,
				Padding = style.Padding,
				ForegroundColor = style.ForegroundColor,
				BackgroundColor = style.BackgroundColor,
				BorderWidth = style.BorderWidth,
				BorderColor = style.BorderColor,
				TextAlignment = style.TextAlignment,
				ParagraphAlignment = style.ParagraphAlignment,
				CellPadding = style.CellPadding,
				RelativeWidths = style.RelativeWidths
			};
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XFont, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XFont, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize, XFontStyle style)
		{
			((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize, style));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize)
		{
			((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
		{
			((PdfStyle<TModel>)styleBuilder).Margin = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
		{
			((PdfStyle<TModel>)styleBuilder).Padding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XStringFormat, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XStringFormat, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XParagraphAlignment, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XParagraphAlignment, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double[], TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double[], TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, params double[] values)
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = new BindProperty<double[], TModel>((g, m) => values);
			return styleBuilder;
		}

		public static PdfStyle<TModel> Build<TModel>(this IStyleBuilder<TModel> styleBuilder)
		{
			return (PdfStyle<TModel>)styleBuilder;
		}
	}
}
