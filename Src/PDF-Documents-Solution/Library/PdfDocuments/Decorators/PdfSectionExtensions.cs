/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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

using System.Linq;

namespace PdfDocuments
{
	public static class PdfSectionExtensions
	{
		public static PdfBounds ApplyMargins<TModel>(this IPdfSection<TModel> section, PdfGridPage g, TModel m, PdfSpacing margin)
			where TModel : IPdfModel
		{
			return section.ActualBounds.SubtractBounds(g, m, margin);
		}

		public static PdfStyle<TModel> ResolveStyle<TModel>(this IPdfSection<TModel> section, int index)
			where TModel : IPdfModel
		{
			PdfStyle<TModel> returnValue = null;

			if (section.StyleNames.Count() > 0)
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
