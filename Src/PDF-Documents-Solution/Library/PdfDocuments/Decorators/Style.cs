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
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	public static class Style
	{
		public static IStyleBuilder<TModel> Create<TModel>()
				where TModel : IPdfModel
		{
			return new PdfStyle<TModel>();
		}

		public static IStyleBuilder<TModel> Copy<TModel>(this PdfStyle<TModel> style)
				where TModel : IPdfModel
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
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XFont, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize, XFontStyle style)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize, style));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseFont<TModel>(this IStyleBuilder<TModel> styleBuilder, string familyName, double emSize)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Font = new BindProperty<XFont, TModel>((g, m) => new XFont(familyName, emSize));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderWidth<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderWidth = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBorderColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BorderColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseForegroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ForegroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseBackgroundColor<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XColor, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).BackgroundColor = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseCellPadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).CellPadding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeHeight<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeHeight = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseMargin<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Margin = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<PdfSpacing, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UsePadding<TModel>(this IStyleBuilder<TModel> styleBuilder, int left, int top, int right, int bottom)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).Padding = new BindProperty<PdfSpacing, TModel>((g, m) => (left, top, right, bottom));
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XStringFormat, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseTextAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XStringFormat, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).TextAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<XParagraphAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseParagraphAlignment<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<XParagraphAlignment, TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).ParagraphAlignment = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindProperty<double[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, BindPropertyAction<double[], TModel> value)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = value;
			return styleBuilder;
		}

		public static IStyleBuilder<TModel> UseRelativeWidths<TModel>(this IStyleBuilder<TModel> styleBuilder, params double[] values)
			where TModel : IPdfModel
		{
			((PdfStyle<TModel>)styleBuilder).RelativeWidths = new BindProperty<double[], TModel>((g, m) => values);
			return styleBuilder;
		}

		public static PdfStyle<TModel> Build<TModel>(this IStyleBuilder<TModel> styleBuilder)
			where TModel : IPdfModel
		{
			return (PdfStyle<TModel>)styleBuilder;
		}
	}
}
