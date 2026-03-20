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
	/// Represents a page within a PDF grid document, providing access to page-specific content, metadata, and rendering
	/// resources.
	/// </summary>
	/// <remarks>The PdfGridPage class encapsulates information and resources related to a single page in a PDF
	/// grid, including the associated document, grid layout, graphics context, and metadata such as page number and title.
	/// This class is typically used when generating or manipulating PDF documents with grid-based layouts. Thread safety
	/// is not guaranteed; access from multiple threads should be synchronized.</remarks>
	public class PdfGridPage
	{
		/// <summary>
		/// Gets or sets the name of the currently active service.
		/// </summary>
		public virtual string CurrentService { get; set; }

		/// <summary>
		/// Gets or sets the title of the document.
		/// </summary>
		public virtual string DocumentTitle { get; set; }

		/// <summary>
		/// Gets or sets the PDF document associated with this instance.
		/// </summary>
		/// <remarks>Setting this property replaces the current document. Accessing this property allows manipulation
		/// of the PDF's structure and content. The property may be null if no document is loaded.</remarks>
		public virtual PdfDocument Document { get; set; }

		/// <summary>
		/// Gets or sets the PDF page associated with this instance.
		/// </summary>
		public virtual PdfPage Page { get; set; }

		/// <summary>
		/// Gets or sets the grid layout used for rendering tabular data in the PDF document.
		/// </summary>
		public virtual PdfGrid Grid { get; set; }

		/// <summary>
		/// Gets or sets the current page number in a paginated collection.
		/// </summary>
		public virtual int PageNumber { get; set; }

		/// <summary>
		/// Gets or sets the graphics context used for drawing operations on the associated surface.
		/// </summary>
		public virtual XGraphics Graphics { get; set; }

		/// <summary>
		/// Gets or sets the file system path to the associated image.
		/// </summary>
		public virtual string ImagePath { get; set; }

		/// <summary>
		/// Gets or sets the debug mode for the application.
		/// </summary>
		public virtual DebugMode DebugMode { get; set; }
	}
}
