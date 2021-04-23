using System.Collections.Generic;
using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfDocument.BillOfLadingDocument;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;

namespace PdfDocument.QuoteDocument
{
	public class ChargeItemsSection : ChargesSection
	{
		public ChargeItemsSection(PageCalculator<Charge> pageCalculator)
			: base(pageCalculator)
		{
		}

		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, Quote model)
		{
			bool returnValue = true;

			// ***
			// *** Display the charges.
			// ***
			int row = 0;

			// ***
			// *** Get the charges for the current page.
			// ***
			IEnumerable<Charge> charges = this.PageCalculator.PartitionedItems[gridPage.PageNumber - 1];

			foreach (Charge charge in charges)
			{
				this.RenderRowText(gridPage, this.ActualBounds, row,
						  new string[] { $"{(row + 1):#,###}.", charge.Description, charge.Detail, charge.Amount.ToString("$#,##0.00") },
						  model);

				row++;
			}

			return Task.FromResult(returnValue);
		}
	}
}
