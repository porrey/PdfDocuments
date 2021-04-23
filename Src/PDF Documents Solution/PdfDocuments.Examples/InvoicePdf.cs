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
			return $"Invoice";
		}

		protected override Task<int> OnGetPageCountAsync(Invoice model)
		{
			return Task.FromResult(1);
		}

		protected override IPdfSection<Invoice> OnCreateReportHeaderSection()
		{
			//
			// Remove the report header section.
			//
			return null;
		}

		protected override IPdfSection<Invoice> OnCreateReportFooterSection()
		{
			//
			// Remove the report footer section.
			//
			return null;
		}

		protected override IPdfSection<Invoice> OnCreatePageHeaderSection()
		{
			//
			// Override the default page header.
			//
			PageHeaderSection<Invoice> section = (PageHeaderSection<Invoice>)base.OnCreatePageHeaderSection();
			section.LogoPath = "./Images/logo.jpg";
			section.RelativeHeight = .12;
			section.BackgroundColor = XColors.White;
			section.Title = "INVOICE";
			section.ForegroundColor = XColors.DarkGray;
			section.Font = new BindProperty<XFont, Invoice>((gp, m) => { return new XFont(gp.Theme.FontFamily.TitleLight, 44); });
			return section;
		}

		protected override IPdfSection<Invoice> OnCreatePageFooterSection()
		{
			//
			// Remove the page footer section.
			//
			return null;
		}

		protected override IPdfSection<Invoice> OnCreateContentSection()
		{
			return new PdfVerticalStackSection<Invoice>()
			{
				Key = "Invoice.Content",
				Children =
				{

				},
				WaterMarkImagePath = new BindProperty<string, Invoice>((gp, m) => { return m.Paid ? "./images/paid.png" : string.Empty; })
			};
		}
	}
}