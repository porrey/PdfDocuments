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
using PdfSharp.Drawing;
using System.Collections.Generic;

namespace PdfDocuments
{
	public class PdfStyleManager<TModel> : Dictionary<string, PdfStyle<TModel>>, IPdfStyleManager<TModel>
			where TModel : IPdfModel
	{
		public const string Default = "Default";
		public const string Debug = "Debug";

		public PdfStyleManager()
		{
			//
			// Add default style.
			//
			this.Add(PdfStyleManager<TModel>.Default, new PdfStyle<TModel>());
			this.Add(PdfStyleManager<TModel>.Debug, new PdfStyle<TModel>() { Font = new XFont("Arial", 8, XFontStyleEx.Regular) });
		}

		public virtual PdfStyle<TModel> GetStyle(string name)
		{
			PdfStyle<TModel> returnValue = this[PdfStyleManager<TModel>.Default];

			if (this.ContainsKey(name))
			{
				returnValue = this[name];
			}

			return returnValue;
		}

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
