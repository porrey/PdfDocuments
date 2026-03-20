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
using PdfSharp.Pdf;

namespace PdfDocuments
{
	/// <summary>
	/// Provides extension methods for the PdfPage type to enhance functionality and convenience.
	/// </summary>
	/// <remarks>This class contains static methods that extend the PdfPage type, allowing for additional operations
	/// such as retrieving page resolution. All methods are intended to be used as extension methods and require a valid
	/// PdfPage instance.</remarks>
	public static class PdfPageExtensions
	{
		/// <summary>
		/// Calculates the resolution of the specified PDF page in user units per point.
		/// </summary>
		/// <remarks>The resolution is calculated based on the ratio of the page's user unit value to its point value
		/// for both width and height. This can be useful for rendering or scaling operations where precise page measurements
		/// are required.</remarks>
		/// <param name="page">The PDF page for which to determine the resolution. Cannot be null.</param>
		/// <returns>An XSize structure representing the width and height resolution of the page, measured in user units per point.</returns>
		public static XSize GetPageResolution(this PdfPage page)
		{
			return new XSize()
			{
				Width = page.Width.Value / page.Width.Point,
				Height = page.Height.Value / page.Height.Point
            };
		}
	}
}
