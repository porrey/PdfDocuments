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
	/// Specifies debug display modes that can be enabled to reveal or modify visual elements during development.
	/// </summary>
	/// <remarks>This enumeration supports bitwise combination of its values due to the <see
	/// cref="FlagsAttribute"/>. Multiple modes can be enabled simultaneously to customize the debugging experience.
	/// Typical usage includes toggling visual overlays or hiding details for UI inspection.</remarks>
	[Flags]
	public enum DebugMode
	{
		/// <summary>
		/// Specifies that no options are set.
		/// </summary>
		None = 1 << 0,

		/// <summary>
		/// Specifies the option to reveal the grid in the display or interface.
		/// </summary>
		/// <remarks>Use this value to enable grid visibility when configuring display options. This flag can be
		/// combined with other options using bitwise operations.</remarks>
		RevealGrid = 1 << 1,

		/// <summary>
		/// Specifies that the layout should be revealed, typically for debugging or visualization purposes.
		/// </summary>
		RevealLayout = 1 << 2,

		/// <summary>
		/// Specifies that detailed report information should be hidden when displaying content.
		/// </summary>
		HideDetails = 1 << 3,

		/// <summary>
		/// Specifies that font details should be revealed in the output.
		/// </summary>
		RevealFontDetails = 1 << 4,

		/// <summary>
		/// Specifies that text should be rendered with an outline effect.
		/// </summary>
		OutlineText = 1 << 5
	}
}
