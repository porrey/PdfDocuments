using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class ConsigneeDetailsSection : PdfSection<BillOfLading>
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
			gridPage.DrawText(model.Consignee.Name, bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyMediumBoldFontSize.Rows;
			gridPage.DrawText($"{model.Consignee.Address1} {model.Consignee.Address2}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyFontSize.Rows;
			gridPage.DrawText($"{model.Consignee.City}, {model.Consignee.State} {model.Consignee.Zip}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			return Task.FromResult(returnValue);
		}
	}
}
