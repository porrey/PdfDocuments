using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfDocument.Abstractions;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class PackageDetailSection : PdfSection<BillOfLading>
	{
		public PackageDetailSection(PageCalculator<LineItem> pageCalculator)
		{
			this.PageCalculator = pageCalculator;
		}

		protected PageCalculator<LineItem> PageCalculator { get; set; }

		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			// ***
			// *** Get the list that corresponds to the current page.
			// ***
			IEnumerable<LineItem> lineItems = this.PageCalculator.PartitionedItems[gridPage.PageNumber - 1];

			if (lineItems.Count() > 0)
			{
				// ***
				// *** Divide the width of the section up into 6 columns.
				// ***
				int[] columnWidths = new int[]
				{
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.10),
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.10),
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.10),
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.40),
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.15),
					this.ActualBounds.LeftColumn + (int)(this.ActualBounds.Columns *.15),
				};

				// ***
				// *** Get the height of the text.
				// ***
				IPdfSize textSize = gridPage.MeasureText(gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, "Pallets");

				// ***
				// *** Get the top row for the first line of text.
				// ***
				int top = this.ActualBounds.TopRow + this.Padding.Bottom + this.Padding.Top;
				int left = this.ActualBounds.LeftColumn;

				// ***
				// *** Draw the columns headers.
				// ***
				gridPage.DrawText("Pallets", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[0], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				left += columnWidths[0] + this.Padding.Left;
				gridPage.DrawText("Freight Class", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[1], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				left += columnWidths[1] + this.Padding.Left;
				gridPage.DrawText("NMFC Class", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[2], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				left += columnWidths[2] + this.Padding.Left;
				gridPage.DrawText("NMFC Description", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[3], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				left += columnWidths[3] + this.Padding.Left;
				gridPage.DrawText("Weight", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[4], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				left += columnWidths[4] + this.Padding.Left;
				gridPage.DrawText("Stackable", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Bold, left, top, columnWidths[4], textSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyLightColor);

				// ***
				// *** Display the data.
				// ***
				top += textSize.Rows + this.Padding.Top;

				// ***
				// ***
				// ***
				gridPage.DrawHorizontalLine(top, this.ActualBounds.LeftColumn, this.ActualBounds.RightColumn, RowEdge.Top, gridPage.Theme.Drawing.LineWeight, gridPage.Theme.Color.BodyLightColor);

				// ***
				// *** Display each line item.
				// ***
				foreach (LineItem item in lineItems)
				{
					left = this.ActualBounds.LeftColumn;
					gridPage.DrawText($"{item.TotalPackages:#,##0}", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[1], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					left += columnWidths[0] + this.Padding.Left;
					gridPage.DrawText(item.FreightClass, gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[1], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					left += columnWidths[1] + this.Padding.Left;
					gridPage.DrawText(item.NmfcClass, gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[2], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					left += columnWidths[2] + this.Padding.Left;
					gridPage.DrawText(item.NmfcDescription, gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[3], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					left += columnWidths[3] + this.Padding.Left;
					gridPage.DrawText($"{item.Weight:#,##0.0} lbs", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[4], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					left += columnWidths[4] + this.Padding.Left;
					string stackable = item.Stackable ? "Yes" : "No";
					gridPage.DrawText($"{stackable}", gridPage.Theme.FontFamily.Body, gridPage.Theme.FontSize.BodySmall, XFontStyle.Regular, left, top, columnWidths[4], textSize.Rows, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

					top += textSize.Rows + this.Padding.Top;
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
