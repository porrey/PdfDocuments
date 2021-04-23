using System;
using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class ShipperAddressSection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyFont = gridPage.BodyFont();
			IPdfSize bodyFontSize = gridPage.MeasureText(bodyFont, model.Shipper.Name);

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Draw the address.
			// ***
			int top = this.ActualBounds.TopRow;
			gridPage.DrawText(model.Shipper.Name, bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyMediumBoldFontSize.Rows;
			gridPage.DrawText($"{model.Shipper.Address1} {model.Shipper.Address2}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyFontSize.Rows;
			gridPage.DrawText($"{model.Shipper.City}, {model.Shipper.State} {model.Shipper.Zip}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			// ***
			// *** Draw the pickup date and time.
			// ***
			top += bodyFontSize.Rows + (2 * this.Padding.Top);
			gridPage.DrawText("SCHEDULED PICK-UP DATE & TIME:", bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyMediumBoldFontSize.Rows + this.Padding.Top;

			string pickupDateTime = $"{model.PickupDateTime:dddd MMM d, yyyy} at {model.PickupDateTime:h:mm tt}";
			gridPage.DrawText(pickupDateTime, bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			return Task.FromResult(returnValue);
		}
	}
}
