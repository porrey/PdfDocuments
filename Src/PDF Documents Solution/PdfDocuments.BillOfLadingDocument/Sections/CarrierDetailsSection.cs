using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class CarrierDetailsSection : PdfSection<BillOfLading>
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
			// *** Use the standard body font.
			// ***
			XFont bodySmallFont = gridPage.BodyFont();
			IPdfSize bodySmallFontSize = gridPage.MeasureText(bodySmallFont, model.Shipper.Name);

			// ***
			// *** Draw the header.
			// ***
			int top = this.ActualBounds.TopRow;
			gridPage.DrawText("DELIVERY CARRIER:", bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			// ***
			// *** Show freight terms.
			// ***
			string terms = $"FREIGHT TERMS: {model.FreightTerms}";
			var termsSize = gridPage.MeasureText(bodyMediumBoldFont, terms);
			int left = this.ActualBounds.RightColumn - termsSize.Columns - (2 * this.Padding.Right);
			gridPage.DrawText(terms, bodyMediumBoldFont, left, top, termsSize.Columns, termsSize.Rows, XStringFormats.TopRight, gridPage.Theme.Color.BodyColor);

			// ***
			// *** Carrier Name
			// ***
			top += (bodyMediumBoldFontSize.Rows + this.Padding.Top);
			gridPage.DrawText($"{model.Carrier.Name} ({model.Carrier.Scac})", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			// ***
			// *** Show invoice remittance information.
			// ***
			top += bodyFontSize.Rows + (2 * this.Padding.Top);
			IPdfSize headerSize = gridPage.DrawText("Send Freight Bill and Delivery Receipt to:".ToUpper(), bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyEmphasisColor);

			top += bodyMediumBoldFontSize.Rows;
			gridPage.DrawText(model.FreightBillTo.Name.ToUpper(), bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodySmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyEmphasisColor);

			top += bodyFontSize.Rows;
			gridPage.DrawText($"{model.FreightBillTo.Address1} {model.FreightBillTo.City} {model.FreightBillTo.State} {model.FreightBillTo.Zip}".ToUpper(), bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodySmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyEmphasisColor);

			top += bodyFontSize.Rows;
			gridPage.DrawText(model.FreightBillTo.Comment, bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodySmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyHighlightColor);

			return Task.FromResult(returnValue);
		}
	}
}
