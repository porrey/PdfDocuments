/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System.Drawing;
using IronBarCode;
using PdfDocuments.Barcode.Abstractions;

namespace PdfDocuments.IronBarcode
{
	public class IronBarcodeGenerator : IBarcodeGenerator
	{
		public IronBarcodeGenerator()
		{
			//
			// Do not copy this license to other software applications. A license is required
			// for additional applications.
			//
			IronBarCode.License.LicenseKey = "";
		}

		public Image Create(string data, int barcodeWidth, int barcodeHeight, BarCodeType type, Color color, Color backColor)
		{
			Image returnValue = null;

			BarcodeEncoding bct = BarcodeEncoding.Code128;

			switch (type)
			{
				case BarCodeType.Code128:
					bct = BarcodeEncoding.Code128;
					break;
				case BarCodeType.Code39:
					bct = BarcodeEncoding.Code39;
					break;
			}

			GeneratedBarcode bc = BarcodeWriter.CreateBarcode(data, bct)
					.ChangeBackgroundColor(backColor)
					.ChangeBarCodeColor(color)
					//.SetMargins(0, 0, 0, 0)
					.ResizeTo(barcodeWidth, barcodeHeight)
					;

			returnValue = bc.ToImage();

			return returnValue;
		}
	}
}
