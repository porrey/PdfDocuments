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
	/// Specifies the layout mode for arranging child sections within a container.
	/// </summary>
	/// <remarks>Use this enumeration to control whether child sections are stacked horizontally or vertically. The
	/// selected mode determines the visual arrangement of contained sections.</remarks>
	public enum PdfSectionsLayoutMode
	{
		/// <summary>
		/// Specifies that child sections are arranged horizontally in the layout.
		/// </summary>
		HorizontalStacking,
		/// <summary>
		/// Specifies that child sections are arranged in a vertical stack.
		/// </summary>
		VerticalStacking,
		/// <summary>
		/// Specifies that child sections are arranged on top of each other.
		/// </summary>
		OverlayStacking
	}
}