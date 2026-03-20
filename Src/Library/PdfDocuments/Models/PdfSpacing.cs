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
	/// Represents spacing values for the left, top, right, and bottom edges, typically used to define margins or padding
	/// in PDF layouts.
	/// </summary>
	/// <remarks>Use this class to specify edge spacing when rendering or manipulating PDF elements. The spacing
	/// values are measured in units consistent with the PDF rendering context, such as points or pixels. The class
	/// provides properties for each edge and supports implicit conversion from a tuple for convenience.</remarks>
	public class PdfSpacing
	{
		/// <summary>
		/// Initializes a new instance of the PdfSpacing class.
		/// </summary>
		public PdfSpacing()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfSpacing class with the specified spacing values for each side.
		/// </summary>
		/// <param name="left">The spacing value, in points, to apply to the left side. Must be non-negative.</param>
		/// <param name="top">The spacing value, in points, to apply to the top side. Must be non-negative.</param>
		/// <param name="right">The spacing value, in points, to apply to the right side. Must be non-negative.</param>
		/// <param name="bottom">The spacing value, in points, to apply to the bottom side. Must be non-negative.</param>
		public PdfSpacing(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		/// <summary>
		/// Gets or sets the maximum number of rows to return from a query.
		/// </summary>
		public virtual int Top { get; set; }

		/// <summary>
		/// Gets or sets the vertical coordinate of the lower edge of the rectangle.
		/// </summary>
		public virtual int Bottom { get; set; }

		/// <summary>
		/// Gets or sets the distance, in pixels, between the right edge of the element and the left edge of its container.
		/// </summary>
		public virtual int Right { get; set; }

		/// <summary>
		/// Gets or sets the horizontal position of the left edge of the element, in pixels.
		/// </summary>
		public virtual int Left { get; set; }

		/// <summary>
		/// Converts a tuple containing left, top, right, and bottom spacing values to a PdfSpacing instance.
		/// </summary>
		/// <param name="item">A tuple representing the spacing values. The first element specifies the left spacing, the second specifies the
		/// top spacing, the third specifies the right spacing, and the fourth specifies the bottom spacing.</param>
		public static implicit operator PdfSpacing((int Left, int Top, int Right, int Bottom) item)
		{
			return new PdfSpacing(item.Left, item.Top, item.Right, item.Bottom);
		}

		/// <summary>
		/// Returns a string that represents the current object, listing the values of the Left, Top, Right, and Bottom
		/// properties.
		/// </summary>
		/// <returns>A comma-separated string containing the values of the Left, Top, Right, and Bottom properties.</returns>
		public override string ToString()
		{
			return $"{this.Left}, {this.Top}, {this.Right}, {this.Bottom}";
		}
	}
}
