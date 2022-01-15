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
using IronBarCode;
using PdfSharp.Drawing;
using System.Threading.Tasks;

namespace PdfDocuments.IronBarcode
{
	public delegate GeneratedBarcode BarcodeCreationCallback(GeneratedBarcode bc);

	public class PdfBarcodeSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding)
		{
			this.Data = data;
			this.BarcodeEncoding = barcodeEncoding;
		}

		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, GeneratedBarcode callback)
		{
			this.Data = data;
			this.BarcodeEncoding = barcodeEncoding;
			this.Callback = callback;
		}

		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier)
			: this(data, barcodeEncoding)
		{
			this.HeightMultiplier = heightMultiplier;
		}

		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, GeneratedBarcode callback)
			: this(data, barcodeEncoding, callback)
		{
			this.HeightMultiplier = heightMultiplier;
		}

		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier)
			: this(data, barcodeEncoding, heightMultiplier)
		{
			this.WidthMultiplier = widthMultiplier;
		}

		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier, GeneratedBarcode callback)
			: this(data, barcodeEncoding, heightMultiplier, callback)
		{
			this.WidthMultiplier = widthMultiplier;
		}

		protected BindProperty<string, TModel> Data { get; set; }
		protected BindProperty<double, TModel> HeightMultiplier { get; set; } = 1.0;
		protected BindProperty<double, TModel> WidthMultiplier { get; set; } = 1.0;
		protected BarcodeEncoding BarcodeEncoding { get; set; }
		protected GeneratedBarcode Callback { get; set; }

		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Get the height and width of the target image.
			//
			int pixelWidth = (int)(g.Grid.ColumnsWidth(bounds.Columns) * this.WidthMultiplier.Resolve(g, m));
			int pixelHeight = (int)(g.Grid.RowsHeight(bounds.Rows) * this.HeightMultiplier.Resolve(g, m));

			//
			// Generate the barcode.
			//
			GeneratedBarcode bc = BarcodeWriter.CreateBarcode(this.Data.Resolve(g, m), this.BarcodeEncoding)
											   .ChangeBackgroundColor(style.BackgroundColor.Resolve(g, m).ToGdiColor())
											   .ChangeBarCodeColor(style.ForegroundColor.Resolve(g, m).ToGdiColor())
											   .ResizeTo(pixelWidth, pixelHeight);

			//
			// Draw the image.
			//
			using (XImage pdfImage = XImage.FromStream(bc.ToStream()))
			{
				g.DrawImage(pdfImage, bounds, PdfHorizontalAlignment.Center, PdfVerticalAlignment.Center);
			}

			return Task.FromResult(returnValue);
		}
	}
}
