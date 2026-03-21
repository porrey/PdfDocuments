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
	/// Represents a PDF section template that arranges child sections vertically within a grid page layout.
	/// </summary>
	/// <remarks>Child sections are stacked from top to bottom, with their heights determined by relative height
	/// settings or evenly distributed if unspecified. This class is useful for creating composite PDF layouts where
	/// multiple sections need to be rendered in a vertical sequence.</remarks>
	/// <typeparam name="TModel">The type of model used for rendering the PDF section. Must implement the IPdfModel interface.</typeparam>
	public class PdfVerticalStackSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfVerticalStackSection class.
		/// </summary>
		public PdfVerticalStackSection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfVerticalStackSection class with the specified child sections arranged
		/// vertically.
		/// </summary>
		/// <remarks>Each child section is rendered in order, from top to bottom. Use this constructor to compose
		/// complex layouts by combining multiple sections.</remarks>
		/// <param name="children">An array of child sections to be stacked vertically within this section. Cannot be null; may be empty to create an
		/// empty stack.</param>
		public PdfVerticalStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

		/// <summary>
		/// Arranges and lays out child sections within the specified grid page according to their relative heights and
		/// available bounds.
		/// </summary>
		/// <remarks>Sections with a specified relative height are allocated space proportionally, while remaining
		/// sections share the leftover space evenly. The layout operation is performed asynchronously for each
		/// section.</remarks>
		/// <param name="gridPage">The PDF grid page on which the child sections will be laid out.</param>
		/// <param name="model">The data model used to determine rendering and layout properties for each section.</param>
		/// <param name="bounds">The bounds defining the available rows and columns for layout within the grid page.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all sections were
		/// successfully laid out; otherwise, <see langword="false"/>.</returns>
		protected override async Task<bool> OnLayoutChildrenAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get a list of section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(gridPage, model)).ToArray();

			//
			// Determine the height of each item. First divide the list
			// into two sets: sections with a relative height and sections
			// without. Those sections without get the remaining space
			// evenly divided.
			//
			IEnumerable<IPdfSection<TModel>> relativeHeightSections = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) != 0 || t.MustCalculateHeight);

			foreach (IPdfSection<TModel> section in relativeHeightSections)
			{
				if (section.MustCalculateHeight)
				{
					//
					// If the section must calculate its height, we need call CalculateHeightAsync() determine the height.
					//
					int calculatedHeight = await section.CalculateHeightAsync(gridPage, model, bounds);
					await section.SetActualRowsAsync(calculatedHeight);
				}
				else
				{
					await section.SetActualRowsAsync((int)(section.RelativeHeight.Resolve(gridPage, model) * bounds.Rows));
				}

				await section.SetActualColumnsAsync(bounds.Columns);
			}

			//
			// Get the sum of the height of the previous sections.
			//
			int usedRows = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) != 0 || t.MustCalculateHeight).Sum(t => t.ActualBounds.Rows);

			//
			// Get the remaining rows.
			//
			int remainingRows = bounds.Rows - usedRows;

			//
			// Get a count of sections where the relative height is not specified.
			//
			IEnumerable<IPdfSection<TModel>> nonRelativeSections = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) == 0 && !t.MustCalculateHeight);

			if (nonRelativeSections.Any())
			{
				//
				// Divide the remaining rows evenly among these sections.
				//
				int rowsPerSection = (int)(remainingRows / nonRelativeSections.Count());

				//
				// Assign the rows to the remaining sections.
				//
				foreach (IPdfSection<TModel> section in nonRelativeSections)
				{
					if (section != nonRelativeSections.Last())
					{
						//
						// Assign the rows calculated dividing the remaining
						// rows by the number of sections.
						//
						await section.SetActualRowsAsync(rowsPerSection);
						await section.SetActualColumnsAsync(bounds.Columns);
						remainingRows -= rowsPerSection;
					}
					else
					{
						//
						// If the remaining rows was not evenly divisible by the
						// number of sections, this will assign all remaining rows
						// to the last section.
						//
						await section.SetActualRowsAsync(remainingRows);
						await section.SetActualColumnsAsync(bounds.Columns);
					}
				}
			}

			//
			// Now align the sections top to bottom.
			//
			int top = bounds.TopRow;

			foreach (IPdfSection<TModel> section in sections)
			{
				section.ActualBounds.TopRow = top;
				top = section.ActualBounds.BottomRow + 1;
				section.ActualBounds.LeftColumn = bounds.LeftColumn;
			}

			//
			// Allow each section to perform a layout.
			//
			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.LayoutAsync(gridPage, model))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}
	}
}
