using System;
using System.Linq;
using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfDocument.BillOfLadingDocument;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.QuoteDocument
{
	public class TotalSection : ChargesSection
	{
		public TotalSection(PageCalculator<Charge> pageCalculator)
			: base(pageCalculator)
		{
		}

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, Quote model)
		{
			bool returnValue = true;

			// ***
			// *** Fill the entire section with the background color.
			// ***
			gridPage.DrawFilledRectangle(this.ActualBounds, gridPage.Theme.Color.AlternateBackgroundColor1);

			// ***
			// *** Calculate the total.
			// ***
			double total = Math.Round(model.Charges.Where(t => t.Code != Charges.Code.LinehaulNet).Sum(t => t.Amount), 2);

			// ***
			// *** Draw the total.
			// ***
			this.RenderRowText(gridPage, this.ActualBounds, 0,
								  new string[] { "TOTAL", String.Empty, String.Empty, $"{total:$#,##0.00}" },
								  model);

			return Task.FromResult(returnValue);
		}

		protected override XFont[] OnGetFont(IPdfGridPage gridPage, Quote model)
		{
			return new XFont[]
			{
				gridPage.TitleFont(XFontStyle.Bold).WithSize(gridPage.Theme.FontSize.BodyExtraLarge),
				gridPage.TitleLightFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleLightFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont(XFontStyle.Bold).WithSize(gridPage.Theme.FontSize.BodyExtraLarge)
			};
		}
	}
}
