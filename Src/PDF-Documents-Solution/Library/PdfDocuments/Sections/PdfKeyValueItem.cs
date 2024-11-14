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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfKeyValueItem<TModel>
		where TModel : IPdfModel
	{
		public PdfKeyValueItem()
		{
		}

		public PdfKeyValueItem(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		public PdfKeyValueItem(string key, BindProperty<string, TModel> value)
		{
			this.Key = key;
			this.Value = value;
		}

		public PdfKeyValueItem(string key, BindPropertyAction<string, TModel> value)
		{
			this.Key = key;
			this.Value = value;
		}

		public string Key { get; set; }
		public BindProperty<string, TModel> Value { get; set; }
	}
}
