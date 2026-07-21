/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocuments.Example.Invoice
{
	public class InvoicePdf : PdfGenerator<Invoice>
	{
		public InvoicePdf(IPdfStyleManager<Invoice> styleManager)
			: base(styleManager)
		{
		}

		protected override Task<PdfGrid> OnSetPageGridAsync(PdfPage page)
		{
			return Task.FromResult(new PdfGrid(this.PageWidth(page).Point, this.PageHeight(page).Point, 400, 160));
		}

		protected override Task<string> OnGetDocumentTitleAsync(Invoice model)
		{
			return Task.FromResult("Invoice");
		}

		protected override Task<int> OnGetPageCountAsync(Invoice model)
		{
			return Task.FromResult(1);
		}

		protected override Task OnInitializeStylesAsync(IPdfStyleManager<Invoice> styleManager)
		{
			#region Report Header
			//
			// Build the styles.
			//
			this.StyleManager.Add("ReportHeader", Style.Create<Invoice>()
				.UseBorderColor(XColors.Red)
				.UseBorderWidth(1)
				.UseMargin(1, 1, 1, 1)
				.UseRelativeHeight(.12)
				.Build());

			this.StyleManager.Add("ReportHeader.Logo", Style.Create<Invoice>()
				.UseHorizontalImageAlignment(PdfHorizontalAlignment.Left)
				.UseVerticalImageAlignment(PdfVerticalAlignment.Center)
				.UsePadding(1, 0, 0, 0)
				.UseImageScale(.23f)
				.Build());

			this.StyleManager.Add("ReportHeader.Title", Style.Create<Invoice>()
				.UseFont("Open Sans", 48)
				.UseForegroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterRight)
				.UsePadding(0, 0, 2, 0)
				.Build());
			#endregion

			#region Invoice Number and date
			//
			// Invoice Number and date
			//
			this.StyleManager.Add("InvoiceNumber.Section", Style.Create<Invoice>()
						.UseRelativeHeight(.11)
						.UseMargin(0, 3, 0, 3)
						.Build());

			this.StyleManager.Add("InvoiceNumber", Style.Create<Invoice>()
						.UseMargin(0, 2, 0, 2)
						.UseRelativeWidths(.53)
						.Build());

			this.StyleManager.Add("InvoiceNumber.Key", Style.Create<Invoice>()
						.UseFont("Tinos", 11.75, XFontStyleEx.Bold)
						.UsePadding(1, 1, 1, 1)
						.UseForegroundColor(XColors.Black.WithLuminosity(.3))
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseRelativeWidths(.5)
						.Build());

			this.StyleManager.Add("InvoiceNumber.Value", Style.Copy(this.StyleManager.GetStyle("InvoiceNumber.Key"))
						.UseFont("Tinos", 11.75, XFontStyleEx.Regular)
						.Build());
			#endregion

			#region References
			//
			// Reference numbers
			//
			this.StyleManager.Add("Reference.Section", Style.Create<Invoice>()
				.UseRelativeHeight(.15)
				.Build());

			#region Payment Method
			this.StyleManager.Add("PaymentMethod", Style.Create<Invoice>()
				.UseRelativeWidths(.333333333)
				.UseMargin(0, 0, 1, 0)
				.Build());

			this.StyleManager.Add("PaymentMethod.Header", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.White)
				.UseBackgroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("PaymentMethod.Container", Style.Create<Invoice>()
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("PaymentMethod.Body", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseBorderColor(ColorPalette.Blue)
				.UseBorderWidth(1)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.Red)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());
			#endregion

			#region Check Number
			this.StyleManager.Add("CheckNumber", Style.Create<Invoice>()
				.UseRelativeWidths(.333333333)
				.Build());

			this.StyleManager.Add("CheckNumber.Header", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.White)
				.UseBackgroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("CheckNumber.Container", Style.Create<Invoice>()
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("CheckNumber.Body", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseBorderColor(ColorPalette.Blue)
				.UseBorderWidth(1)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.Red)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());
			#endregion

			#region Work Order
			this.StyleManager.Add("WorkOrder", Style.Create<Invoice>()
				.UseRelativeWidths(.333333333)
				.UseMargin(1, 0, 0, 0)
				.Build());

			this.StyleManager.Add("WorkOrder.Header", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.White)
				.UseBackgroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("WorkOrder.Container", Style.Create<Invoice>()
				.UseRelativeHeight(.5)
				.Build());

			this.StyleManager.Add("WorkOrder.Body", Style.Create<Invoice>()
				.UseFont("Open Sans", 17, XFontStyleEx.Regular)
				.UseBorderColor(ColorPalette.Blue)
				.UseBorderWidth(1)
				.UseCellPadding(2, 2, 2, 2)
				.UseForegroundColor(ColorPalette.Red)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());
			#endregion
			#endregion

			#region Bill To/From
			//
			// Bill To/From section
			//
			this.StyleManager.Add("BillToFrom.Section", Style.Create<Invoice>()
						.UseMargin(0, 5, 0, 5)
						.UseRelativeHeight(.40)
						.Build());

			#region Bill To
			this.StyleManager.Add("BillTo.Frame", Style.Create<Invoice>()
						.UseMargin(0, 0, 1, 0)
						.UseRelativeWidths(.5)
						.Build());

			this.StyleManager.Add("BillTo.Header", Style.Create<Invoice>()
						.UseFont("Open Sans", 14, XFontStyleEx.Regular)
						.UseCellPadding(1, 3, 1, 3)
						.UseBackgroundColor(ColorPalette.Red)
						.UseForegroundColor(ColorPalette.White)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.UseRelativeHeight(.15)
						.Build());

			this.StyleManager.Add("BillTo.ContentBlock", Style.Create<Invoice>()
						.UseBorderColor(ColorPalette.Blue)
						.UseBorderWidth(1)
						.UseRelativeHeight(.85)
						.Build());

			this.StyleManager.Add("BillTo.Content", Style.Create<Invoice>()
						.Build());
			#endregion

			#region Bill From
			this.StyleManager.Add("BillFrom.Frame", Style.Create<Invoice>()
						.UseMargin(1, 0, 0, 0)
						.UseRelativeWidths(.5)
						.Build());

			this.StyleManager.Add("BillFrom.Header", Style.Create<Invoice>()
						.UseFont("Open Sans", 14, XFontStyleEx.Regular)
						.UseCellPadding(1, 3, 1, 3)
						.UseBackgroundColor(ColorPalette.Red)
						.UseForegroundColor(ColorPalette.White)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.UseRelativeHeight(.15)
						.Build());

			this.StyleManager.Add("BillFrom.ContentBlock", Style.Create<Invoice>()
						.UseBorderColor(ColorPalette.Blue)
						.UseBorderWidth(1)
						.UseRelativeHeight(.85)
						.Build());

			this.StyleManager.Add("BillFrom.Content", Style.Create<Invoice>()
						.Build());
			#endregion

			this.StyleManager.Add("BillToFrom.Key", Style.Create<Invoice>()
						.UseFont("Open Sans", 10.5, XFontStyleEx.Regular)
						.UseCellPadding(2, 2, 2, 2)
						.UseForegroundColor(ColorPalette.LightGray)
						.UseTextAlignment(XStringFormats.CenterRight)
						.Build());

			this.StyleManager.Add("BillToFrom.Value", Style.Copy(this.StyleManager.GetStyle("BillTo.Key"))
						.UseFont("Open Sans", 9.5, XFontStyleEx.Bold)
						.UseCellPadding(2, 2, 2, 2)
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());
			#endregion

			//
			// Invoice details
			//
			this.StyleManager.Add("InvoiceDetails.Section", Style.Create<Invoice>()
						.UseMargin(0, 2, 0, 2)
						.UseRelativeHeight(.25)
						.Build());

			this.StyleManager.Add("InvoiceDetails.Header.Item", Style.Create<Invoice>()
						.UseFont("Tinos", 13, XFontStyleEx.Bold)
						.UseMargin(0, 1, 0, 1)
						.UsePadding(0, 2, 1, 2)
						.UseCellPadding(1, 1, 1, 1)
						.UseForegroundColor(ColorPalette.Red)
						.UseBackgroundColor(ColorPalette.MediumRed)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			this.StyleManager.Add("InvoiceDetails.Header.Other", Style.Copy(this.StyleManager.GetStyle("InvoiceDetails.Header.Item"))
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseMargin(1, 1, 0, 1)
						.Build());

			this.StyleManager.Add("InvoiceDetails.Body.Left", Style.Create<Invoice>()
						.UseFont("Tinos", 13, XFontStyleEx.Regular)
						.UseMargin(0, 1, 0, 1)
						.UsePadding(0, 2, 1, 2)
						.UseCellPadding(1, 1, 1, 1)
						.UseForegroundColor(ColorPalette.Blue)
						.UseBackgroundColor(ColorPalette.MediumBlue)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			this.StyleManager.Add("InvoiceDetails.Body.Other", Style.Copy(this.StyleManager.GetStyle("InvoiceDetails.Body.Left"))
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseMargin(1, 1, 0, 1)
						.Build());

			//
			// Invoice totals
			//
			this.StyleManager.Add("Totals.Section", Style.Create<Invoice>()
						.UseRelativeHeight(.1)
						.Build());

			this.StyleManager.Add("Totals", Style.Create<Invoice>()
						.UsePadding(0, 2, 0, 2)
						.UseRelativeWidths(.45)
						.Build());

			this.StyleManager.Add("Totals.Key", Style.Create<Invoice>()
						.UseFont("Open Sans", 11.75, XFontStyleEx.Regular)
						.UseMargin(1, 1, 0, 1)
						.UsePadding(1, 1, 1, 1)
						.UseCellPadding(2, 2, 2, 2)
						.UseForegroundColor(ColorPalette.Blue)
						.UseBackgroundColor(ColorPalette.LightRed)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseRelativeWidths(.45)
						.Build());

			this.StyleManager.Add("Totals.Value", Style.Copy(this.StyleManager.GetStyle("Totals.Key"))
						.UseFont("Open Sans", 11.75, XFontStyleEx.Bold)
						.Build());

			//
			// Signature
			//
			this.StyleManager.Add("Signature.Section", Style.Create<Invoice>()
						.UseFont("Open Sans", 10, XFontStyleEx.Regular)
						.UseBorderWidth(1)
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.UseRelativeWidths(.4)
						.UseRelativeHeight(.02)
						.Build());

			//
			// Tag line
			//
			this.StyleManager.Add("ThankYou.Section", Style.Create<Invoice>()
						.UseFont("Tinos", 14, XFontStyleEx.Italic)
						.UsePadding(0, 2, 1, 2)
						.UseMargin(0, 3, 0, 0)
						.UseForegroundColor(ColorPalette.Red)
						.UseTextAlignment(XStringFormats.Center)
						.UseRelativeHeight(.02)
						.Build());

			//
			// Footer
			//
			this.StyleManager.Add("Footer", Style.Create<Invoice>()
				.UseRelativeHeight(.01)
				.UseMargin(0, 1, 0, 1)
				.Build());

			this.StyleManager.Add("Footer.TopLeft", Style.Create<Invoice>()
				.UseFont("Open Sans", 8, XFontStyleEx.Regular)
				.UsePadding(1, 0, 0, 0)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseForegroundColor(ColorPalette.LightGray)
				.Build());

			this.StyleManager.Add("Footer.TopRight", Style.Create<Invoice>()
				.UseFont("Open Sans", 8, XFontStyleEx.Regular)
				.UsePadding(0, 0, 1, 0)
				.UseTextAlignment(XStringFormats.CenterRight)
				.UseForegroundColor(ColorPalette.LightGray)
				.Build());

			this.StyleManager.Add("Footer.BottomLeft", Style.Create<Invoice>()
				.UseFont("Open Sans", 8, XFontStyleEx.Regular)
				.UseForegroundColor(XColors.Blue)
				.UsePadding(1, 0, 0, 0)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseForegroundColor(ColorPalette.LightGray)
				.Build());

			this.StyleManager.Add("Footer.BottomRight", Style.Create<Invoice>()
				.UseFont("Open Sans", 8, XFontStyleEx.Regular)
				.UseForegroundColor(XColors.Red)
				.UsePadding(0, 0, 1, 0)
				.UseTextAlignment(XStringFormats.CenterRight)
				.UseForegroundColor(ColorPalette.LightGray)
				.Build());

			return Task.CompletedTask;
		}

		protected override Task<IPdfSection<Invoice>> OnAddContentAsync()
		{
			return Task.FromResult(Pdf.VerticalStackSection<Invoice>
			(
				//
				// Page header.
				//
				Pdf.ReportHeaderSection<Invoice>()
					.WithTitle("INVOICE")
					.WithLogo("./Images/logo.jpg")
					.WithLogoPosition(PdfLogoPosition.Left)
					.WithStyles("ReportHeader", "ReportHeader.Logo", "ReportHeader.Title"),

				//
				// Invoice number and date.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>(),
					Pdf.KeyValueSection<Invoice>
					(
						new PdfKeyValueItem<Invoice>("Invoice Number:", (g, m) => $"{m.Id:00000000}"),
						new PdfKeyValueItem<Invoice>("Invoice Date:", (g, m) => m.InvoiceDate.ToLongDateString()),
						new PdfKeyValueItem<Invoice>("Terms:", (g, m) => m.Terms),
						new PdfKeyValueItem<Invoice>("Invoice Due Date:", (g, m) => m.DueDate.ToLongDateString())
					)
					.WithStyles("InvoiceNumber", "InvoiceNumber.Key", "InvoiceNumber.Value")
				).WithStyles("InvoiceNumber.Section"),

				//
				// Reference numbers section.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithText("Payment Method")
						.WithStyles("PaymentMethod", "PaymentMethod.Header", "PaymentMethod.Container")
						.WithContentSection(
							Pdf.TextBlockSection<Invoice>()
								.WithText((g, m) => m.PaymentMethod)
								.WithStyles("PaymentMethod.Body")
						),

					Pdf.HeaderContentSection<Invoice>()
						.WithText("Check Number")
						.WithStyles("CheckNumber", "CheckNumber.Header", "CheckNumber.Container")
						.WithContentSection(
							Pdf.TextBlockSection<Invoice>()
								.WithText((g, m) => m.CheckNumber)
								.WithStyles("CheckNumber.Body")
						),

					Pdf.HeaderContentSection<Invoice>()
						.WithText("Work Order")
						.WithStyles("WorkOrder", "WorkOrder.Header", "WorkOrder.Container")
						.WithContentSection(
							Pdf.TextBlockSection<Invoice>()
								.WithText((g, m) => m.JobNumber)
								.WithStyles("WorkOrder.Body")
						)
				).WithStyles("Reference.Section"),

				//
				// Bill to/from.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithText("Bill To")
						.WithContentSection(
							Pdf.KeyValueSection<Invoice>(
								new PdfKeyValueItem<Invoice>("Name:", (g, m) => m.BillTo.Name),
								new PdfKeyValueItem<Invoice>("Address:", (g, m) => m.BillTo.AddressLine),
								new PdfKeyValueItem<Invoice>("City/State/Zip:", (g, m) => m.BillTo.CityStateZip),
								new PdfKeyValueItem<Invoice>("Phone:", (g, m) => m.BillTo.Phone)
							).WithStyles("BillTo.Content", "BillToFrom.Key", "BillToFrom.Value")
						).WithStyles("BillTo.Frame", "BillTo.Header", "BillTo.ContentBlock"),

					Pdf.HeaderContentSection<Invoice>()
					   .WithText("Bill From")
					   .WithContentSection
					   (
							Pdf.KeyValueSection<Invoice>
							(
								new PdfKeyValueItem<Invoice>("Name:", (g, m) => m.BillFrom.Name),
								new PdfKeyValueItem<Invoice>("Address:", (g, m) => m.BillFrom.AddressLine),
								new PdfKeyValueItem<Invoice>("City/State/Zip:", (g, m) => m.BillFrom.CityStateZip),
								new PdfKeyValueItem<Invoice>("Phone:", (g, m) => m.BillFrom.Phone)
							).WithStyles("BillFrom.Content", "BillToFrom.Key", "BillToFrom.Value")
						).WithStyles("BillFrom.Frame", "BillFrom.Header", "BillFrom.ContentBlock")
				).WithStyles("BillToFrom.Section"),

				//
				// Invoice details
				//
				Pdf.DataGridSection<Invoice, InvoiceItem>()
				   .AddColumn<Invoice, InvoiceItem, int>("Item Number", t => t.Id, .45, "{0:1000000000000}", "InvoiceDetails.Header.Item", "InvoiceDetails.Body.Left")
				   .AddColumn<Invoice, InvoiceItem, int>("Quantity", t => t.Quantity, .25, "{0:#,###}", "InvoiceDetails.Header.Other", "InvoiceDetails.Body.Other")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Unit Price", t => t.UnitPrice, .25, "{0:C}", "InvoiceDetails.Header.Other", "InvoiceDetails.Body.Other")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Amount", t => t.Amount, .15, "{0:C}", "InvoiceDetails.Header.Other", "InvoiceDetails.Body.Other")
				   .UseItems((g, m) => m.Items)
				   .WithStyles("InvoiceDetails.Section"),

				//
				// Invoice totals
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>(),
					Pdf.KeyValueSection<Invoice>
						(
							new PdfKeyValueItem<Invoice>("Sub Total:", (g, m) => m.Items.Sum(t => t.Amount).ToString("C")),
							new PdfKeyValueItem<Invoice>("Tax (6.0%):", (g, m) => (m.Items.Sum(t => t.Amount) * .06M).ToString("C")),
							new PdfKeyValueItem<Invoice>("Total:", (g, m) => (m.Items.Sum(t => t.Amount) * 1.06M).ToString("C")))
						.WithStyles("Totals", "Totals.Key", "Totals.Value")
				).WithStyles("Totals.Section"),

				//
				// Signature section will display only on the last page.
				//
				Pdf.SignatureSection<Invoice>()
					.WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount)
					.WithStyles("Signature.Section")
					.WithSignatureOptions(new SignatureOptions<Invoice>()
					{
						SignatureText = "Approved By",
						//SignatureImage = "./Images/signature.jpg",
						DateLabel = "Date",
						Date = DateTimeOffset.Now
					}),

				//
				// Tag line.
				//
				Pdf.TextBlockSection<Invoice>()
					.WithText("Thank you for your business!")
					.WithStyles("ThankYou.Section"),

				//
				// Page footer.
				//
				Pdf.PageFooterSection<Invoice>()
					.WithTopLeftText("Contact us immediately with any questions")
					.WithTopRightText((g, m) => $"Page {g.PageNumber} of {g.Document.PageCount}")
					.WithBottomLeftText("Copyright Daniel Porrey. All rights reserved.")
					.WithBottomRightText((g, m) => $"Invoiced: {m.InvoiceDate:D}")
					.WithStyles("Footer", "Footer.TopLeft", "Footer.TopRight", "Footer.BottomLeft", "Footer.BottomRight")
			)
			.WithStyleManager(this.StyleManager)
			.WithKey("Report")
			.WithWatermark((g, m) => m.Paid ? "./images/paid.png" : string.Empty));
		}
	}
}