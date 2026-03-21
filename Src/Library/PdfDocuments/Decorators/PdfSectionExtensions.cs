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
	/// Provides extension methods for the IPdfSection<![CDATA[<TModel>]]> interface to support margin application and style resolution
	/// in PDF generation scenarios.
	/// </summary>
	/// <remarks>These extension methods simplify common operations when working with PDF sections, such as
	/// adjusting bounds for margins and resolving styles based on section configuration. Intended for use with
	/// implementations of IPdfSection<![CDATA[<TModel>]]> in PDF rendering workflows.</remarks>
	public static class PdfSectionExtensions
	{
		/// <summary>
		/// Calculates the bounds of a PDF section after applying the specified margins.
		/// </summary>
		/// <typeparam name="TModel">The type of the model used by the PDF section. Must implement IPdfModel.</typeparam>
		/// <param name="section">The PDF section whose bounds will be adjusted.</param>
		/// <param name="g">The PDF grid page context used for calculating the bounds.</param>
		/// <param name="m">The model instance associated with the section.</param>
		/// <param name="margin">The spacing values to apply as margins to the section's bounds.</param>
		/// <returns>A PdfBounds instance representing the section's bounds after margins have been applied.</returns>
		public static PdfBounds ApplyMargins<TModel>(this IPdfSection<TModel> section, PdfGridPage g, TModel m, PdfSpacing margin)
			where TModel : IPdfModel
		{
			return section.ActualBounds.SubtractBounds(g, m, margin);
		}

		/// <summary>
		/// Resolves and returns the style associated with the specified index for the given PDF section.
		/// </summary>
		/// <remarks>If the section contains no style names, the default style is returned. If the index exceeds the
		/// available style names, the first style is used.</remarks>
		/// <typeparam name="TModel">The model type associated with the PDF section. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="section">The PDF section for which to resolve the style. Cannot be null.</param>
		/// <param name="index">The zero-based index of the style to resolve. If the index is out of range, the first style is used.</param>
		/// <returns>A <see cref="PdfStyle{TModel}"/> instance corresponding to the specified style index, or the default style if no
		/// styles are defined.</returns>
		public static PdfStyle<TModel> ResolveStyle<TModel>(this IPdfSection<TModel> section, int index)
			where TModel : IPdfModel
		{
			PdfStyle<TModel> returnValue = null;

			if (section.StyleNames.Any())
			{
				if (index < section.StyleNames.Count())
				{
					returnValue = section.StyleManager.GetStyle(section.StyleNames.ElementAt(index));
				}
				else
				{
					returnValue = section.StyleManager.GetStyle(section.StyleNames.First());
				}
			}
			else
			{
				section.StyleManager.GetStyle(PdfStyleManager<TModel>.Default);
			}

			return returnValue;
		}
	}
}
