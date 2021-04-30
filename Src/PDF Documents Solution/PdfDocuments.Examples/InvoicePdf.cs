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

namespace PdfDocuments.Example
{
	public class InvoicePdf : PdfGenerator<Invoice>
	{
		public InvoicePdf(IPdfStyleManager<Invoice> styleManager)
			: base(styleManager)
		{
		}

		protected override void OnInitializeStyles(IPdfStyleManager<Invoice> styleManager)
		{
			//
			// Build the styles.
			//
			this.StyleManager.Add("PageHeader", Style.Create<Invoice>()
									.UseFont("Arial Narrow", 48)
									.UseForegroundColor(ColorPalette.Blue)
									.UseBackgroundColor(ColorPalette.Transparent)
									.UseBorderColor(ColorPalette.Red)
									.UseBorderWidth(1)
									.UseTextAlignment(XStringFormats.CenterRight)
									.UsePadding(0, 0, 2, 0)
									.UseRelativeHeight(.12)
									.Build());

			this.StyleManager.Add("InvoiceNumber", Style.Create<Invoice>()
									.UseMargin(0, 2, 0, 2)
									.UseRelativeHeight(.095)
									.UseRelativeWidths(.53)
									.Build());

			this.StyleManager.Add("InvoiceNumber.Key", Style.Create<Invoice>()
									.UseFont("Times New Roman", 11.75, XFontStyle.Bold)
									.UseForegroundColor(ColorPalette.Gray)
									.UseTextAlignment(XStringFormats.CenterRight)
									.UseRelativeWidths(.5)
									.Build());

			this.StyleManager.Add("InvoiceNumber.Value", Style.Copy(this.StyleManager.GetStyle("InvoiceNumber.Key"))
									.UseFont("Times New Roman", 11.75, XFontStyle.Regular)
									.Build());

			this.StyleManager.Add("Address.Section", Style.Create<Invoice>()
									.UseMargin(0, 5, 0, 5)
									.Build());

			this.StyleManager.Add("Address.ContentBlock", Style.Create<Invoice>()
									.UseFont("Arial", 11, XFontStyle.Regular)
									.UsePadding(1, 2, 1, 2)
									.UseBorderColor(ColorPalette.Blue)
									.UseBorderWidth(1)
									.UseBackgroundColor(ColorPalette.White)
									.UseForegroundColor(ColorPalette.Transparent)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("Address.Header.Left", Style.Create<Invoice>()
									.UseFont("Arial", 11, XFontStyle.Regular)
									.UsePadding(1, 3, 1, 3)
									.UseMargin(0, 2, 1, 2)
									.UseBackgroundColor(ColorPalette.Red)
									.UseForegroundColor(ColorPalette.White)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("Address.Header.Right", Style.Copy(this.StyleManager.GetStyle("Address.Header.Left"))
									.UseMargin(1, 2, 0, 2)
									.Build());

			this.StyleManager.Add("Address.Key", Style.Create<Invoice>()
									.UseFont("Arial", 11, XFontStyle.Regular)
									.UsePadding(1, 1, 1, 1)
									.UseForegroundColor(ColorPalette.Gray)
									.UseTextAlignment(XStringFormats.CenterRight)
									.UseRelativeWidths(.3)
									.Build());

			this.StyleManager.Add("Address.Value", Style.Copy(this.StyleManager.GetStyle("Address.Key"))
									.UseFont("Arial", 11, XFontStyle.Bold)
									.Build());

			this.StyleManager.Add("Totals", Style.Create<Invoice>()
									.UsePadding(0, 2, 0, 2)
									.Build());

			this.StyleManager.Add("Totals.Key", Style.Create<Invoice>()
									.UseFont("Arial", 11.75, XFontStyle.Regular)
									.UsePadding(1, 1, 1, 1)
									.UseForegroundColor(ColorPalette.Blue)
									.UseBackgroundColor(ColorPalette.LightRed)
									.UseTextAlignment(XStringFormats.CenterRight)
									.UseRelativeWidths(.45)
									.Build());

			this.StyleManager.Add("Totals.Value", Style.Copy(this.StyleManager.GetStyle("Totals.Key"))
									.UseFont("Arial", 11, XFontStyle.Bold)
									.Build());

			this.StyleManager.Add("Reference.Header.1", Style.Create<Invoice>()
									.UseFont("Arial", 11, XFontStyle.Regular)
									.UsePadding(1, 2, 1, 2)
									.UseMargin(0, 2, 1, 2)
									.UseForegroundColor(ColorPalette.White)
									.UseBackgroundColor(ColorPalette.Blue)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("Reference.Header.2", Style.Copy(this.StyleManager.GetStyle("Reference.Header.1"))
									.UseMargin(1, 2, 1, 2)
									.Build());

			this.StyleManager.Add("Reference.Header.3", Style.Copy(this.StyleManager.GetStyle("Reference.Header.1"))
									.UseMargin(1, 2, 0, 2)
									.Build());

			this.StyleManager.Add("Reference.Body", Style.Create<Invoice>()
									.UseFont("Arial", 11, XFontStyle.Regular)
									.UseBorderColor(ColorPalette.Blue)
									.UseBorderWidth(1)
									.UsePadding(1, 2, 1, 2)
									.UseForegroundColor(ColorPalette.Red)
									.UseBackgroundColor(ColorPalette.White)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("InvoiceItems.Grid", Style.Create<Invoice>()
									.UseMargin(1, 2, 1, 2)
									.Build());

			this.StyleManager.Add("InvoiceItems.Header.Left", Style.Create<Invoice>()
									.UseFont("Times New Roman", 13, XFontStyle.Bold)
									.UsePadding(0, 2, 1, 2)
									.UseCellPadding(2, 2, 2, 2)
									.UseForegroundColor(ColorPalette.Red)
									.UseBackgroundColor(ColorPalette.MediumRed)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("InvoiceItems.Header.Right", Style.Copy(this.StyleManager.GetStyle("InvoiceItems.Header.Left"))
									.UseTextAlignment(XStringFormats.CenterRight)
									.Build());

			this.StyleManager.Add("InvoiceItems.Body.Left", Style.Create<Invoice>()
									.UseFont("Times New Roman", 13, XFontStyle.Regular)
									.UsePadding(0, 2, 1, 2)
									.UseCellPadding(2, 2, 2, 2)
									.UseForegroundColor(ColorPalette.Blue)
									.UseBackgroundColor(ColorPalette.MediumBlue)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.Build());

			this.StyleManager.Add("InvoiceItems.Body.Right", Style.Copy(this.StyleManager.GetStyle("InvoiceItems.Body.Left"))
									.UseForegroundColor(ColorPalette.Gray)
									.UseTextAlignment(XStringFormats.CenterRight)
									.Build());

			this.StyleManager.Add("Signature", Style.Create<Invoice>()
									.UseFont("Arial", 10, XFontStyle.Regular)
									.UseBorderWidth(1)
									.UseForegroundColor(ColorPalette.Gray)
									.UseTextAlignment(XStringFormats.CenterLeft)
									.UseRelativeWidths(.4)
									.Build());

			this.StyleManager.Add("ThankYou", Style.Create<Invoice>()
									.UseFont("Arial", 12, XFontStyle.Italic)
									.UsePadding(0, 2, 1, 2)
									.UseMargin(0, 15, 0, 0)
									.UseForegroundColor(ColorPalette.Red)
									.UseTextAlignment(XStringFormats.Center)
									.Build());
		}

		protected override Task<PdfGrid> OnSetPageGridAsync(PdfPage page)
		{
			return Task.FromResult(new PdfGrid(this.PageWidth(page), this.PageHeight(page), 400, 160));
		}

		protected override string OnGetDocumentTitle(Invoice model)
		{
			return "Invoice";
		}

		protected override Task<int> OnGetPageCountAsync(Invoice model)
		{
			return Task.FromResult(1);
		}

		protected override IPdfSection<Invoice> OnAddContent()
		{
			return Pdf.VerticalStackSection<Invoice>
			(
				//
				// Page header - shows on every page.
				//
				Pdf.PageHeaderSection<Invoice>()
				   .WithRelativeHeight(.12)
				   .WithTitle("INVOICE")
				   .WithLogoPath("./Images/logo.jpg")
				   .WithStyles("PageHeader"),

				//
				// Invoice number and date.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>(),
					Pdf.KeyValueSection<Invoice>
					(
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Number:", Value = new BindProperty<string, Invoice>((g, m) => $"{m.Id:00000000}") },
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Date:", Value = new BindProperty<string, Invoice>((g, m) => m.CreateDateTime.ToLongDateString()) },
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Due Date:", Value = new BindProperty<string, Invoice>((g, m) => m.DueDate.ToLongDateString()) }
					)
					.WithStyles("InvoiceNumber", "InvoiceNumber.Key", "InvoiceNumber.Value")
					.WithRelativeWidth(.53)
				).WithRelativeHeight(.095)
				 .WithStyles("InvoiceNumber"),

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
				).WithRelativeHeight(.065)
				 .WithStyles("Default"),

				//
				// Bill to/from.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithText("Bill To")
						.WithStyles("Address.Header.Left")
						.WithContentSection(Pdf.KeyValueSection<Invoice>
						(
							new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Name) },
							new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.AddressLine) },
							new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.CityStateZip) },
							new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Phone) }
						)
						.WithStyles("Address.ContentBlock", "Address.Key", "Address.Value")),

					Pdf.HeaderContentSection<Invoice>()
					   .WithText("From")
					   .WithStyles("Address.Header.Right")
					   .WithContentSection(Pdf.KeyValueSection<Invoice>
						(
							new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Name) },
							new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.AddressLine) },
							new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.CityStateZip) },
							new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Phone) }
						)
						.WithStyles("Address.ContentBlock", "Address.Key", "Address.Value"))

				).WithRelativeHeight(.16)
				 .WithStyles("Address.Section"),

				//
				// Invoice details
				//
				Pdf.DataGridSection<Invoice, InvoiceItem>()
				   .AddColumn<Invoice, InvoiceItem, int>("Item Number", t => t.Id, .20, "{0:1000000000000}", "InvoiceItems.Header.Left", "InvoiceItems.Body.Left")
				   .AddColumn<Invoice, InvoiceItem, int>("Quantity", t => t.Quantity, .25, "{0:#,###}", "InvoiceItems.Header.Right", "InvoiceItems.Body.Right")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Unit Price", t => t.UnitPrice, .25, "{0:C}", "InvoiceItems.Header.Right", "InvoiceItems.Body.Right")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Amount", t => t.Amount, .30, "{0:C}", "InvoiceItems.Header.Right", "InvoiceItems.Body.Right")
				   .UseItems((g, m) => m.Items)
				   .WithStyles("InvoiceItems.Grid"),

				//
				// Invoice totals
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>().WithRelativeWidth(.55),
					Pdf.KeyValueSection<Invoice>
						(
							new PdfKeyValueItem<Invoice>() { Key = "Sub Total:", Value = new BindProperty<string, Invoice>((g, m) => m.Items.Sum(t => t.Amount).ToString("C")) },
							new PdfKeyValueItem<Invoice>() { Key = "Tax (6.0%):", Value = new BindProperty<string, Invoice>((g, m) => (m.Items.Sum(t => t.Amount) * .06M).ToString("C")) },
							new PdfKeyValueItem<Invoice>() { Key = "Total:", Value = new BindProperty<string, Invoice>((g, m) => (m.Items.Sum(t => t.Amount) * 1.06M).ToString("C")) })
						.WithStyles("Totals", "Totals.Key", "Totals.Value")
				).WithRelativeHeight(.1),

				//
				// Signature section will display only on the last page.
				//
				Pdf.SignatureSection<Invoice>()
				   .WithRelativeHeight(.05)
				   .WithText("Approved by")
				   .WithStyles("Signature")
				   .WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount),

				//
				// Tag line.
				//
				Pdf.TextBlockSection<Invoice>()
					.WithText("Thank you for your business!")
					.WithRelativeHeight(.058)
					.WithStyles("ThankYou")
			)
			.WithKey("Report")
			.WithWatermark((g, m) => m.Paid ? "./images/paid.png" : string.Empty);
		}
	}
}