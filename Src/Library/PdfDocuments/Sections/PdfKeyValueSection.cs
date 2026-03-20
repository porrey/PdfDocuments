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
	/// Represents a PDF section that displays a collection of key-value pairs using a specified model type.
	/// </summary>
	/// <remarks>Use this class to render structured key-value data within a PDF document. Each item in the section
	/// is displayed with its key and corresponding value, formatted according to the resolved styles. The section is
	/// suitable for scenarios where labeled values need to be presented, such as metadata, summaries, or property
	/// lists.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for the key-value items. Must implement the IPdfModel interface.</typeparam>
	public class PdfKeyValueSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfKeyValueSection class with the specified key-value items.
		/// </summary>
		/// <param name="values">An array of PdfKeyValueItem<![CDATA[<TModel>]]>; objects to include in the section. Can be empty to create an empty section.</param>
		public PdfKeyValueSection(params PdfKeyValueItem<TModel>[] values)
		{
			foreach (PdfKeyValueItem<TModel> value in values)
			{
				this.Items.Add(value);
			}
		}

		/// <summary>
		/// Gets the collection of key-value items associated with the current model.
		/// </summary>
		/// <remarks>The collection is read-only and contains all key-value pairs relevant to the instance.
		/// Modifications to the collection itself are not supported; however, individual items within the collection may be
		/// modified if their types allow it.</remarks>
		public virtual IList<PdfKeyValueItem<TModel>> Items { get; } = [];

		/// <summary>
		/// Renders key-value items onto the specified PDF grid page asynchronously.
		/// </summary>
		/// <remarks>This method arranges and renders each key-value pair within the provided bounds, applying styles
		/// and layout based on the relative widths. The rendering is performed asynchronously, but the operation completes
		/// immediately.</remarks>
		/// <param name="g">The PDF grid page on which the key-value items will be rendered.</param>
		/// <param name="m">The model providing data and context for rendering the items.</param>
		/// <param name="bounds">The bounds within the grid page that define the area for rendering the items.</param>
		/// <returns>A task that represents the asynchronous rendering operation. The task result is <see langword="true"/> if
		/// rendering completes successfully.</returns>
		protected override Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get the styles.
			//
			PdfStyle<TModel> keyStyle = this.ResolveStyle(1);
			PdfStyle<TModel> valueStyle = this.ResolveStyle(2);

			//
			// Get the relative width of the sections.
			//
			double[] widths = keyStyle.RelativeWidths.Resolve(g, m);
			double relativeWidth = widths.Length > 0 ? widths[0] : .5;

			//
			// Determine the width
			//
			int keyWidth = (int)(bounds.Columns * relativeWidth);

			//
			// Get the starting point for the top of the text.
			//
			int top = bounds.TopRow;

			//
			// Set the initial top and the left for the name and values.
			//
			foreach (PdfKeyValueItem<TModel> item in this.Items)
			{
				//
				// Draw the Key
				//
				PdfTextElement<TModel> keyElement = new PdfTextElement<TModel>(item.Key);
				PdfSize keySize = keyElement.Measure(g, m, keyStyle);
				PdfBounds keyBounds = new PdfBounds(bounds.LeftColumn, top, keyWidth, keySize.Rows);
				keyElement.Render(g, m, keyBounds, keyStyle);

				//
				// Draw the Value
				//
				PdfTextElement<TModel> valueElement = new PdfTextElement<TModel>(item.Value.Resolve(g, m));
				PdfSize valueSize = valueElement.Measure(g, m, valueStyle);
				PdfBounds valueBounds = new PdfBounds(bounds.LeftColumn + keyWidth, top, bounds.Columns - keyWidth, valueSize.Rows);
				valueElement.Render(g, m, valueBounds, valueStyle);

				top += (new int[] { keySize.Rows, valueSize.Rows }).Max();
			}

			return Task.FromResult(returnValue);
		}
	}
}
