/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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
using System.Linq;
using System.Threading.Tasks;

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
			return Task.FromResult(new PdfGrid(this.PageWidth(page), this.PageHeight(page), 400, 160));
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
			//
			// Build the styles.
			//
			this.StyleManager.Add("PageHeader.Section", Style.Create<Invoice>()
						.UseFont("Arial Narrow", 48)
						.UseForegroundColor(ColorPalette.Blue)
						.UseBorderColor(ColorPalette.Red)
						.UseBorderWidth(1)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UsePadding(0, 0, 2, 0)
						.UseRelativeHeight(.12)
						.Build());

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
						.UseFont("Times New Roman", 11.75, XFontStyle.Bold)
						.UsePadding(1, 1, 1, 1)
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseRelativeWidths(.5)
						.Build());

			this.StyleManager.Add("InvoiceNumber.Value", Style.Copy(this.StyleManager.GetStyle("InvoiceNumber.Key"))
						.UseFont("Times New Roman", 11.75, XFontStyle.Regular)
						.Build());

			//
			// Reference numbers
			//
			this.StyleManager.Add("Reference.Section", Style.Create<Invoice>()
						.UseRelativeHeight(.065)
						.Build());

			this.StyleManager.Add("Reference.Header.1", Style.Create<Invoice>()
						.UseFont("Arial", 11, XFontStyle.Regular)
						.UsePadding(1, 2, 1, 2)
						.UseMargin(0, 0, 1, 0)
						.UseForegroundColor(ColorPalette.White)
						.UseBackgroundColor(ColorPalette.Blue)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			this.StyleManager.Add("Reference.Header.2", Style.Copy(this.StyleManager.GetStyle("Reference.Header.1"))
						.UseMargin(1, 0, 1, 0)
						.Build());

			this.StyleManager.Add("Reference.Header.3", Style.Copy(this.StyleManager.GetStyle("Reference.Header.1"))
						.UseMargin(1, 0, 0, 0)
						.Build());

			this.StyleManager.Add("Reference.Body", Style.Create<Invoice>()
						.UseFont("Arial", 11, XFontStyle.Regular)
						.UseBorderColor(ColorPalette.Blue)
						.UseBorderWidth(1)
						.UseCellPadding(1, 2, 1, 2)
						.UseForegroundColor(ColorPalette.Red)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			//
			// Bill to section
			//
			this.StyleManager.Add("BillTo.Section", Style.Create<Invoice>()
						.UseMargin(0, 5, 0, 5)
						.UseRelativeHeight(.16)
						.Build());

			this.StyleManager.Add("BillTo.ContentBlock", Style.Create<Invoice>()
						.UseBorderColor(ColorPalette.Blue)
						.UseBorderWidth(1)
						.Build());

			this.StyleManager.Add("BillTo.Content", Style.Create<Invoice>()
						.UseFont("Arial", 11, XFontStyle.Regular)
						.UsePadding(0, 0, 0, 0)
						.UseMargin(0, 2, 0, 2)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			this.StyleManager.Add("BillTo.Header.Left", Style.Create<Invoice>()
						.UseFont("Arial", 11, XFontStyle.Regular)
						.UsePadding(1, 3, 1, 3)
						.UseMargin(0, 2, 1, 2)
						.UseBackgroundColor(ColorPalette.Red)
						.UseForegroundColor(ColorPalette.White)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			this.StyleManager.Add("BillTo.Header.Right", Style.Copy(this.StyleManager.GetStyle("BillTo.Header.Left"))
						.UseMargin(1, 2, 0, 2)
						.Build());

			this.StyleManager.Add("BillTo.Key", Style.Create<Invoice>()
						.UseFont("Arial", 11, XFontStyle.Regular)
						.UsePadding(1, 1, 1, 1)
						.UseForegroundColor(ColorPalette.LightGray)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseRelativeWidths(.3)
						.Build());

			this.StyleManager.Add("BillTo.Value", Style.Copy(this.StyleManager.GetStyle("BillTo.Key"))
						.UseFont("Arial", 11, XFontStyle.Bold)
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.Build());

			//
			// Invoice details
			//
			this.StyleManager.Add("InvoiceDetails.Section", Style.Create<Invoice>()
						.UseMargin(0, 2, 0, 2)
						.Build());

			this.StyleManager.Add("InvoiceDetails.Header.Item", Style.Create<Invoice>()
						.UseFont("Times New Roman", 13, XFontStyle.Bold)
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
						.UseFont("Times New Roman", 13, XFontStyle.Regular)
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
						.UseFont("Arial", 11.75, XFontStyle.Regular)
						.UseMargin(1, 1, 0, 1)
						.UsePadding(1, 1, 1, 1)
						.UseCellPadding(2, 2, 2, 2)
						.UseForegroundColor(ColorPalette.Blue)
						.UseBackgroundColor(ColorPalette.LightRed)
						.UseTextAlignment(XStringFormats.CenterRight)
						.UseRelativeWidths(.45)
						.Build());

			this.StyleManager.Add("Totals.Value", Style.Copy(this.StyleManager.GetStyle("Totals.Key"))
						.UseFont("Arial", 11.75, XFontStyle.Bold)
						.Build());

			//
			// Signature
			//
			this.StyleManager.Add("Signature.Section", Style.Create<Invoice>()
						.UseFont("Arial", 10, XFontStyle.Regular)
						.UseBorderWidth(1)
						.UseForegroundColor(ColorPalette.Gray)
						.UseTextAlignment(XStringFormats.CenterLeft)
						.UseRelativeWidths(.4)
						.UseRelativeHeight(.05)
						.Build());

			//
			// Tag line
			//
			this.StyleManager.Add("ThankYou.Section", Style.Create<Invoice>()
						.UseFont("Arial", 12, XFontStyle.Italic)
						.UsePadding(0, 2, 1, 2)
						.UseMargin(0, 3, 0, 0)
						.UseForegroundColor(ColorPalette.Red)
						.UseTextAlignment(XStringFormats.Center)
						.UseRelativeHeight(.058)
						.Build());

			//
			// Footer
			//
			this.StyleManager.Add("PageFooter.Section", Style.Create<Invoice>()
						.UseFont("Arial Narrow", 7, XFontStyle.Regular)
						.UseRelativeHeight(.035)
						.UseMargin(0, 5, 0, 0)
						.UseForegroundColor(ColorPalette.LightGray)
						.Build());

			return Task.CompletedTask;
		}

		protected override Task<IPdfSection<Invoice>> OnAddContentAsync()
		{
			return Task.FromResult(Pdf.VerticalStackSection<Invoice>
			(
				//
				// Page header - shows on every page.
				//
				Pdf.PageHeaderSection<Invoice>()
				   .WithTitle("INVOICE")
				   .WithLogo("./Images/logo.jpg")
				   .WithStyles("PageHeader.Section"),

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
						.WithStyles("Reference.Header.1")
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.PaymentMethod)
											   .WithStyles("Reference.Body")),

					Pdf.HeaderContentSection<Invoice>()
						.WithText("Check Number")
						.WithStyles("Reference.Header.2")
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.CheckNumber)
											   .WithStyles("Reference.Body")),

					Pdf.HeaderContentSection<Invoice>()
						.WithText("Job Number")
						.WithStyles("Reference.Header.3")
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.JobNumber)
											   .WithStyles("Reference.Body"))
				).WithStyles("Reference.Section"),

				//
				// Bill to/from.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithText("Bill To")
						.WithStyles("BillTo.Header.Left")
						.WithContentSection
						(
							Pdf.OverlayStackSection<Invoice>
							(
								Pdf.KeyValueSection<Invoice>
								(
									new PdfKeyValueItem<Invoice>("Name:", (g, m) => m.BillTo.Name),
									new PdfKeyValueItem<Invoice>("Address:", (g, m) => m.BillTo.AddressLine),
									new PdfKeyValueItem<Invoice>("City/State/Zip:", (g, m) => m.BillTo.CityStateZip),
									new PdfKeyValueItem<Invoice>("Phone:", (g, m) => m.BillTo.Phone)
								).WithStyles("BillTo.Content", "BillTo.Key", "BillTo.Value")
							).WithStyles("BillTo.ContentBlock")
						),

					Pdf.HeaderContentSection<Invoice>()
					   .WithText("From")
					   .WithStyles("BillTo.Header.Right")
					   .WithContentSection
					   (
							Pdf.OverlayStackSection<Invoice>
							(
								Pdf.KeyValueSection<Invoice>
								(
									new PdfKeyValueItem<Invoice>("Name:", (g, m) => m.BillFrom.Name),
									new PdfKeyValueItem<Invoice>("Address:", (g, m) => m.BillFrom.AddressLine),
									new PdfKeyValueItem<Invoice>("City/State/Zip:", (g, m) => m.BillFrom.CityStateZip),
									new PdfKeyValueItem<Invoice>("Phone:", (g, m) => m.BillFrom.Phone)
								).WithStyles("BillTo.Content", "BillTo.Key", "BillTo.Value")
							).WithStyles("BillTo.ContentBlock")
						)
				).WithStyles("BillTo.Section"),

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
				   .WithText("Approved by")
				   .WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount)
				   .WithStyles("Signature.Section"),

				//
				// Tag line.
				//
				Pdf.TextBlockSection<Invoice>()
					.WithText("Thank you for your business!")
					.WithStyles("ThankYou.Section"),

				//
				//
				//
				Pdf.PageFooterSection<Invoice>()
					.WithTopLeftText("Contact us immediately with any questions")
					.WithTopRightText((g, m) => $"Page {g.PageNumber} of {g.Document.PageCount}")
					.WithBottomLeftText("Copyright Daniel Porrey. All right reserved.")
					.WithBottomRightText((g, m) => $"Invoiced: {m.InvoiceDate.ToLongDateString()}")
					.WithStyles("PageFooter.Section")
			)
			.WithKey("Report")
			.WithWatermark((g, m) => m.Paid ? "./images/paid.png" : string.Empty));
		}
	}
}