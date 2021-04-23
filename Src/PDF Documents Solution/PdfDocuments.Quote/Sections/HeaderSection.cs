using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfDocument.BillOfLadingDocument;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.QuoteDocument
{
	public class HeaderSection : ChargesSection
	{
		public HeaderSection(PageCalculator<Charge> pageCalculator)
			: base(pageCalculator)
		{
		}

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, Quote model)
		{
			bool returnValue = true;

			// ***
			// ***
			// ***
			bool usePadding = this.UsePadding.Invoke(gridPage, model);

			// ***
			// *** Grid area.
			// ***
			IPdfBounds bounds = new PdfBounds(this.ActualBounds.LeftColumn,
											  this.ActualBounds.TopRow,
											  this.ActualBounds.Columns,
											  this.ActualBounds.Rows);

			// ***
			// ***
			// ***
			int bottom = this.RenderRowText(gridPage, bounds, 0,
						  new string[] { "Item", "Description", "Detail", "Amount" },
						  model);

			gridPage.DrawHorizontalLine(bottom, this.ActualBounds.LeftColumn + (usePadding ? this.Padding.Left : 0), this.ActualBounds.RightColumn - (usePadding ? this.Padding.Right : 0), RowEdge.Top, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyEmphasisColor);

			return Task.FromResult(returnValue);
		}

		protected override XFont[] OnGetFont(IPdfGridPage gridPage, Quote model)
		{
			return new XFont[]
			{
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge),
				gridPage.TitleFont().WithSize(gridPage.Theme.FontSize.BodyLarge)
			};
		}

		protected override XColor[] OnGetColor(IPdfGridPage gridPage, Quote model)
		{
			return new XColor[]
			{
				gridPage.Theme.Color.BodyColor,
				gridPage.Theme.Color.BodyColor,
				gridPage.Theme.Color.BodyColor,
				gridPage.Theme.Color.BodyColor
			};
		}
	}
}
