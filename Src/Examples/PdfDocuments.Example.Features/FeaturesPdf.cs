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

namespace PdfDocuments.Example.Features
{
	/// <summary>
	/// Generates a PDF document displaying exzamples of the features within this library.
	/// </summary>
	public class FeaturesPdf : PdfGenerator<Message>
	{
		/// <summary>
		/// Initializes a new instance of the FeaturesPdf class with the specified PDF style manager.
		/// </summary>
		/// <param name="styleManager">The style manager used to apply formatting to Message objects in generated PDF documents. Cannot be null.</param>
		public FeaturesPdf(IPdfStyleManager<Message> styleManager)
			: base(styleManager)
		{
		}

		protected override Task OnInitializeStylesAsync(IPdfStyleManager<Message> styleManager)
		{
			//
			// Add a style for the text. The style name is referenced in the
			// content creation.
			//
			this.StyleManager.Add("HelloWorld.Text", Style.Create<Message>()
						.UseFont("Arial", 48)
						.UseForegroundColor(XColors.Purple)
						//.UseBackgroundColor(XColors.LightPink)
						//.UseBorderWidth(1)
						//.UseBorderColor(XColors.Red)
						//.UseCellPadding(2, 2, 2, 2)
						.UseTextAlignment(XStringFormats.Center)
						.UsePadding(2, 2, 2, 2)
						//.UseTextWrapping(true)
						//.UseParagraphAlignment(XParagraphAlignment.Right)
						//.UseMargin(4, 4, 4, 4)
						//.UseClipDrawing(true)
						//.UseFixedHeight(75)
						//.UseFixedWidths(75)
						//.UseRelativeHeight(.5)
						//.UseRelativeWidths(.3)
						.Build());

			this.StyleManager.Add("Watermark", Style.Create<Message>()
						.UseHorizontalImageAlignment(PdfHorizontalAlignment.Right)
						.UseVerticalImageAlignment(PdfVerticalAlignment.Bottom)
						.UsePadding(0, 0, 0, 0)
						.UseImageOpacity(.15f)
						.UseImageScale(.75f)
						.Build());

			this.StyleManager.Add("Footer", Style.Create<Message>()
				.UseFixedHeight(8)
				.Build());

			this.StyleManager.Add("Footer.TopLeft", Style.Create<Message>()
				.UseFont("Arial", 8)
				.UsePadding(1, 0, 0, 0)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());

			this.StyleManager.Add("Footer.TopRight", Style.Create<Message>()
				.UseFont("Arial", 8)
				.UsePadding(0, 0, 1, 0)
				.UseTextAlignment(XStringFormats.CenterRight)
				.Build());

			this.StyleManager.Add("Footer.BottomLeft", Style.Create<Message>()
				.UseFont("Arial", 8, XFontStyleEx.Bold)
				.UseForegroundColor(XColors.Blue)
				.UsePadding(1, 0, 0, 0)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());

			this.StyleManager.Add("Footer.BottomRight", Style.Create<Message>()
				.UseFont("Arial", 8, XFontStyleEx.Bold)
				.UseForegroundColor(XColors.Red)
				.UsePadding(0, 0, 1, 0)
				.UseTextAlignment(XStringFormats.CenterRight)
				.Build());

			this.StyleManager.Add("Footer.Line", Style.Create<Message>()
				.UseForegroundColor(XColors.Black)
				.UsePadding(0, 0, 0, 0)
				.UseVerticalLineAlignment(PdfVerticalAlignment.Top)
				.UseBorderColor(XColors.Green)
				.UseBorderWidth(1)
				.UseFixedHeight(1)
				.Build());

			this.StyleManager.Add("Header.Line", Style.Create<Message>()
				.UseForegroundColor(XColors.Black)
				.UsePadding(0, 0, 0, 0)
				.UseVerticalLineAlignment(PdfVerticalAlignment.Bottom)
				.UseBorderColor(XColors.Orange)
				.UseBorderWidth(1)
				.UseFixedHeight(1)
				.Build());

			this.StyleManager.Add("Reference", Style.Create<Message>()
				.UseRelativeHeight(.15)
				.UseRelativeWidths(.5)
				.Build());

			this.StyleManager.Add("Reference.Header", Style.Create<Message>()
				.UseFont("Arial", 19, XFontStyleEx.Regular)
				.UseCellPadding(1, 2, 1, 2)
				.UseForegroundColor(ColorPalette.White)
				.UseBackgroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UseRelativeHeight(.35)
				.Build());

			this.StyleManager.Add("Reference.Container", Style.Create<Message>()
				//.UseRelativeHeight(.65)
				.Build());

			this.StyleManager.Add("Reference.Body", Style.Create<Message>()
				.UseFont("Arial", 19, XFontStyleEx.Regular)
				.UseBorderColor(ColorPalette.Blue)
				.UseBorderWidth(1)
				.UseCellPadding(1, 2, 1, 2)
				.UseForegroundColor(ColorPalette.Red)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.Build());

			this.StyleManager.Add("Image", Style.Create<Message>()
				.UseCellPadding(0, 0, 5, 5)
				.UsePadding(5, 5, 5, 5)
				.UseImageScale(.5f)
				.UseHorizontalImageAlignment(PdfHorizontalAlignment.Right)
				.UseVerticalImageAlignment(PdfVerticalAlignment.Bottom)
				.UseClipDrawing(true)
				.UseMargin(5, 5, 5, 5)
				.UseBorderColor(XColors.Red)
				.UseBorderWidth(1)
				.Build());

			this.StyleManager.Add("ReportHeader", Style.Create<Message>()
				.UseBorderColor(XColors.Red)
				.UseBorderWidth(1)
				.UseMargin(1, 1, 1, 1)
				.UseRelativeHeight(.15)
				.Build());

			this.StyleManager.Add("ReportHeader.Logo", Style.Create<Message>()
				.UseHorizontalImageAlignment(PdfHorizontalAlignment.Right)
				.UseVerticalImageAlignment(PdfVerticalAlignment.Center)
				.UsePadding(1, 0, 0, 0)
				.UseImageScale(.25f)
				.Build());

			this.StyleManager.Add("ReportHeader.Title", Style.Create<Message>()
				.UseFont("Arial", 48, XFontStyleEx.Regular)
				.UseForegroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.CenterLeft)
				.UsePadding(0, 0, 2, 0)
				.Build());

			this.StyleManager.Add("Signature.Section", Style.Create<Message>()
				.UseFixedHeight(15)
				.Build());

			this.StyleManager.Add("Signature.Line", Style.Create<Message>()
				.UseBorderColor(XColors.Red)
				.UseBorderWidth(1)
				.UseFixedHeight(1)
				.Build());

			this.StyleManager.Add("Signature.Image", Style.Create<Message>()
				.UseRelativeWidths(.35)
				.UseImageScale(.05f)
				.UseHorizontalImageAlignment(PdfHorizontalAlignment.Left)
				.UseVerticalImageAlignment(PdfVerticalAlignment.Top)
				.UsePadding(1, 0, 0, 0)
				.Build());

			this.StyleManager.Add("Signature.Text", Style.Create<Message>()
				.UseFont("Arial", 10, XFontStyleEx.Regular)
				.UseForegroundColor(ColorPalette.Red)
				.UseTextAlignment(XStringFormats.BottomLeft)
				.UsePadding(0, 0, 0, 2)
				.UseRelativeWidths(.15)
				.Build());

			this.StyleManager.Add("Signature.Date", Style.Create<Message>()
				.UseFont("Arial", 12, XFontStyleEx.BoldItalic)
				.UseForegroundColor(ColorPalette.Blue)
				.UseTextAlignment(XStringFormats.BottomLeft)
				.UsePadding(0, 0, 0, 2)
				.UseRelativeWidths(.35)
				.Build());

			this.StyleManager.Add("KeyValue", Style.Create<Message>()
				.UsePadding(0, 0, 0, 0)
				.UseRelativeHeight(.11)
				.Build());

			this.StyleManager.Add("KeyValue.Key", Style.Create<Message>()
				.UseFont("Arial", 11.75, XFontStyleEx.Regular)
				.UseMargin(1, 1, 1, 1)
				.UsePadding(0, 0, 0, 0)
				.UseCellPadding(1, 1, 1, 1)
				.UseForegroundColor(ColorPalette.Blue)
				.UseBackgroundColor(ColorPalette.LightRed)
				.UseTextAlignment(XStringFormats.CenterRight)
				.UseRelativeWidths(.5)
				.Build());

			this.StyleManager.Add("KeyValue.Value", Style.Copy(this.StyleManager.GetStyle("KeyValue.Key"))
				.UseFont("Arial", 11.75, XFontStyleEx.Bold)
				.UseRelativeWidths(.5)
				.Build());

			return Task.CompletedTask;
		}

		protected override Task<IPdfSection<Message>> OnAddContentAsync()
		{
			//
			// Add a basic text block using the style that was created.
			//
			return Task.FromResult(
				Pdf.ContentSection<Message>(

				//Pdf.StackedTextSection<Message>()
				//	.WithStackedItems((g, m) => m.Text, (g, m) => m.Text, (g, m) => m.Text, (g, m) => m.Text, (g, m) => m.Text)
				//	.WithStyles("HelloWorld.Text")
				//	.WithSectionsLayoutMode(PdfSectionsLayoutMode.VerticalStacking)

				//Pdf.VerticalStackSection(
				//	Pdf.TextBlockSection<Message>()
				//		.WithText((g, m) => m.Text)
				//		.WithStyles("HelloWorld.Text")
				//		.WithKey("HelloWorld.TextBlock"),
				//	Pdf.TextBlockSection<Message>()
				//		.WithText((g, m) => m.Text)
				//		.WithStyles("HelloWorld.Text")
				//		.WithKey("HelloWorld.TextBlock"),
				//	Pdf.TextBlockSection<Message>()
				//		.WithText((g, m) => m.Text)
				//		.WithStyles("HelloWorld.Text")
				//		.WithKey("HelloWorld.TextBlock")
				//).WithStyleManager(this.StyleManager)
				// .WithStyles("Watermark")
				// .WithWatermark("Images/watermark.png")

				//Pdf.HeaderContentSection<Message>()
				//	.WithText("Payment Method")
				//	.WithStyles("Reference", "Reference.Header", "Reference.Container")
				//	.WithContentSection
				//	(
				//		Pdf.TextBlockSection<Message>()
				//			.WithText((g, m) => m.Text)
				//			.WithStyles("Reference.Body")
				//	)

				//Pdf.ImageSection<Message>()
				//	.WithStyles("Image")
				//	.WithImage("Images/watermark.png")

				//Pdf.ReportHeaderSection<Message>()
				//	.WithTitle("INVOICE")
				//	.WithLogo("./Images/logo.jpg")
				//	.WithStyles("ReportHeader", "ReportHeader.Logo", "ReportHeader.Title")
				//	.WithLogoPosition(PdfLogoPosition.Right)

				//Pdf.VerticalStackSection(
				//	Pdf.PageHeaderSection<Message>()
				//		.WithTopLeftText("Top Left Text")
				//		.WithTopRightText("Top Right Text")
				//		.WithBottomLeftText("Bottom Left Text")
				//		.WithBottomRightText("Bottom Right Text")
				//		.WithStyles("Footer", "Footer.TopLeft", "Footer.TopRight", "Footer.BottomLeft", "Footer.BottomRight")
				//		.WithZOrder(10),
				//	Pdf.HorizontalLineSection<Message>()
				//		.WithRowEdge(PdfRowEdge.Top)
				//		.WithStyles("Header.Line"),
				//	Pdf.TextBlockSection<Message>()
				//		.WithText((g, m) => m.Text)
				//		.WithStyles("HelloWorld.Text")
				//		.WithKey("HelloWorld.TextBlock"),
				//	Pdf.HorizontalLineSection<Message>()
				//		.WithRowEdge(PdfRowEdge.Bottom)
				//		.WithStyles("Footer.Line"),
				//	Pdf.PageFooterSection<Message>()
				//		.WithTopLeftText("Top Left Text")
				//		.WithTopRightText("Top Right Text")
				//		.WithBottomLeftText("Bottom Left Text")
				//		.WithBottomRightText("Bottom Right Text")
				//		.WithStyles("Footer", "Footer.TopLeft", "Footer.TopRight", "Footer.BottomLeft", "Footer.BottomRight")
				//)

				//Pdf.SignatureSection<Message>()
				//	.WithRenderCondition((g, m) => g.PageNumber == g.Document.PageCount)
				//	.WithStyles("Signature.Section", "Signature.Line", "Signature.Text", "Signature.Image", "Signature.Text", "Signature.Date")
				//	.WithSignatureOptions(new SignatureOptions<Message>()
				//	{
				//		SignatureText = "Approved By",
				//		SignatureImage = "./Images/signature.jpg",
				//		DateLabel = "Approval Date",
				//		Date = DateTimeOffset.Now
				//	})

				//Pdf.HorizontalStackSection<Message>
				//(
				//	Pdf.KeyValueSection<Message>
				//		(
				//			new PdfKeyValueItem<Message>("Sub Total:", (g, m) => "$82,765.23"),
				//			new PdfKeyValueItem<Message>("Tax (6.0%):", (g, m) => "$4,965.91"),
				//			new PdfKeyValueItem<Message>("Total:", (g, m) => "$87,731.14")
				//		).WithStyles("KeyValue", "KeyValue.Key", "KeyValue.Value"),
				//	Pdf.EmptySection<Message>()
				//)

				).WithStyleManager(this.StyleManager)
			);
		}
	}
}