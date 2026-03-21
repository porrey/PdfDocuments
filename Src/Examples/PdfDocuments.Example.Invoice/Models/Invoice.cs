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
namespace PdfDocuments.Example.Invoice
{
	/// <summary>
	/// Represents a customer invoice containing billing, payment, and itemized charge information.
	/// </summary>
	/// <remarks>The Invoice class provides details necessary for billing and payment processing, including
	/// recipient and sender addresses, payment terms, and a collection of invoice items. It is typically used to generate,
	/// display, or process invoices in financial or accounting applications.</remarks>
	public class Invoice : IPdfModel
	{
		/// <summary>
		/// Gets or sets the unique identifier for the entity.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the payment method used for the transaction.
		/// </summary>
		public string PaymentMethod { get; set; }

		/// <summary>
		/// Gets or sets the check number associated with the transaction.
		/// </summary>
		public string CheckNumber { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier assigned to the job.
		/// </summary>
		public string JobNumber { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the item is due.
		/// </summary>
		public DateTime DueDate { get; set; }

		/// <summary>
		/// Gets or sets the payment terms associated with the invoice.
		/// </summary>
		public string Terms { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the payment has been completed.
		/// </summary>
		public bool Paid { get; set; } = true;

		/// <summary>
		/// Gets or sets the date when the invoice was issued.
		/// </summary>
		public DateTime InvoiceDate { get; set; }

		/// <summary>
		/// Gets or sets the billing address associated with the order.
		/// </summary>
		public Address BillTo { get; set; }

		/// <summary>
		/// Gets or sets the address from which the bill is issued.
		/// </summary>
		public Address BillFrom { get; set; }

		/// <summary>
		/// Gets or sets the collection of items included in the invoice.
		/// </summary>
		public IEnumerable<InvoiceItem> Items { get; set; }
	}
}
