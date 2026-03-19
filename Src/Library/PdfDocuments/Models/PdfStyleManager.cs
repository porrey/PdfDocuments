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
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfStyleManager<TModel> : Dictionary<string, PdfStyle<TModel>>, IPdfStyleManager<TModel>
			where TModel : IPdfModel
	{
		public const string Default = "Default";
		public const string Debug = "Debug";

		/// <summary>
		/// Initializes a new instance of the PdfStyleManager class using the default font style.
		/// </summary>
		/// <remarks>This constructor sets the font to "Arial" by default. Use this overload when no specific font is
		/// required for PDF styling.</remarks>
		public PdfStyleManager()
			: this("Arial")
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfStyleManager class with the specified default font name.
		/// </summary>
		/// <remarks>The constructor adds a default style and a debug style using the provided font name. Use this to
		/// ensure consistent font settings across generated PDF styles.</remarks>
		/// <param name="defaultFontName">The name of the font to use as the default for styles managed by this instance. Cannot be null or empty.</param>
		/// <param name="emSize">The size of the font in points. Default is 8. Use this to specify the default font size for styles.</param>
		public PdfStyleManager(string defaultFontName, double emSize = 8)
		{
			//
			// Add default style.
			//
			this.Add(PdfStyleManager<TModel>.Default, new PdfStyle<TModel>());
			this.Add(PdfStyleManager<TModel>.Debug, new PdfStyle<TModel>() { Font = new XFont(defaultFontName, emSize, XFontStyleEx.Regular) });
		}

		/// <summary>
		/// Retrieves the style associated with the specified name, or returns the default style if the name is not found.
		/// </summary>
		/// <param name="name">The name of the style to retrieve. If the specified name does not exist, the default style is returned.</param>
		/// <returns>The style corresponding to the specified name, or the default style if the name is not present.</returns>
		public virtual PdfStyle<TModel> GetStyle(string name)
		{
			PdfStyle<TModel> returnValue = this[PdfStyleManager<TModel>.Default];

			if (this.ContainsKey(name))
			{
				returnValue = this[name];
			}

			return returnValue;
		}

		/// <summary>
		/// Replaces the style associated with the specified name. If a style with the given name already exists, it is
		/// removed and replaced; otherwise, the style is added.
		/// </summary>
		/// <param name="name">The key name identifying the style to replace or add. Cannot be null.</param>
		/// <param name="style">The style to associate with the specified name. Cannot be null.</param>
		public virtual void Replace(string name, PdfStyle<TModel> style)
		{
			if (this.ContainsKey(name))
			{
				this.Remove(name);
				this.Add(name, style);
			}
			else
			{
				this.Add(name, style);
			}
		}
	}
}
