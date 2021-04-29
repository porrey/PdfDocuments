using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocuments
{
	public class PdfStyle<TModel> : IStyleBuilder<TModel>
	{
		public virtual BindProperty<double, TModel> RelativeHeight { get; set; } = 0;
		public virtual BindProperty<XFont, TModel> Font { get; set; } = new XFont("Arial", 12);
		public virtual BindProperty<PdfSpacing, TModel> Margin { get; set; } = new PdfSpacing(0, 0, 0, 0);
		public virtual BindProperty<PdfSpacing, TModel> Padding { get; set; } = new PdfSpacing(0, 0, 0, 0);
		public virtual BindProperty<XColor, TModel> ForegroundColor { get; set; } = XColors.Black;
		public virtual BindProperty<XColor, TModel> BackgroundColor { get; set; } = XColors.Transparent;
		public virtual BindProperty<double, TModel> BorderWidth { get; set; } = 0;
		public virtual BindProperty<XColor, TModel> BorderColor { get; set; } = XColors.Transparent;
		public virtual BindProperty<XStringFormat, TModel> TextAlignment { get; set; } = XStringFormats.CenterLeft;
		public virtual BindProperty<XParagraphAlignment, TModel> ParagraphAlignment { get; set; } = XParagraphAlignment.Justify;
		public virtual BindProperty<PdfSpacing, TModel> CellPadding { get; set; } = new PdfSpacing(1, 1, 1, 1);
		public virtual BindProperty<double[], TModel> RelativeWidths { get; set; } = new double[0];
	}
}
