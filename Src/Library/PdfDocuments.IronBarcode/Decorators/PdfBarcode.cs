/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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
	public static class PdfBarcode
	{
		public static void SetLicense(string licenseKey)
		{
			IronBarCode.License.LicenseKey = licenseKey;
		}

		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding);
		}

		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding);
		}


		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindPropertyAction<double, TModel> heightMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, callback);
		}

		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, callback);
		}


		public static IPdfSection<TModel> BarcodeSection<TModel>(BindPropertyAction<string, TModel> data, BarcodeEncoding barcodeEncoding, BindPropertyAction<double, TModel> heightMultiplier, BindPropertyAction<double, TModel> widthMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, widthMultiplier, callback);
		}
		
		public static IPdfSection<TModel> BarcodeSection<TModel>(BindProperty<string, TModel> data, BarcodeEncoding barcodeEncoding, BindProperty<double, TModel> heightMultiplier, BindProperty<double, TModel> widthMultiplier, GeneratedBarcode callback = null)
			where TModel : IPdfModel
		{
			return new PdfBarcodeSection<TModel>(data, barcodeEncoding, heightMultiplier, widthMultiplier, callback);
		}

	}
}
