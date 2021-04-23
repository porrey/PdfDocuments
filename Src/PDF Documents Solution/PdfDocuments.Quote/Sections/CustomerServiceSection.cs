using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.QuoteDocument
{
	public class CustomerServiceSection : PdfHorizontalStackSection<Quote>
	{
		protected override Task<bool> OnRenderAsync(IPdfGridPage gridPage, Quote model)
		{
			bool returnValue = true;

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyFont = gridPage.BodyFont();
			IPdfSize bodyFontSize = gridPage.MeasureText(bodyFont);

			// ***
			// *** Use the standard body font.
			// ***
			XFont bodyMediumBoldFont = gridPage.BodyMediumFont(XFontStyle.Bold);
			IPdfSize bodyMediumBoldFontSize = gridPage.MeasureText(bodyMediumBoldFont);

			// ***
			// *** Draw the header.
			// ***
			int top = this.ActualBounds.TopRow + this.Padding.Top;
			gridPage.DrawText("Have questions about or issues with this quote? Please Email or give us a call.", bodyMediumBoldFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyMediumBoldFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyMediumBoldFontSize.Rows + this.Padding.Top;
			gridPage.DrawText($"Email: {model.Email}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			top += bodyFontSize.Rows;
			gridPage.DrawText($"Phone: {model.Phone}", bodyFont, this.ActualBounds.LeftColumn, top, this.ActualBounds.Columns, bodyFontSize.Rows, XStringFormats.TopLeft, gridPage.Theme.Color.BodyColor);

			return Task.FromResult(returnValue);
		}
	}
}
