using System.Linq;
using System.Threading.Tasks;
using PdfDocument.Abstractions;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class ConsigneeServicesSection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyLightSmallFont = gridPage.BodyLightFont().WithSize(gridPage.Theme.FontSize.BodySmall);
			IPdfSize bodyLightSmallFontSize = gridPage.MeasureText(bodyLightSmallFont, model.Shipper.Name);

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont, model.Shipper.Name);

			// ***
			// *** Draw the address.
			// ***
			int top = this.ActualBounds.TopRow;
			gridPage.DrawText("DELIVERY SERVICES", bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			if (model.Accessorials.Any())
			{
				// ***
				// *** Get the widest item.
				// ***
				int widest = 0;

				foreach (var accessorial in model.Accessorials)
				{
					IPdfSize size = gridPage.MeasureText(bodyLightSmallFont, accessorial.Description);

					if (size.Columns > widest)
					{
						widest = size.Columns;
					}
				}

				int i = 1;
				int left = 0;
				int maxRows = 6;

				foreach (AccessorialResponse accessorial in model.Accessorials.Take(maxRows * 2))
				{
					if (i == (maxRows + 1))
					{
						// ***
						// *** Reset the top.
						// ***
						top = this.ActualBounds.TopRow + this.Padding.Top;
					}

					if (i <= maxRows)
					{
						left = this.ActualBounds.LeftColumn;
					}
					else
					{
						left = this.ActualBounds.LeftColumn + (widest + (3 * this.Padding.Left));
					}

					top += bodyLightSmallFontSize.Rows + this.Padding.Top;
					gridPage.DrawText($"[{i}] {accessorial.Description}", bodyLightSmallFont, left, top, this.ActualBounds.Columns, bodyLightSmallFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);
					i++;
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}
