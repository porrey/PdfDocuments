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
	/// Represents a PDF section that renders text with automatic word wrapping within the specified bounds.
	/// </summary>
	/// <remarks>This section uses the resolved style and padding to determine how text is wrapped and positioned.
	/// It is useful for displaying paragraphs or blocks of text that need to fit within a constrained area of a PDF
	/// page.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for rendering the PDF section. Must implement the IPdfModel interface.</typeparam>
	[Obsolete("Use PdfTextSection with appropriate styling instead. This class is retained for backward compatibility but may be removed in future versions.")]
	public class PdfWrappingTextSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
	}
}
