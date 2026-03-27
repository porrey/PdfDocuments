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
	/// Provides extension methods for working with the DebugMode enumeration, enabling convenient flag manipulation and
	/// checking.
	/// </summary>
	/// <remarks>These extension methods simplify common operations when using DebugMode as a bit field, such as
	/// determining whether a specific flag is set or updating flags. The methods are intended for use with DebugMode
	/// values that represent combinations of flags.</remarks>
	public static class PdfDebugModeExtensions
	{
		/// <summary>
		/// Determines whether the specified debug mode value includes the given flag.
		/// </summary>
		/// <remarks>Use this method to test whether one or more flags are present in a composite DebugMode value.
		/// This is useful when DebugMode is defined as a bit field enumeration.</remarks>
		/// <param name="debugMode">The debug mode value to evaluate for the presence of the specified flag.</param>
		/// <param name="checkFlag">The flag to check for within the debug mode value.</param>
		/// <returns>true if all bits in checkFlag are set in debugMode; otherwise, false.</returns>
		public static bool HasFlag(this DebugMode debugMode, DebugMode checkFlag)
		{
			return ((debugMode & checkFlag) == checkFlag);
		}

		/// <summary>
		/// Sets or clears the specified debug flag in the given debug mode value.
		/// </summary>
		/// <param name="debugMode">The current debug mode value to modify.</param>
		/// <param name="flag">The debug flag to set or clear within the debug mode value.</param>
		/// <param name="set">A value indicating whether to set (<see langword="true"/>) or clear (<see langword="false"/>) the specified flag.
		/// Defaults to <see langword="true"/>.</param>
		/// <returns>A new <see cref="DebugMode"/> value with the specified flag set or cleared as indicated.</returns>
		public static DebugMode SetFlag(this DebugMode debugMode, DebugMode flag, bool set = true)
		{
			if (set)
			{
				debugMode |= flag;
			}
			else
			{
				debugMode &= ~flag;
			}

			return debugMode;
		}
	}
}
