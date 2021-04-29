using PdfSharp.Drawing;

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

		public static IStyleBuilder<TModel> Copy<TModel>(this IStyleBuilder<TModel> style)
		{
			return new PdfStyle<TModel>();
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XFont, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		public static PdfStyle<TModel> Build<TModel>(this IStyleBuilder<TModel> styleBuilder)
		{
			return (PdfStyle<TModel>)styleBuilder;
		}
	}
}
