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

namespace PdfDocuments.IronBarcode
{
	/// <summary>
	/// Provides static methods for creating PDF sections containing barcodes and for setting the license key required for
	/// barcode generation.
	/// </summary>
	/// <remarks>Use the methods in this class to add barcode elements to PDF documents by binding model properties
	/// and specifying barcode encoding options. The class supports customization of barcode size and allows integration
	/// with callback functions for generated barcodes. All methods are static and thread-safe. Setting the license key is
	/// required before generating barcodes if using a licensed version of IronBarCode.</remarks>
	public static class PdfBarcode
	{
		/// <summary>
		/// Sets the license key used to activate IronBarCode features.
		/// </summary>
		/// <remarks>Call this method before using any IronBarCode functionality that requires a valid license.
		/// Setting an invalid or expired license key may restrict access to premium features.</remarks>
		/// <param name="licenseKey">The license key string to apply. Cannot be null or empty.</param>
		public static void SetLicense(string licenseKey)
		{
			IronBarCode.License.LicenseKey = licenseKey;
		}

		/// <summary>
		/// Creates a PDF section that displays a barcode generated from the specified data using the given encoding.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model associated with the section. Must implement IPdfModel.</typeparam>
		/// <param name="data">A binding action that provides the string data to be encoded as a barcode in the PDF section.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <returns>A PDF section that renders the barcode based on the provided data and encoding.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding);
		}

		/// <summary>
		/// Creates a PDF section that displays a barcode generated from the specified data using the given encoding.
		/// </summary>
		/// <typeparam name="TModel">The model type that implements IPdfModel and provides context for the PDF section.</typeparam>
		/// <param name="data">The bound property containing the string value to encode as a barcode in the PDF section.</param>
		/// <param name="barcodeEncoding">The encoding format to use when generating the barcode from the provided data.</param>
		/// <returns>A PDF section instance that renders the barcode based on the specified data and encoding.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding);
		}

		/// <summary>
		/// Creates a PDF section that renders a barcode using the specified data and encoding.
		/// </summary>
		/// <remarks>The barcode section supports dynamic data binding and customization through the callback. The
		/// height multiplier allows for flexible sizing of the barcode within the PDF layout.</remarks>
		/// <typeparam name="TModel">The type of the PDF model used for data binding within the section.</typeparam>
		/// <param name="data">A binding action that provides the string data to encode in the barcode.</param>
		/// <param name="barcodeEncoding">The encoding format to use for generating the barcode.</param>
		/// <param name="heightMultiplier">A binding action that specifies the height multiplier for the barcode, affecting its vertical scaling.</param>
		/// <param name="callback">An optional callback invoked with the generated barcode, allowing for additional customization or processing.</param>
		/// <returns>A PDF section configured to display a barcode based on the provided data and encoding.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindPropertyAction<double, TModel> heightMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, callback);
		}

		/// <summary>
		/// Creates a PDF section that renders a barcode using the specified data, encoding, and height multiplier.
		/// </summary>
		/// <typeparam name="TModel">The type of the PDF model to which the barcode section is bound.</typeparam>
		/// <param name="data">The property containing the string value to encode in the barcode. Cannot be null.</param>
		/// <param name="barcodeEncoding">The barcode encoding format to use for rendering the barcode.</param>
		/// <param name="heightMultiplier">The property specifying the height multiplier for the barcode. Must be a positive value.</param>
		/// <param name="callback">An optional callback that receives the generated barcode object for further customization or processing. If null,
		/// no callback is invoked.</param>
		/// <returns>A PDF section instance that displays the barcode with the specified configuration.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, callback);
		}

		/// <summary>
		/// Creates a PDF section that renders a barcode based on the specified model data and encoding.
		/// </summary>
		/// <remarks>Use this method to add barcode elements to a PDF document, with support for dynamic data binding
		/// and customization. The section can be integrated into larger PDF generation workflows.</remarks>
		/// <typeparam name="TModel">The type of the PDF model used to bind barcode data and properties.</typeparam>
		/// <param name="data">A binding action that provides the string value to encode in the barcode from the model.</param>
		/// <param name="barcodeEncoding">The barcode encoding format to use for rendering the barcode.</param>
		/// <param name="heightMultiplier">A binding action that determines the height multiplier for the barcode, allowing dynamic sizing based on the
		/// model.</param>
		/// <param name="widthMultiplier">A binding action that determines the width multiplier for the barcode, allowing dynamic sizing based on the model.</param>
		/// <param name="callback">An optional callback that receives the generated barcode for further customization or processing.</param>
		/// <returns>A PDF section that displays the barcode with the specified encoding and sizing, bound to the provided model data.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindPropertyAction<double, TModel> heightMultiplier, BindPropertyAction<double, TModel> widthMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, widthMultiplier, callback);
		}
		
		/// <summary>
		/// Creates a PDF section that renders a barcode using the specified data and encoding.
		/// </summary>
		/// <remarks>The barcode section supports customization of barcode size and encoding. The callback can be used
		/// to access the generated barcode for further processing or inspection.</remarks>
		/// <typeparam name="TModel">The type of the PDF model to which the barcode section is bound.</typeparam>
		/// <param name="data">The property containing the string value to encode in the barcode. Cannot be null.</param>
		/// <param name="barcodeEncoding">The barcode encoding format to use for rendering the barcode.</param>
		/// <param name="heightMultiplier">The property specifying the multiplier for the barcode's height. Must be positive.</param>
		/// <param name="widthMultiplier">The property specifying the multiplier for the barcode's width. Must be positive.</param>
		/// <param name="callback">An optional callback invoked with the generated barcode object after rendering. If null, no callback is executed.</param>
		/// <returns>A PDF section that displays the barcode with the specified data, encoding, and size multipliers.</returns>
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, widthMultiplier, callback);
		}
	}
}
