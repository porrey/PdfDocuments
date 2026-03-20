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
	/// Defines the contract for generating PDF documents from a specific document model.
	/// </summary>
	/// <remarks>Implementations of this interface are responsible for converting a document model into a PDF
	/// format. The type of document model supported is indicated by the DocumentModelType property. The DebugMode property
	/// can be used to control diagnostic output or behavior during PDF generation.</remarks>
	public interface IPdfGenerator
	{
		/// <summary>
		/// Gets the type of the document model represented by this instance.
		/// </summary>
		Type DocumentModelType { get; }

		/// <summary>
		/// Gets or sets the debug mode configuration for the application.
		/// </summary>
		/// <remarks>Use this property to control debugging behavior, such as enabling verbose logging or diagnostic
		/// features. Changing the debug mode may affect performance and logging output.</remarks>
		DebugMode DebugMode { get; set; }
	}
}
