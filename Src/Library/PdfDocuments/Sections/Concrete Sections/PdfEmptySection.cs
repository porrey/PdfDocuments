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
	/// Represents a PDF section that contains no content. This class can be used to insert an empty section into a PDF
	/// document template.
	/// </summary>
	/// <typeparam name="TModel">The type of the model associated with the PDF section. Must implement the IPdfModel interface.</typeparam>
	public class PdfEmptySection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfEmptySection class with rendering disabled.
		/// </summary>
		public PdfEmptySection()
		{
		}

		/// <summary>
		/// Performs asynchronous initialization logic for the grid page using the specified model and bounds.
		/// </summary>
		/// <param name="g">The grid page to initialize.</param>
		/// <param name="m">The model containing data or state used for initialization.</param>
		/// <param name="bounds">The bounds within which the grid page should be initialized.</param>
		/// <returns>A task that represents the asynchronous initialization operation.</returns>
		protected override Task OnInitializeAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (this.Children.Count != 0)
			{
				this.Children.Clear();
			}

			return Task.CompletedTask;
		}
	}
}
