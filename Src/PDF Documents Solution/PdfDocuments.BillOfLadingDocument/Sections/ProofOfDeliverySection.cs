using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace PdfDocument.BillOfLadingDocument
{
	public class ProofOfDeliverySection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodySmallFont = gridPage.BodyFont().WithSize(gridPage.Theme.FontSize.BodySmall);
			XFont bodyEmphasisSmallFont = gridPage.BodyFont(XFontStyle.Bold).WithSize(gridPage.Theme.FontSize.BodySmall);
			IPdfSize bodyLightSmallFontSize = gridPage.MeasureText(bodySmallFont, model.Shipper.Name);

			// ***
			// *** Draw the signature line.
			// ***
			int top = this.ActualBounds.TopRow + this.Padding.Top;

			// ***
			// *** Shipper Per
			// ***
			int left = this.ActualBounds.LeftColumn;
			gridPage.DrawText("Shipper, Per", bodySmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			//
			// Add the name of person who signed for the delivery.
			//
			if (!string.IsNullOrWhiteSpace(model.DeliveryReceivedBy))
			{
				left = this.ActualBounds.LeftColumn + 20;
				gridPage.DrawText(model.DeliveryReceivedBy, bodyEmphasisSmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyEmphasisColor);
			}

			left = this.ActualBounds.RightColumn - 40;
			gridPage.DrawText("Date:", bodySmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			//
			// Add delivery date.
			//
			if (model.Delivered.HasValue)
			{
				left = this.ActualBounds.RightColumn - 20;
				gridPage.DrawText(model.Delivered.Value.ToShortDateString(), bodyEmphasisSmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyEmphasisColor);
			}

			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyBoldColor);

			// ***
			// *** Carrier Per
			// ***
			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawText("Carrier, Per", bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			left = this.ActualBounds.RightColumn - 40;
			gridPage.DrawText("Date:", bodySmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyBoldColor);

			// ***
			// *** Hazardous materials
			// ***
			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawWrappingText(WellKnown.Strings.PodLegalText, bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, this.ActualBounds.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor, XParagraphAlignment.Justify);

			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyBoldColor);

			// ***
			// *** Destination Receipt
			// ***
			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawWrappingText("Destination Receipt: In good order", bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, this.ActualBounds.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor, XParagraphAlignment.Justify);

			left = this.ActualBounds.RightColumn - 60;
			gridPage.DrawText("Damage", bodySmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyBoldColor);

			// ***
			// *** Signature
			// ***
			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawWrappingText("Details noted on face of bill of lading Per", bodySmallFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, this.ActualBounds.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor, XParagraphAlignment.Justify);

			left = this.ActualBounds.RightColumn - 40;
			gridPage.DrawText("Date:", bodySmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyBoldColor);

			top += bodyLightSmallFontSize.Rows + this.Padding.Top;
			gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Bottom, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyBoldColor);


			return Task.FromResult(returnValue);
		}
	}
}
