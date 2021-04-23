using System.Threading.Tasks;
using Lsc.Logistics.Insight.Barcode.Abstractions;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class CarrierProNumberSection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			string label = "CARRIER PRO NUMBER";

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Draw the label.
			// ***
			int top = this.ActualBounds.TopRow;
			gridPage.DrawText(label, bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.CenterRight, gridPage.Theme.Color.BodyColor);

			// ***
			// *** Draw the bar code.
			// ***
			PdfBounds bounds = new PdfBounds(this.ActualBounds.LeftColumn, top + bodyMediumBoldFontSize.Rows + this.Padding.Top, this.ActualBounds.Columns, this.ActualBounds.Rows - bodyMediumBoldFontSize.Rows + 3);
			gridPage.DrawBarCode(BarCodeType.Code39, bounds, HorizontalAlignment.Right, VerticalAlignment.Center, gridPage.Theme.Color.BodyBoldColor, gridPage.Theme.Color.BodyBackgroundColor, model.Carrier.ProNumber);

			return Task.FromResult(returnValue);
		}
	}
}
