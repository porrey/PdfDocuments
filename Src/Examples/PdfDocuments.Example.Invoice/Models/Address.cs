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
	/// Represents a physical or mailing address, including contact information such as name and phone number.
	/// </summary>
	public class Address
	{
		/// <summary>
		/// Gets or sets the name associated with the current instance.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the first line of the street address.
		/// </summary>
		public string AddressLine { get; set; }

		/// <summary>
		/// Gets or sets the combined city, state, and ZIP code information.
		/// </summary>
		public string CityStateZip { get; set; }

		/// <summary>
		/// Gets or sets the phone number associated with the contact.
		/// </summary>
		public string Phone { get; set; }
	}
}
