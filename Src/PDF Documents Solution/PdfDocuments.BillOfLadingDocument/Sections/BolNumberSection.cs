using System.Threading.Tasks;
using Lsc.Logistics.Insight.Barcode.Abstractions;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class BolNumberSection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			string label = "Bill of Lading Number:";
			string bol = $"{model.ShipmentId:#0000000000}";

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyExtraLargeBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold).WithSize(16.5);
			IPdfSize bodyExtraLargeBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Draw the BOL label text.
			// ***
			int top = this.ActualBounds.TopRow + this.Padding.Top;
			IPdfSize labelSize = gridPage.DrawText(label, bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodySubtleColor);

			// ***
			// *** Draw the bar code.
			// ***
			int barcodeWidth = 30;
			PdfBounds bounds = new PdfBounds(this.ActualBounds.RightColumn - barcodeWidth + 1, top, barcodeWidth, this.ActualBounds.Rows + 3);
			gridPage.DrawBarCode(BarCodeType.Code39, bounds, HorizontalAlignment.Right, VerticalAlignment.Top, gridPage.Theme.Color.BodyBoldColor, gridPage.Theme.Color.BodyBackgroundColor, bol);

			// ***
			// *** Draw the BOL text.
			// ***
			top += bodyMediumBoldFontSize.Rows + this.Padding.Top;
			gridPage.DrawText(bol, bodyExtraLargeBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyExtraLargeBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			return Task.FromResult(returnValue);
		}
	}
}
