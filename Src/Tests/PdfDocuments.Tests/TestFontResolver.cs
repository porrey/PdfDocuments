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

namespace PdfDocuments.Tests
{
	/// <summary>
	/// A minimal font resolver that maps common font family names to Liberation font files
	/// available on Linux CI environments, allowing PdfSharp to resolve fonts without
	/// a Windows font store.
	/// </summary>
	internal sealed class TestFontResolver : IFontResolver
	{
		private static readonly string FontFolder = "/usr/share/fonts/truetype/liberation";

		private static readonly Dictionary<string, string> FaceMap =
			new(StringComparer.OrdinalIgnoreCase)
			{
				["Arial#R"]           = "LiberationSans-Regular.ttf",
				["Arial#B"]           = "LiberationSans-Bold.ttf",
				["Arial#I"]           = "LiberationSans-Italic.ttf",
				["Arial#BI"]          = "LiberationSans-BoldItalic.ttf",
				["Liberation Sans#R"] = "LiberationSans-Regular.ttf",
				["Liberation Sans#B"] = "LiberationSans-Bold.ttf",
				["Liberation Sans#I"] = "LiberationSans-Italic.ttf",
				["Liberation Sans#BI"]= "LiberationSans-BoldItalic.ttf",
			};

		public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
		{
			string suffix = (bold, italic) switch
			{
				(true, true)  => "#BI",
				(true, false) => "#B",
				(false, true) => "#I",
				_             => "#R",
			};

			string key = familyName + suffix;

			if (FaceMap.ContainsKey(key))
			{
				return new FontResolverInfo(key);
			}

			// Fall back to regular for any unknown family
			string fallbackKey = familyName + "#R";

			if (FaceMap.ContainsKey(fallbackKey))
			{
				return new FontResolverInfo(fallbackKey, mustSimulateBold: bold, mustSimulateItalic: italic);
			}

			// Last resort: use Arial regular
			return new FontResolverInfo("Arial#R", mustSimulateBold: bold, mustSimulateItalic: italic);
		}

		public byte[] GetFont(string faceName)
		{
			if (FaceMap.TryGetValue(faceName, out string? fileName))
			{
				string path = Path.Combine(FontFolder, fileName);

				if (File.Exists(path))
				{
					return File.ReadAllBytes(path);
				}
			}

			// Fall back to Arial Regular
			string defaultPath = Path.Combine(FontFolder, "LiberationSans-Regular.ttf");

			if (File.Exists(defaultPath))
			{
				return File.ReadAllBytes(defaultPath);
			}

			return [];
		}
	}
}
