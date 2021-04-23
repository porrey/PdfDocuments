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
using PdfDocuments.Barcode;
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocuments
{
	public class PdfGridPage : IPdfGridPage
	{
		public string CurrentService { get; set; }
		public ITheme Theme { get; set; }
		public string DocumentTitle { get; set; }
		public PdfDocument Document { get; set; }
		public PdfPage Page { get; set; }
		public IPdfGrid Grid { get; set; }
		public int PageNumber { get; set; }
		public XGraphics Graphics { get; set; }
		public string ImagePath { get; set; }
		public DebugMode DebugMode { get; set; }
		public IBarcodeGenerator BarcodeGenerator { get; set; }
	}
}
