using System;
using System.Linq;
using System.Threading.Tasks;
using Lsc.Logistics.Insight.Barcode.Abstractions;
using PdfDocument.Abstractions;
using PdfDocument.BillOfLadingDocument;
using PdfDocument.Theme.Abstractions;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocument.QuoteDocument
{
	public class QuotePdf : PdfGenerator<Quote>
	{
		public QuotePdf(ITheme theme, string imagePath, IBarcodeGenerator barcodeGenerator)
			: base(theme, imagePath, barcodeGenerator)
		{
		}

		protected PageCalculator<Charge> PageCalculator { get; set; }

		protected override Task<PdfGrid> OnSetPageGridAsync(PdfPage page)
		{
			return Task.FromResult(new PdfGrid(this.PageWidth(page), this.PageHeight(page), 400, 160));
		}

		protected override string OnGetDocumentTitle(Quote model)
		{
			return $"Freight Quote";
		}

		protected override Task<int> OnGetPageCountAsync(Quote model)
		{
			// ***
			// *** Order the charges as they need to appear on the quote.
			// ***
			Charge[] charges = (from tbl1 in model.Charges
								join tbl2 in ChargeOrdering.Order on tbl1.Code equals tbl2.Code
								orderby tbl2.Ordinal
								select tbl1).ToArray();

			this.PageCalculator = new PageCalculator<Charge>
			(
				charges,
				19,
				27,
				49,
				41
			);

			return this.PageCalculator.TotalPagesAsync();
		}

		protected override IPdfSection<Quote> OnCreateReportFooterSection()
		{
			// ***
			// *** Remove the report footer section.
			// ***
			return null;
		}

		protected override IPdfSection<Quote> OnCreateContentSection()
		{
			return new PdfVerticalStackSection<Quote>()
			{
				Key = "Quote.Content",
				Children =
				{
					new PdfWrappingTextSection<Quote>()
					{
						Key = "Content.Legal",
						Text = new BindPropertyAction<string, Quote>((gp, m) => { return WellKnown.Strings.LegalText; }),
						RelativeHeight = 0.05,
						ForegroundColor =new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyLightColor; }),
						Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.BodyLightFont().WithSize(gp.Theme.FontSize.Legal); }),
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfTextBlockSection<Quote>()
					{
						Key = "Content.QuoteNumber",
						Text = new BindPropertyAction<string, Quote>((gp, m) => { return $"Quote # {m.QuoteNumber}"; }),
						Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleLightFont().WithSize(16.0); }),
						ForegroundColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyEmphasisColor; }),
						Alignment= XStringFormats.CenterLeft,
						UsePadding = true,
						Padding = new PdfSpacing() { Left = 0, Top = 0, Right = 0, Bottom = 1},
						RelativeHeight = 0.05
					},
					new PdfHorizontalStackSection<Quote>()
					{
						Key = "Content.Detail.Columns",
						RelativeHeight = 0.115,
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == 1; }),
						Children =
						{
							new PdfHeaderContentSection<Quote>()
							{
								Key = "Content.Detail.Shipper",
								Text = "Shipper",
								UseMargins = true,
								BorderWidth = new BindPropertyAction<double, Quote>((gp, m) => { return gp.Theme.Drawing.LineWeight; }),
								UsePadding = true,
								Children =
								{
									new PdfStackedTextSection<Quote>()
									{
										Key = "Content.Detail.Shipper.Address",
										FirstItemDifferent = true,
										FirstItemFont = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleFont(XFontStyle.Bold).WithSize(gp.Theme.FontSize.BodyExtraLarge); }),
										Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleFont().WithSize(gp.Theme.FontSize.BodyExtraLarge); }),
										UsePadding = true,
										StackedItems =
										{
											new BindPropertyAction<string, Quote>((gp, m) => { return m.Shipper.Name; }),
											new BindPropertyAction<string, Quote>((gp, m) => { return $"{m.Shipper.Address1} {m.Shipper.Address2}".Trim(); }),
											new BindPropertyAction<string, Quote>((gp, m)  => { return $"{m.Shipper.City}, {m.Shipper.State} {m.Shipper.Zip}".Trim(); })
										}
									}
								}
							},
							new PdfHeaderContentSection<Quote>()
							{
								Key = "Content.Detail.Consignee",
								Text = "Consignee",
								UseMargins = true,
								BorderWidth = new BindPropertyAction<double, Quote>((gp, m) => { return gp.Theme.Drawing.LineWeight; }),
								UsePadding = true,
								Children =
								{
									new PdfStackedTextSection<Quote>()
									{
										Key = "Content.Detail.Consignee.Address",
										FirstItemDifferent = true,
										FirstItemFont = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleFont(XFontStyle.Bold).WithSize(gp.Theme.FontSize.BodyExtraLarge); }),
										Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleFont().WithSize(gp.Theme.FontSize.BodyExtraLarge); }),
										UsePadding = true,
										StackedItems =
										{
											new BindPropertyAction<string, Quote>((gp, m) => { return m.Consignee.Name; }),
											new BindPropertyAction<string, Quote>((gp, m) => { return $"{m.Consignee.Address1} {m.Consignee.Address2}".Trim(); }),
											new BindPropertyAction<string, Quote>((gp, m)  => { return $"{m.Consignee.City}, {m.Consignee.State} {m.Consignee.Zip}".Trim(); })
										}
									}
								}
							}
						}
					},
					new PdfHeaderContentSection<Quote>()
					{
						Key = "Content.Detail.Stack.Columns.Details",
						Text = "Details",
						BorderWidth = new BindPropertyAction<double, Quote>((gp, m) => { return gp.Theme.Drawing.LineWeight; }),
						UsePadding = true,
						RelativeHeight = .165,
						UseMargins = true,
						Margin = new PdfSpacing() { Left = 0, Top = 2, Right = 0, Bottom = 1 },
						HeaderBackgroundColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.AlternateBackgroundColor1; }),
						HeaderForegroundColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyColor; }),
						BorderColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyColor; }),
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == 1; }),
						Children =
						{
							new PdfNameValueSection<Quote>()
							{
								Key = "Content.Detail.Misc.Details",
								UsePadding = true,
								Padding = new PdfSpacing() { Left = 1, Top = 1, Right = 1, Bottom = 1 },
								Font = new BindProperty<XFont, Quote>((gp, m) => { return gp.TitleFont(XFontStyle.Bold).WithSize(gp.Theme.FontSize.BodyLarge); }),
								ValueFont = new BindProperty<XFont, Quote>((gp, m) => { return gp.TitleFont(XFontStyle.Regular).WithSize(gp.Theme.FontSize.BodyLarge); }),
								NameColumnRelativeWidth = .4,
								Items =
								{
									new PdfNameValueItem<Quote>() { Key = "Transit Time:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return $"{m.TransitDays} day{(m.TransitDays == 1 ? "":"s")}"; }) },
									new PdfNameValueItem<Quote>() { Key = "Service:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return m.ServiceType; }) },
									new PdfNameValueItem<Quote>() { Key = "Carrier:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return  m.Carrier != null ? $"{m.Carrier.Name} [{m.Carrier.Scac}]" : "Not Assigned"; })  },
									new PdfNameValueItem<Quote>() { Key = "Shipment/Rate Account Numbers:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return $"{m.ShipmentAccountNumber:000000000}/{m.RateAccountNumber:000000000}"; }) },
									new PdfNameValueItem<Quote>() { Key = "Group ID:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return $"{(m.GroupId==0 ? "Not Grouped" : m.GroupId.ToString())}"; }) },
									new PdfNameValueItem<Quote>() { Key = "Tariff ID:", NameAlignment = XStringFormats.CenterLeft, ValueAlignment = XStringFormats.CenterLeft, Value = new BindProperty<string, Quote>((grp, m) => { return $"{m.TariffId}"; }) }
								}
							}
						}
					},
					new PdfTextBlockSection<Quote>()
					{
						Key = "Content.Expiration",
						Text = new BindPropertyAction<string, Quote>((gp, m) => { return $"Quote Expires {m.ExpirationDateTime:dddd MMM d, yyyy}"; }),
						Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.TitleLightFont().WithSize(10.5); }),
						ForegroundColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyEmphasisColor; }),
						Alignment= XStringFormats.CenterLeft,
						RelativeHeight = 0.04,
						UsePadding = true,
						Padding = new PdfSpacing() { Left = 0, Top = 0, Bottom = 1, Right = 0 },
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfHeaderContentSection<Quote>()
					{
						Key = "Content.Breakdown.Header",
						Text = new BindProperty<string, Quote>((gp, m) => { return gp.PageNumber == 1 ? "Quote Breakdown" : this.PageCalculator.PartitionedItems[gp.PageNumber-1].Count() > 0 ? "ITEMS (continued)" : String.Empty; }),
						BorderWidth = new BindProperty<double, Quote>((gp, m) => { return gp.Theme.Drawing.LineWeight; }),
						UsePadding = true,
						Padding = new PdfSpacing() { Left = 1, Top = 1, Right = 1, Bottom = 1 },
						Children =
						{
							new PdfVerticalStackSection<Quote>()
							{
								Children =
								{
									new HeaderSection(this.PageCalculator)
									{
										Key = "Content.Breakdown.ItemsHeader",
										RelativeHeight = .068,
										UsePadding = true,
										ShouldRender =new BindProperty<bool, Quote>((gp, m) => { return gp.PageNumber == 1; })
									},
									new ChargeItemsSection(this.PageCalculator)
									{
										Key = "Content.Breakdown.Items",
										RelativeHeight = 0.0,
										TextBackgroundColor = XColors.White
									}
								}
							}
						}
					},
					new TotalSection(this.PageCalculator)
					{
						Key = "Content.Total",
						RelativeHeight = .037,
						UsePadding = true,
						Padding = new PdfSpacing() { Left = 1, Top = 0, Right = 1, Bottom = 1 },
						UseMargins = true,
						Margin = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						BorderColor = new BindProperty<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyColor; }),
						BorderWidth = .065,
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == gp.Document.PageCount; })
					},
					new PdfTextBlockSection<Quote>()
					{
						Key = "Content.Fuel",
						Text = new BindPropertyAction<string, Quote>((gp, m) => { return $"Fuel Surcharge was determined at a rate of {m.FuelSurchargePercent:#,##0.0%} of line haul based on the current fuel price of {m.FuelPrice:$#,##0.00} as of {m.FuelEffectiveDate.ToLongDateString()}."; }),
						Font = new BindPropertyAction<XFont, Quote>((gp, m) => { return gp.BodyFont().WithSize(gp.Theme.FontSize.BodySmall); }),
						ForegroundColor = new BindPropertyAction<XColor, Quote>((gp, m) => { return gp.Theme.Color.BodyEmphasisColor; }),
						Alignment= XStringFormats.CenterLeft,
						RelativeHeight = 0.035,
						UsePadding = true,
						Padding = new PdfSpacing() { Left = 0, Top = 0, Right = 0, Bottom = 1 },
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == gp.Document.PageCount; })
					},
					new PdfHeaderContentSection<Quote>()
					{
						Key = "Content.CustomerService.Stack",
						Text = "Customer Service",
						RelativeHeight = .09,
						Children =
						{
							new CustomerServiceSection()
							{
								Key = "Content.CustomerService.Detail"
							}
						},
						ShouldRender = new BindPropertyAction<bool, Quote>((gp, m) => { return gp.PageNumber == gp.Document.PageCount; })
					},
				},
				WaterMarkImagePath = new BindProperty<string, Quote>((gp, m) => { return (!String.IsNullOrEmpty(m.ProNumber)) ? "./images/approved.png" : (m.ExpirationDateTime < DateTime.Now) ? "./images/expired.png" : String.Empty; })
			};
		}
	}
}