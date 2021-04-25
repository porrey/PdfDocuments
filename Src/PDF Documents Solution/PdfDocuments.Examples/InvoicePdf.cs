using System.Threading.Tasks;
using PdfDocuments.Barcode;
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocuments.Example
{
	public class InvoicePdf : PdfGenerator<Invoice>
	{
		public InvoicePdf(ITheme theme, IBarcodeGenerator barcodeGenerator)
			: base(theme, barcodeGenerator)
		{
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
				   .WithBackgroundColor(XColors.White)
				   .WithForegroundColor(XColor.FromArgb(37, 32, 98))
				   .WithFont((g, m) => new XFont(g.Theme.FontFamily.TitleLight, 44))
				   .WithBorderWidth(1)
				   .WithBorderColor(XColor.FromArgb(215, 35, 44))
				   .WithPadding((0, 0, 2, 0)),

				//
				// Invoice number and date.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>(),
					Pdf.KeyValueSection<Invoice>
					(
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Number:", Value = new BindProperty<string, Invoice>((g, m) => $"{m.Id:00000000}"), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterRight },
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Date:", Value = new BindProperty<string, Invoice>((g, m) => m.CreateDateTime.ToLongDateString()), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterRight },
						new PdfKeyValueItem<Invoice>() { Key = "Invoice Due Date:", Value = new BindProperty<string, Invoice>((g, m) => m.DueDate.ToLongDateString()), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterRight }
					)
					.WithFont((g, m) => new XFont(g.Theme.FontFamily.TitleLight, 10, XFontStyle.Bold))
					.WithValueFont((g, m) => new XFont(g.Theme.FontFamily.TitleLight, 10, XFontStyle.Regular))
					.WithPadding((0, 0, 0, 0))
					.WithRelativeWidth(.35)
				).WithRelativeHeight(.095)
				 .WithMargin((0, 5, 0, 9)),

				//
				// Reference numbers section.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithMargin((0, 0, 1, 0))
						.WithPadding((1, 2, 1, 2))
						.WithText("Payment Method")
						.WithHeaderBackgroundColor(XColor.FromArgb(37, 32, 98))
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.PaymentMethod)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(XColor.FromArgb(215, 35, 44))
											   .WithFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithValueFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithBorderWidth(1)
											   .WithBorderColor(XColor.FromArgb(37, 32, 98))),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin((1, 0, 1, 0))
						.WithPadding((1, 2, 1, 2))
						.WithText("Check Number")
						.WithHeaderBackgroundColor(XColor.FromArgb(37, 32, 98))
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.CheckNumber)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(XColor.FromArgb(215, 35, 44))
											   .WithFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithValueFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithBorderWidth(1)
											   .WithBorderColor(XColor.FromArgb(37, 32, 98))),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin((1, 0, 0, 0))
						.WithPadding((1, 2, 1, 2))
						.WithText("Job Number")
						.WithHeaderBackgroundColor(XColor.FromArgb(37, 32, 98))
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.JobNumber)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(XColor.FromArgb(215, 35, 44))
											   .WithFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithValueFont((g, m) => g.SubTitleFont().WithSize(14))
											   .WithBorderWidth(1)
											   .WithBorderColor(XColor.FromArgb(37, 32, 98)))
				).WithRelativeHeight(.065)
				 .WithPadding((0, 0, 0, 0)),

				//
				// Bill to/from.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithMargin((0, 0, 1, 0))
						.WithPadding((1, 2, 1, 2))
						.WithText("Bill To")
						.WithHeaderBackgroundColor(XColor.FromArgb(215, 35, 44))
						.WithContentSection(Pdf.KeyValueSection<Invoice>
												(
													new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Name), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.AddressLine), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.CityStateZip), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Phone), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft }
												)
											   .WithForegroundColor(XColor.FromArgb(99, 99, 99))
											   .WithFont((g, m) => g.SubTitleFont().WithSize(11))
											   .WithValueFont((g, m) => g.SubTitleFont().WithSize(11))
											   .WithBorderWidth(1)
											   .WithBorderColor(XColor.FromArgb(37, 32, 98))
											   .WithMargin((0, 3, 0, 3)))
											   .WithPadding((2, 2, 2, 2)),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin((0, 0, 1, 0))
						.WithPadding((1, 2, 1, 2))
						.WithText("From")
						.WithHeaderBackgroundColor(XColor.FromArgb(215, 35, 44))
						.WithContentSection(Pdf.KeyValueSection<Invoice>
												(
													new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Name), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.AddressLine), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.CityStateZip), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Phone), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft }
												)
											   .WithForegroundColor(XColor.FromArgb(99, 99, 99))
											   .WithFont((g, m) => g.SubTitleFont().WithSize(11))
											   .WithValueFont((g, m) => g.SubTitleFont().WithSize(11))
											   .WithBorderWidth(1)
											   .WithBorderColor(XColor.FromArgb(37, 32, 98))
											   .WithMargin((0, 3, 0, 3)))

				).WithRelativeHeight(.16)
				 .WithPadding((0, 0, 0, 0))
				 .WithMargin((0, 5, 0, 5)),

				//
				// Invoice details
				//
				Pdf.DataGridSection<Invoice, InvoiceItem>()
				   .AddColumn<Invoice, InvoiceItem, int>("Item Number", t => t.Id, .20, "{0:1000000000000}", XStringFormats.CenterLeft)
				   .AddColumn<Invoice, InvoiceItem, int>("Quantity", t => t.Quantity, .25, "{0:#,###}", XStringFormats.CenterRight)
				   .AddColumn<Invoice, InvoiceItem, decimal>("Unit Price", t => t.UnitPrice, .25, "{0:C}", XStringFormats.CenterRight)
				   .AddColumn<Invoice, InvoiceItem, decimal>("Amount", t => t.Amount, .30, "{0:C}", XStringFormats.CenterRight)
				   .UseItems((g, m) => m.Items)
				   .WithCellPadding<Invoice, InvoiceItem>((2, 2, 2, 2))
				   .WithPadding((0, 2, 1, 2))
				   .WithColumnHeaderFont<Invoice, InvoiceItem>((g, m) => g.BodyMediumFont(XFontStyle.Bold).WithSize(12))
				   .WithColumnHeaderColor<Invoice, InvoiceItem>(XColor.FromArgb(215, 35, 44))
				   .WithColumnValueFont<Invoice, InvoiceItem>((g, m) => g.BodyLightFont(XFontStyle.Bold).WithSize(12))
				   .WithColumnValueColor<Invoice, InvoiceItem>(XColor.FromArgb(99, 99, 99)),

				//
				// Signature section will display only on the last page.
				//
				Pdf.SignatureSection<Invoice>()
				   .WithRelativeHeight(.05)
				   .WithText("Approved by")
				   .WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount),

				//
				// Tag line.
				//
				Pdf.TextBlockSection<Invoice>()
					.WithText("Thank you for your business!")
					.WithMargin((0, 15, 0, 0))
					.WithFont((g, m) => g.TitleLightFont().WithSize(14))
					.WithForegroundColor(XColor.FromArgb(215, 35, 44))
					.WithTextAlignment(XStringFormats.Center)
					.WithRelativeHeight(.058)
			)
			.WithKey("Report")
			.WithWatermark((g, m) => m.Paid ? "./images/paid.png" : string.Empty);
		}
	}
}