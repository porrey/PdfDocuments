using PdfDocument.Abstractions;
using PdfDocument.BillOfLadingDocument;
using PdfDocument.DocumentShared;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.QuoteDocument
{
	public abstract class ChargesSection : GridSection<Quote>
	{
		public ChargesSection(PageCalculator<Charge> pageCalculator)
			: base(pageCalculator)
		{
		}

		protected override int[] OnGetColumnWidth(IPdfGridPage gridPage, Quote model)
		{
			// ***
			// *** Create 3 columns for the display of the data. These values are used
			// *** as relative widths.
			// ***
			return new int[]
			{
				(int)(.07 * this.ActualBounds.Columns),
				(int)(.34 * this.ActualBounds.Columns),
				(int)(.44 * this.ActualBounds.Columns),
				(int)(.15 * this.ActualBounds.Columns)
			};
		}

		protected override XStringFormat[] OnGetFormat(IPdfGridPage gridPage, Quote model)
		{
			return new XStringFormat[]
			{
				XStringFormats.CenterLeft,
				XStringFormats.CenterLeft,
				XStringFormats.CenterLeft,
				XStringFormats.CenterRight
			};
		}

		protected override XFont[] OnGetFont(IPdfGridPage gridPage, Quote model)
		{
			return new XFont[]
			{
				gridPage.TitleLightFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleLightFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge)
			};
		}

		protected override XColor[] OnGetColor(IPdfGridPage gridPage, Quote model)
		{
			return new XColor[]
			{
				gridPage.Theme.Color.BodyColor,
				gridPage.Theme.Color.BodyColor,
				gridPage.Theme.Color.BodyEmphasisColor,
				gridPage.Theme.Color.BodyColor
			};
		}
	}
}
