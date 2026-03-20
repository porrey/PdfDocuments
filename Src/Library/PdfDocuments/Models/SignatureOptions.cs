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
	/// Represents configurable options for customizing signature fields in a PDF model.
	/// </summary>
	/// <remarks>Use this class to specify text, image, and date-related properties for signature fields when
	/// generating or editing PDF documents. Each property is bound to the specified PDF model type, allowing dynamic
	/// customization based on model data.</remarks>
	/// <typeparam name="TModel">The type of PDF model to which the signature options are bound. Must implement the IPdfModel interface.</typeparam>
	public class SignatureOptions<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the signature text associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> SignatureText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the image data representing the signature for the model.
		/// </summary>
		public virtual BindProperty<string, TModel> SignatureImage { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the label text used to display the date field in the user interface.
		/// </summary>
		public virtual BindProperty<string, TModel> DateLabel { get; set; } = "Date";

		/// <summary>
		/// Gets or sets the date value to be bound to the model.
		/// </summary>
		public virtual BindProperty<DateTimeOffset?, TModel> Date { get; set; } = null;

		/// <summary>
		/// Gets or sets the offset of the signature image within the PDF document.
		/// </summary>
		/// <remarks>The offset is specified as a size in columns and rows, allowing precise placement of the
		/// signature image relative to the document layout. Adjust this property to control the horizontal and vertical
		/// positioning of the signature image.</remarks>
		public virtual BindProperty<PdfSize, TModel> Offset { get; set; } = new PdfSize() { Columns = 10, Rows = -2 };
	}
}
