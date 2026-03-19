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
using IronBarCode;
using PdfSharp.Drawing;

namespace PdfDocuments.IronBarcode
{
	/// <summary>
	/// Represents a callback method that processes a generated barcode and returns a modified or new barcode instance.
	/// </summary>
	/// <param name="bc">The generated barcode to be processed by the callback. Cannot be null.</param>
	/// <returns>A barcode instance resulting from the callback's processing. May be the original or a new barcode.</returns>
	public delegate GeneratedBarcode BarcodeCreationCallback(GeneratedBarcode bc);

	/// <summary>
	/// Represents a PDF section that renders a barcode based on a bound property from the model.
	/// </summary>
	/// <remarks>Use PdfBarcodeSection to add a barcode to a PDF document, with customizable encoding and size. The
	/// barcode value is dynamically resolved from the model at render time. Height and width multipliers allow scaling of
	/// the barcode image. Optionally, a callback can be provided to access the generated barcode for further customization
	/// or processing.</remarks>
	/// <typeparam name="TModel">The type of the model used for binding barcode data and rendering properties. Must implement IPdfModel.</typeparam>
	public class PdfBarcodeSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding and barcode encoding.
		/// </summary>
		/// <param name="data">The data binding action used to retrieve or set the barcode value from the model.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding)
		{
			this.Data = data;
			this.BarcodeEncoding = barcodeEncoding;
		}

		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding, barcode encoding, and
		/// callback for generated barcodes.
		/// </summary>
		/// <param name="data">The data binding action used to associate a string value with the model. Cannot be null.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode. Determines how the barcode data is represented.</param>
		/// <param name="callback">The callback to invoke when a barcode is generated. Used to handle the generated barcode result.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, GeneratedBarcode callback)
		{
			this.Data = data;
			this.BarcodeEncoding = barcodeEncoding;
			this.Callback = callback;
		}

		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding, barcode encoding, and
		/// height multiplier.
		/// </summary>
		/// <param name="data">A binding action that provides the barcode data to be encoded. Cannot be null.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <param name="heightMultiplier">A binding that determines the vertical scaling factor for the barcode. Must be greater than zero.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier)
			: this(data, barcodeEncoding)
		{
			this.HeightMultiplier = heightMultiplier;
		}

		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding, barcode encoding,
		/// height multiplier, and barcode generation callback.
		/// </summary>
		/// <param name="data">A binding action that provides the data to be encoded in the barcode. Cannot be null.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <param name="heightMultiplier">A binding that determines the vertical scaling factor for the barcode. Must be a positive value.</param>
		/// <param name="callback">A callback function that handles the generated barcode output. Cannot be null.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, GeneratedBarcode callback)
			: this(data, barcodeEncoding, callback)
		{
			this.HeightMultiplier = heightMultiplier;
		}

		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding, barcode encoding,
		/// height multiplier, and width multiplier.	
		/// </summary>
		/// <param name="data">A binding that provides the barcode data to be rendered. Cannot be null.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <param name="heightMultiplier">A binding that determines the vertical scaling factor for the barcode. Must be greater than zero.</param>
		/// <param name="widthMultiplier">A binding that determines the horizontal scaling factor for the barcode. Must be greater than zero.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier)
			: this(data, barcodeEncoding, heightMultiplier)
		{
			this.WidthMultiplier = widthMultiplier;
		}

		/// <summary>
		/// Initializes a new instance of the PdfBarcodeSection class with the specified data binding, barcode encoding,
		/// height multiplier, width multiplier, and barcode generation callback.
		/// </summary>
		/// <param name="data">A binding action that provides the barcode data to be encoded. Cannot be null.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <param name="heightMultiplier">A binding that determines the vertical scaling factor for the barcode. Must be a positive value.</param>
		/// <param name="widthMultiplier">A binding that determines the horizontal scaling factor for the barcode. Must be a positive value.</param>
		/// <param name="callback">A callback invoked when the barcode is generated. Used to handle the generated barcode result.</param>
		public PdfBarcodeSection(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier, GeneratedBarcode callback)
			: this(data, barcodeEncoding, heightMultiplier, callback)
		{
			this.WidthMultiplier = widthMultiplier;
		}

		/// <summary>
		/// Gets or sets the bound data value for the model.
		/// </summary>
		protected BindProperty<string, TModel> Data { get; set; }

		/// <summary>
		/// Gets or sets the multiplier applied to the height value for the model.
		/// </summary>
		protected BindProperty<double, TModel> HeightMultiplier { get; set; } = 1.0;

		/// <summary>
		/// Gets or sets the multiplier applied to the width calculation for the model.
		/// </summary>
		protected BindProperty<double, TModel> WidthMultiplier { get; set; } = 1.0;

		/// <summary>
		/// Gets or sets the encoding scheme used for barcode generation and interpretation.
		/// </summary>
		protected BarcodeEncoding BarcodeEncoding { get; set; }

		/// <summary>
		/// Gets or sets the generated barcode associated with the callback operation.
		/// </summary>
		protected GeneratedBarcode Callback { get; set; }

		/// <summary>
		/// Renders a barcode image onto the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>The barcode is generated and styled based on the resolved model and style parameters. The image
		/// is centered within the specified bounds. This method does not throw exceptions for rendering failures; instead, it
		/// returns <see langword="false"/> if rendering is unsuccessful.</remarks>
		/// <param name="g">The PDF grid page on which the barcode image will be rendered.</param>
		/// <param name="m">The data model used to resolve barcode content and rendering parameters.</param>
		/// <param name="bounds">The bounds within the grid that define the area for rendering the barcode image.</param>
		/// <returns>A task that represents the asynchronous rendering operation. The result is <see langword="true"/> if rendering
		/// succeeds; otherwise, <see langword="false"/>.</returns>
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
