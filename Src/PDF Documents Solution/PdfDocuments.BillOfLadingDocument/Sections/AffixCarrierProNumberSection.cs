using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class AffixCarrierProNumberSection : PdfSection<BillOfLading>
	{
		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			string label = "AFFIX CARRIER PRO NUMBER";

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Draw the label.
			// ***
			int top = this.ActualBounds.TopRow;
			gridPage.DrawText(label, bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.Center, gridPage.Theme.Color.BodyVeryLightColor);

			return Task.FromResult(returnValue);
		}
	}
}
