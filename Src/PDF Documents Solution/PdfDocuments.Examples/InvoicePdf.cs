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
			this.StyleManager.Add("Test", Style.Create<Invoice>()
											   .UseFont(new XFont("Arial", 12))
											   .UseForegroundColor(ColorPalette.Red)
											   .UseBackgroundColor(ColorPalette.Transparent)
											   .UseBorderColor(ColorPalette.LightRed)
											   .UseBorderWidth(1)
											   .Build());

			this.StyleManager.GetStyle("").Copy().UseBackgroundColor(ColorPalette.White);

			//
			// Add page header style.
			//
			this.StyleManager.Add("PageHeader", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial Narrow", 48),
				BorderColor = ColorPalette.Red,
				BorderWidth = 1,
				Padding = new PdfSpacing(0, 0, 2, 0),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Blue,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("InvoiceNumber", new PdfStyle<Invoice>()
			{
				Margin = new PdfSpacing(0, 5, 0, 7)
			});

			this.StyleManager.Add("InvoiceNumber.Key", new PdfStyle<Invoice>()
			{
				Font = new XFont("Times New Roman", 11.75, XFontStyle.Bold),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(0, 0, 0, 0),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterRight,
				RelativeWidths = new double[] { .5 }
			});

			this.StyleManager.Add("InvoiceNumber.Value", new PdfStyle<Invoice>()
			{
				Font = new XFont("Times New Roman", 11.75, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(0, 0, 0, 0),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("Address.Section", new PdfStyle<Invoice>()
			{
				Padding = new PdfSpacing(0, 0, 0, 0),
				Margin = new PdfSpacing(0, 5, 0, 5)
			});

			this.StyleManager.Add("Address.ContentBlock", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				//BorderColor = ColorPalette.Red,
				BorderColor = ColorPalette.Blue,
				BorderWidth = 1,
				Padding = new PdfSpacing(1, 2, 1, 2),
				Margin = new PdfSpacing(0, 0, 0, 0),
				BackgroundColor = ColorPalette.White,
				ForegroundColor = ColorPalette.Transparent,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Address.Header.Left", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 3, 1, 3),
				Margin = new PdfSpacing(0, 2, 1, 2),
				BackgroundColor = ColorPalette.Red,
				ForegroundColor = ColorPalette.White,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Address.Header.Right", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 3, 1, 3),
				Margin = new PdfSpacing(1, 2, 0, 2),
				BackgroundColor = ColorPalette.Red,
				ForegroundColor = ColorPalette.White,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Address.Key", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 1, 1, 1),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterRight,
				RelativeWidths = new double[] { .3 }
			});

			this.StyleManager.Add("Address.Value", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Bold),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 1, 1, 1),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("Totals", new PdfStyle<Invoice>()
			{
				Padding = new PdfSpacing(0, 2, 0, 2)
			});

			this.StyleManager.Add("Totals.Key", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11.75, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 1, 1, 1),
				BackgroundColor = ColorPalette.LightRed,
				ForegroundColor = ColorPalette.Blue,
				TextAlignment = XStringFormats.CenterRight,
				RelativeWidths = new double[] { .45 }
			});

			this.StyleManager.Add("Totals.Value", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11.75, XFontStyle.Bold),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 1, 1, 1),
				BackgroundColor = ColorPalette.LightRed,
				ForegroundColor = ColorPalette.Blue,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("Reference.Header.1", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 2, 1, 2),
				Margin = new PdfSpacing(0, 2, 1, 2),
				BackgroundColor = ColorPalette.Blue,
				ForegroundColor = ColorPalette.White,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Reference.Header.2", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 2, 1, 2),
				Margin = new PdfSpacing(1, 2, 1, 2),
				BackgroundColor = ColorPalette.Blue,
				ForegroundColor = ColorPalette.White,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Reference.Header.3", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(1, 2, 1, 2),
				Margin = new PdfSpacing(1, 2, 0, 2),
				BackgroundColor = ColorPalette.Blue,
				ForegroundColor = ColorPalette.White,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("Reference.Body", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 11, XFontStyle.Regular),
				BorderColor = ColorPalette.Blue,
				BorderWidth = 1,
				Padding = new PdfSpacing(1, 2, 1, 2),
				BackgroundColor = ColorPalette.White,
				ForegroundColor = ColorPalette.Red,
				TextAlignment = XStringFormats.CenterLeft
			});

			this.StyleManager.Add("InvoiceItems.Grid", new PdfStyle<Invoice>()
			{
				Margin = new PdfSpacing(0, 2, 0, 2)
			});

			this.StyleManager.Add("InvoiceItems.Header", new PdfStyle<Invoice>()
			{
				Font = new XFont("Times New Roman", 13, XFontStyle.Bold),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(0, 2, 1, 2),
				CellPadding = new PdfSpacing(2, 2, 2, 2),
				BackgroundColor = ColorPalette.MediumRed,
				ForegroundColor = ColorPalette.Red,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("InvoiceItems.Body", new PdfStyle<Invoice>()
			{
				Font = new XFont("Times New Roman", 13, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(0, 2, 1, 2),
				BackgroundColor = ColorPalette.MediumBlue,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterRight
			});

			this.StyleManager.Add("Signature", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 10, XFontStyle.Regular),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 1,
				Padding = new PdfSpacing(0, 0, 0, 0),
				Margin = new PdfSpacing(0, 0, 0, 0),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Gray,
				TextAlignment = XStringFormats.CenterLeft,
				RelativeWidths = new double[] { .4 }
			});

			this.StyleManager.Add("ThankYou", new PdfStyle<Invoice>()
			{
				Font = new XFont("Arial", 12, XFontStyle.Italic),
				BorderColor = ColorPalette.Transparent,
				BorderWidth = 0,
				Padding = new PdfSpacing(0, 2, 1, 2),
				Margin = new PdfSpacing(0, 15, 0, 0),
				CellPadding = new PdfSpacing(2, 2, 2, 2),
				BackgroundColor = ColorPalette.Transparent,
				ForegroundColor = ColorPalette.Red,
				TextAlignment = XStringFormats.Center
			});
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
					.WithStyles("Default", "InvoiceNumber.Key", "InvoiceNumber.Value")
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
				   .AddColumn<Invoice, InvoiceItem, int>("Item Number", t => t.Id, .20, "{0:1000000000000}")
				   .AddColumn<Invoice, InvoiceItem, int>("Quantity", t => t.Quantity, .25, "{0:#,###}")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Unit Price", t => t.UnitPrice, .25, "{0:C}")
				   .AddColumn<Invoice, InvoiceItem, decimal>("Amount", t => t.Amount, .30, "{0:C}")
				   .UseItems((g, m) => m.Items)
				   .WithStyles("InvoiceItems.Grid", "InvoiceItems.Header", "InvoiceItems.Body"),

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