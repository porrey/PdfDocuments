using System;
using System.Linq;
using System.Threading.Tasks;
using Lsc.Logistics.Insight.Barcode.Abstractions;
using PdfDocument.Abstractions;
using PdfDocument.Theme.Abstractions;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocument.BillOfLadingDocument
{
	public class BillOfLadingPdf : PdfGenerator<BillOfLading>
	{
		public BillOfLadingPdf(ITheme theme, string imagePath, IBarcodeGenerator barcodeGenerator)
			: base(theme, imagePath, barcodeGenerator)
		{
		}

		protected PageCalculator<LineItem> PageCalculator { get; set; }

		protected override Task<PdfGrid> OnSetPageGridAsync(PdfPage page)
		{
			return Task.FromResult(new PdfGrid(this.PageWidth(page), this.PageHeight(page), 400, 160));
		}

		protected override string OnGetDocumentTitle(BillOfLading model)
		{
			return "Straight Bill of Lading";
		}

		protected override Task<int> OnGetPageCountAsync(BillOfLading model)
		{
			this.PageCalculator = new PageCalculator<LineItem>
			(
				model.LineItems,
				2,
				14,
				56,
				43
			);

			return this.PageCalculator.TotalPagesAsync();
		}

		protected override IPdfSection<BillOfLading> OnCreateContentSection()
		{
			return new PdfVerticalStackSection<BillOfLading>()
			{
				Key = "Bol.Content",
				UseMargins = false,
				UsePadding = false,
				Children =
				{
					new PdfWrappingTextSection<BillOfLading>()
					{
						Key = "Content.Shipper.Legal",
						Text = WellKnown.Strings.LegalText,
						RelativeHeight = 0.09,
						ForegroundColor = new BindPropertyAction<XColor, BillOfLading>((gp, m)=>{ return gp.Theme.Color.BodyLightColor; }),
						Font = new BindPropertyAction<XFont, BillOfLading>((gp, m) => { return gp.BodyLightFont().WithSize(gp.Theme.FontSize.Legal); }),
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new BolNumberSection()
					{
						Key = "Content.BolNumber",
						RelativeHeight = .065
					},
					new ReferenceFieldSection()
					{
						Key = "Content.ReferenceNumbers",
						RelativeHeight = .025,
						Padding = new PdfSpacing() { Left = 1, Top = 1, Right = 1, Bottom = 1 },
						UsePadding = true,
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && (m.Reference1 != null || m.Reference2 != null || m.Reference3 != null || m.Reference4 != null); })
					},
					new PdfHeaderContentSection<BillOfLading>()
					{
						Key = "Content.Shipper",
						Text = "Shipper",
						RelativeHeight = .115,
						Children =
						{
							new PdfHorizontalStackSection<BillOfLading>()
							{
								Key = "Content.Shipper.Stack",
								Children =
								{
									new ShipperAddressSection()
									{
										Key = "Content.Shipper.Address",
										RelativeWidth = .6
									},
									new  PdfWrappingTextSection<BillOfLading>()
									{
										Key = "Content.Shipper.Legal",
										RelativeWidth = .4,
										Text = WellKnown.Strings.ShipperLegalText,
										ForegroundColor = new BindPropertyAction<XColor, BillOfLading>((gp, m)=>{ return gp.Theme.Color.BodyLightColor; }),
										Font = new BindPropertyAction<XFont, BillOfLading>((gp, m) => { return gp.BodyLightFont().WithSize(gp.Theme.FontSize.Legal); }),
										Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
										UsePadding = true
									}
								}
							}
						},
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfWrappingTextSection<BillOfLading>()
					{
						Key = "Content.Shipper.Note",
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						RelativeHeight = .025,
						Text = new BindProperty<string, BillOfLading>((gp, m) => { return $"Pickup Note: {m.PickupNote}"; }),
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && !String.IsNullOrWhiteSpace(m.PickupNote); })
					},
					new PdfSignatureSection<BillOfLading>()
					{
						Key = "Content.Consignee.Signature",
						RelativeHeight = .04,
						Text = "Consignor Signature",
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfHeaderContentSection<BillOfLading>()
					{
						Key = "Content.Carrier",
						Text = "Carrier",
						RelativeHeight = .14,
						Children =
						{
							new PdfHorizontalStackSection<BillOfLading>()
							{
								Key = "Content.Carrier.Stack",
								Children =
								{
									new CarrierDetailsSection()
									{
										Key = "Content.Carrier.Details",
										ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
									},
									new CarrierProNumberSection()
									{
										Key = "Content.Carrier.Tracking",
										RelativeWidth = .30,
										ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && !String.IsNullOrWhiteSpace(m.Carrier.ProNumber) && m.ServiceType.ToUpper() == "LTL"; })
									},
									new AffixCarrierProNumberSection()
									{
										Key = "Content.Carrier.AffixTracking",
										RelativeWidth = .35,
										Margin = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 0 },
										UseMargins = true,
										BorderColor = new BindProperty<XColor, BillOfLading>((gp, m) => { return gp.Theme.Color.BodyVeryLightColor; }),
										BorderWidth = new BindProperty<double, BillOfLading>((gp, m) => { return gp.Theme.Drawing.LineWeight; }),
										ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && String.IsNullOrWhiteSpace(m.Carrier.ProNumber) && m.ServiceType.ToUpper() == "LTL"; })
									}
								}
							}
						},
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfWrappingTextSection<BillOfLading>()
					{
						Key = "Content.Carrier.SpecialInstructions",
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						RelativeHeight = .025,
						Text = new BindProperty<string, BillOfLading>((gp, m) => { return $"Special Instructions: {m.SpecialInstructions}"; }),
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && !String.IsNullOrWhiteSpace(m.SpecialInstructions); })
					},
					new PdfSignatureSection<BillOfLading>()
					{
						Key = "Content.Carrier.Signature",
						RelativeHeight = .04,
						Text = "Driver Signature",
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfHeaderContentSection<BillOfLading>()
					{
						Key = "Content.Consignee",
						Text = "Consignee",
						RelativeHeight = .145,
						Children =
						{
							new PdfHorizontalStackSection<BillOfLading>()
							{
								Key = "Content.Consignee.Stack",
								Children =
								{
									new ConsigneeDetailsSection()
									{
										Key = "Content.Consignee.Details",
										ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
									},
									new ConsigneeServicesSection()
									{
										Key = "Content.Consignee.Services",
										RelativeWidth = .60,
										ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && m.Accessorials.Any(); })
									}
								}
							}
						},
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1; })
					},
					new PdfWrappingTextSection<BillOfLading>()
					{
						Key = "Content.Consignee.Note",
						Padding = new PdfSpacing() { Left = 0, Top = 1, Right = 0, Bottom = 1 },
						UsePadding = true,
						RelativeHeight = .025,
						Text = new BindProperty<string, BillOfLading>((gp, m) => { return $"Delivery Note: {m.DeliveryNote}"; }),
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == 1 && !String.IsNullOrWhiteSpace(m.DeliveryNote); })
					},
					new PdfHeaderContentSection<BillOfLading>()
					{
						Key = "Content.PackageDetail.Header",
						Text = "Package Detail",
						Children =
						{
							new PackageDetailSection(this.PageCalculator)
							{
								Key = "Content.PackageDetail"
							}
						}
					},
					new PdfHeaderContentSection<BillOfLading>()
					{
						Key = "Content.ProofOfDelivery.Header",
						Text = "Proof of Delivery",
						RelativeHeight = .21,
						ShouldRender = new BindPropertyAction<bool, BillOfLading>((gp, m) => { return gp.PageNumber == gp.Document.PageCount; }),
						Children =
						{
							new ProofOfDeliverySection()
							{
								Key = "Content.ProofOfDelivery"
							}
						}
					}
				},
				WaterMarkImagePath = new BindProperty<string, BillOfLading>((gp, m) => { return m.Delivered.HasValue ? "./images/delivered.png" : String.Empty; })
			};
		}

		protected override IPdfSection<BillOfLading> OnCreateReportFooterSection()
		{
			// ***
			// *** Remove the report footer section.
			// ***
			return null;
		}
	}
}