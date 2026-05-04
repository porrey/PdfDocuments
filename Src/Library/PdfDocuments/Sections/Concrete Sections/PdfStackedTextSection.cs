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
	/// Represents a PDF section that renders a collection of text items stacked vertically, using customizable styles and
	/// bindings to a model.
	/// </summary>
	/// <remarks>This section allows each text item to be bound to a property of the model and rendered with
	/// distinct styles. The first item and subsequent items can use different font and alignment settings. Items with null
	/// or whitespace values are not rendered. Use this class to display multiple lines of text in a stacked layout within
	/// a PDF grid page.</remarks>
	/// <typeparam name="TModel">The type of the model used for binding text items. Must implement the IPdfModel interface.</typeparam>
	public class PdfStackedTextSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfStackedTextSection class.
		/// </summary>
		public PdfStackedTextSection()
		{
		}

		/// <summary>
		/// Gets or sets the collection of stacked items bound to the model.
		/// </summary>
		/// <remarks>Each item in the collection represents a binding between a string identifier and a model
		/// instance. The order of items in the collection may affect how they are processed or displayed.</remarks>
		public IEnumerable<BindProperty<string, TModel>> StackedItems { get; set; }

		/// <summary>
		/// Performs asynchronous initialization logic for the grid page using the specified model and bounds.
		/// </summary>
		/// <param name="g">The grid page to initialize.</param>
		/// <param name="m">The model containing data or state used for initialization.</param>
		/// <param name="bounds">The bounds within which the grid page should be initialized.</param>
		/// <returns>A task that represents the asynchronous initialization operation.</returns>
		protected override Task OnInitializeAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (this.Children.Count == 0)
			{
				int i = 0;

				foreach (BindProperty<string, TModel> item in this.StackedItems)
				{
					//
					// Get the style name for the item. If there are more items than style names,
					// use the last style name for the remaining items.
					//
					string styleName = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i) : this.StyleNames.Last();

					this.AddChildren(new PdfTextBlockSection<TModel>
					{
						Text = item,
						StyleNames = [styleName]
					});
				}

				//
				// Clear the style names so no margins or padding are applied to the section itself,
				// allowing the styles to be applied only to the individual text blocks.
				//
				this.StyleNames = [PdfStyleManager<TModel>.Default];
			}

			return Task.CompletedTask;
		}
	}
}
