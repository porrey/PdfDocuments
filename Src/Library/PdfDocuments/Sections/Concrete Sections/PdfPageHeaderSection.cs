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
namespace PdfDocuments
{
	/// <summary>
	/// Represents a page header section template for a PDF page, allowing customizable text content in each corner of the
	/// header.
	/// </summary>
	/// <remarks>Use this class to define header content for PDF pages, with support for binding text to the
	/// top-left, top-right, bottom-left, and bottom-right corners. Header content and styles can be dynamically resolved
	/// based on the provided model. This section is typically rendered at the top of each PDF page and can be
	/// customized for different document layouts.</remarks>
	/// <typeparam name="TModel">The model type used to bind header content and styles. Must implement the IPdfModel interface.</typeparam>
	public class PdfPageHeaderSection<TModel> : PdfPageFooterSection<TModel>
		where TModel : IPdfModel
	{
	}
}
