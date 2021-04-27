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
using System.Linq;
using System.Threading.Tasks;
using PdfDocuments.Barcode;
using PdfDocuments.Example.Theme;
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
				   .WithBackgroundColor(ColorPalette.Empty)
				   .WithForegroundColor((g, m) => g.Theme.Color.TitleColor)
				   .WithFont((g, m) => g.TitleLight1Font())
				   .WithBorderWidth(1)
				   .WithBorderColor(ColorPalette.Red)
				   .WithPadding(0, 0, 2, 0),

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
					.WithFont((g, m) => g.BodyLightFont(XFontStyle.Bold).WithSize(g.Theme.FontSize.BodyExtraLarge))
					.WithValueFont((g, m) => g.BodyLightFont().WithSize(g.Theme.FontSize.BodyExtraLarge))
					.WithPadding(0, 0, 0, 0)
					.WithKeyRelativeWidth(.4)
					.WithRelativeWidth(.53)
				).WithRelativeHeight(.095)
				 .WithMargin(0, 5, 0, 7),

				//
				// Reference numbers section.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithMargin(0, 0, 1, 0)
						.WithPadding(1, 2, 1, 2)
						.WithText("Payment Method")
						.WithFont((g, m) => g.SubTitle3Font())
						.WithHeaderBackgroundColor(ColorPalette.Blue)
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.PaymentMethod)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(ColorPalette.Red)
											   .WithFont((g, m) => g.SubTitle3Font())
											   .WithValueFont((g, m) => g.SubTitle3Font())
											   .WithBorderWidth(1)
											   .WithBorderColor(ColorPalette.Blue)),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin(1, 0, 1, 0)
						.WithPadding(1, 2, 1, 2)
						.WithText("Check Number")
						.WithFont((g, m) => g.SubTitle3Font())
						.WithHeaderBackgroundColor(ColorPalette.Blue)
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.CheckNumber)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(ColorPalette.Red)
											   .WithFont((g, m) => g.SubTitle3Font())
											   .WithValueFont((g, m) => g.SubTitle3Font())
											   .WithBorderWidth(1)
											   .WithBorderColor(ColorPalette.Blue)),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin(1, 0, 0, 0)
						.WithPadding(1, 2, 1, 2)
						.WithText("Job Number")
						.WithFont((g, m) => g.SubTitle3Font())
						.WithHeaderBackgroundColor(ColorPalette.Blue)
						.WithContentSection(Pdf.TextBlockSection<Invoice>()
											   .WithText((g, m) => m.JobNumber)
											   .WithTextAlignment(XStringFormats.CenterLeft)
											   .WithForegroundColor(ColorPalette.Red)
											   .WithFont((g, m) => g.SubTitle3Font())
											   .WithValueFont((g, m) => g.SubTitle3Font())
											   .WithBorderWidth(1)
											   .WithBorderColor(ColorPalette.Blue))
				).WithRelativeHeight(.065)
				 .WithPadding(0, 0, 0, 0),

				//
				// Bill to/from.
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.HeaderContentSection<Invoice>()
						.WithMargin(0, 0, 1, 0)
						.WithPadding(1, 2, 1, 2)
						.WithText("Bill To")
						.WithFont((g, m) => g.SubTitle3Font())
						.WithHeaderBackgroundColor(ColorPalette.Red)
						.WithContentSection(Pdf.KeyValueSection<Invoice>
												(
													new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Name), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.AddressLine), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.CityStateZip), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillTo.Phone), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft }
												)
											   .WithForegroundColor(ColorPalette.Gray)
											   .WithFont((g, m) => g.SubTitle3Font())
											   .WithValueFont((g, m) => g.SubTitle3Font())
											   .WithBorderWidth(1)
											   .WithBorderColor(ColorPalette.Blue)
											   .WithKeyRelativeWidth(.4)
											   .WithMargin(0, 3, 0, 3)),

					Pdf.HeaderContentSection<Invoice>()
						.WithMargin(0, 0, 0, 0)
						.WithPadding(1, 2, 1, 2)
						.WithText("From")
						.WithFont((g, m) => g.SubTitle3Font())
						.WithHeaderBackgroundColor(ColorPalette.Red)
						.WithContentSection(Pdf.KeyValueSection<Invoice>
												(
													new PdfKeyValueItem<Invoice>() { Key = "Name:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Name), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Address:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.AddressLine), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "City/State/Zip:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.CityStateZip), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft },
													new PdfKeyValueItem<Invoice>() { Key = "Phone:", Value = new BindProperty<string, Invoice>((g, m) => m.BillFrom.Phone), KeyAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft }
												)
											   .WithForegroundColor(ColorPalette.Gray)
											   .WithFont((g, m) => g.SubTitle3Font())
											   .WithValueFont((g, m) => g.SubTitle3Font())
											   .WithBorderWidth(1)
											   .WithBorderColor(ColorPalette.Blue)
											   .WithKeyRelativeWidth(.4)
											   .WithMargin(0, 3, 0, 3))

				).WithRelativeHeight(.16)
				 .WithPadding(0, 0, 0, 0)
				 .WithMargin(0, 5, 0, 5),

				//
				// Invoice details
				//
				Pdf.DataGridSection<Invoice, InvoiceItem>()
				   .AddColumn<Invoice, InvoiceItem, int>("Item Number", t => t.Id, .20, "{0:1000000000000}", XStringFormats.CenterLeft)
				   .AddColumn<Invoice, InvoiceItem, int>("Quantity", t => t.Quantity, .25, "{0:#,###}", XStringFormats.CenterRight)
				   .AddColumn<Invoice, InvoiceItem, decimal>("Unit Price", t => t.UnitPrice, .25, "{0:C}", XStringFormats.CenterRight)
				   .AddColumn<Invoice, InvoiceItem, decimal>("Amount", t => t.Amount, .30, "{0:C}", XStringFormats.CenterRight)
				   .UseItems((g, m) => m.Items)
				   .WithCellPadding<Invoice, InvoiceItem>(2, 2, 2, 2)
				   .WithPadding(0, 2, 1, 2)
				   .WithColumnHeaderFont<Invoice, InvoiceItem>((g, m) => g.BodyMediumFont(XFontStyle.Bold).WithSize(13))
				   .WithColumnHeaderForegroundColor<Invoice, InvoiceItem>(ColorPalette.Red)
				   .WithColumnHeaderBackgroundColor<Invoice, InvoiceItem>(ColorPalette.MediumRed)
				   .WithColumnValueFont<Invoice, InvoiceItem>((g, m) => g.BodyLightFont(XFontStyle.Regular).WithSize(13))
				   .WithColumnValueForegroundColor<Invoice, InvoiceItem>(ColorPalette.Gray)
				   .WithColumnValueBackgroundColor<Invoice, InvoiceItem>(ColorPalette.MediumBlue),

				//
				//
				//
				Pdf.HorizontalStackSection<Invoice>
				(
					Pdf.EmptySection<Invoice>().WithRelativeWidth(.55),
					Pdf.KeyValueSection<Invoice>
						(
							new PdfKeyValueItem<Invoice>() { Key = "Sub Total:", Value = new BindProperty<string, Invoice>((g, m) => m.Items.Sum(t => t.Amount).ToString("C")), KeyAlignment = XStringFormats.CenterRight, ValueAlignment = XStringFormats.CenterRight },
							new PdfKeyValueItem<Invoice>() { Key = "Tax (6.0%):", Value = new BindProperty<string, Invoice>((g, m) => (m.Items.Sum(t => t.Amount) * .06M).ToString("C")), KeyAlignment = XStringFormats.CenterRight, ValueAlignment = XStringFormats.CenterRight },
							new PdfKeyValueItem<Invoice>() { Key = "Total:", Value = new BindProperty<string, Invoice>((g, m) => (m.Items.Sum(t => t.Amount) * 1.06M).ToString("C")), KeyAlignment = XStringFormats.CenterRight, ValueAlignment = XStringFormats.CenterRight })
						.WithForegroundColor(ColorPalette.Blue)
						.WithFont((g, m) => g.SubTitle2Font())
						.WithValueFont((g, m) => g.SubTitle2Font(XFontStyle.Bold))
						.WithMargin(0, 2, 0, 2)
						.WithPadding(1, 1, 1, 1)
						.WithCellBackgroundColor(ColorPalette.LightRed)
						.WithKeyRelativeWidth(.45)
				).WithRelativeHeight(.1),

				//
				// Signature section will display only on the last page.
				//
				Pdf.SignatureSection<Invoice>()
				   .WithRelativeHeight(.05)
				   .WithText("Approved by")
				   .WithForegroundColor(ColorPalette.Gray)
				   .WithFont((g, m) => g.SubTitle3Font(XFontStyle.Bold))
				   .WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount),

				//
				// Tag line.
				//
				Pdf.TextBlockSection<Invoice>()
					.WithText("Thank you for your business!")
					.WithMargin(0, 15, 0, 0)
					.WithFont((g, m) => g.Title3Font())
					.WithForegroundColor(ColorPalette.Red)
					.WithTextAlignment(XStringFormats.Center)
					.WithRelativeHeight(.058)
			)
			.WithKey("Report")
			.WithWatermark((g, m) => m.Paid ? "./images/paid.png" : string.Empty);
		}
	}
}