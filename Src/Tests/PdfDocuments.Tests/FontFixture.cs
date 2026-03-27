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
using PdfSharp.Fonts;
using Xunit;

namespace PdfDocuments.Tests
{
	/// <summary>
	/// Shared test fixture that ensures PdfSharp's global font resolver is initialised
	/// before any test that creates XFont instances runs.  The resolver is registered
	/// exactly once; subsequent attempts to replace it are silently ignored by PdfSharp.
	/// </summary>
	public class FontFixture
	{
		public FontFixture()
		{
			// GlobalFontSettings.FontResolver can only be set once per process.
			// Guard against being called multiple times (e.g. during parallel test runs).
			if (GlobalFontSettings.FontResolver == null)
			{
				GlobalFontSettings.FontResolver = new TestFontResolver();
			}
		}
	}

	/// <summary>
	/// xUnit collection definition that shares the FontFixture across all test classes
	/// in the "FontTests" collection.
	/// </summary>
	[CollectionDefinition("FontTests")]
	public class FontTestsCollection : ICollectionFixture<FontFixture>
	{
		// This class has no code. Its purpose is to define the collection.
	}
}
