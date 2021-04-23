/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System.Threading.Tasks;

namespace PdfDocuments
{
	public delegate TResult BindPropertyAction<TResult, TModel>(IPdfGridPage gp, TModel m);

	public class BindProperty<TProperty, TModel>
	{
		public BindProperty()
		{
		}

		public BindProperty(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
		}

		public BindPropertyAction<TProperty, TModel> Action { get; set; }

		public TProperty Invoke(IPdfGridPage gp, TModel m)
		{
			TProperty returnValue = default;

			if (this.Action != null)
			{
				returnValue = this.Action.Invoke(gp, m);
			}

			return returnValue;
		}

		public Task<TProperty> GetValueAsync(IPdfGridPage gp, TModel m)
		{
			return Task.FromResult(this.Invoke(gp, m));
		}

		public void SetAction(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
		}

		public Task SetActionAsync(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
			return Task.FromResult(0);
		}

		public static implicit operator BindProperty<TProperty, TModel>(TProperty source)
		{
			return new BindProperty<TProperty, TModel>()
			{
				Action = (gp, m) => { return source; }
			};
		}

		public static implicit operator BindProperty<TProperty, TModel>(BindPropertyAction<TProperty, TModel> source)
		{
			return new BindProperty<TProperty, TModel>()
			{
				Action = source
			};
		}
	}
}