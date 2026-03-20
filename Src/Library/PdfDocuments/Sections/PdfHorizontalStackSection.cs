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
	/// Represents a section in a PDF document that arranges its child sections horizontally, distributing available space
	/// according to relative widths or evenly when unspecified.
	/// </summary>
	/// <remarks>Child sections are laid out side by side within the section's bounds. Sections with specified
	/// relative widths receive proportional space, while others share remaining space equally. Padding and layout are
	/// applied to each child section. This class is typically used to compose complex PDF layouts by stacking multiple
	/// sections horizontally.</remarks>
	/// <typeparam name="TModel">The model type used for rendering and layout operations. Must implement the IPdfModel interface.</typeparam>
	public class PdfHorizontalStackSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfHorizontalStackSection class, which represents a section that arranges PDF
		/// elements horizontally.
		/// </summary>
		/// <remarks>Use this constructor to create a horizontal stack section for organizing PDF content side by
		/// side. This section can be added to a PDF document to group elements in a horizontal layout.</remarks>
		public PdfHorizontalStackSection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfHorizontalStackSection class with the specified child sections.
		/// </summary>
		/// <remarks>Each child section will be rendered side by side in the order provided. Use this constructor to
		/// compose complex layouts from multiple sections.</remarks>
		/// <param name="children">An array of child sections to be arranged horizontally within this section. Cannot be null or contain null
		/// elements.</param>
		public PdfHorizontalStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

		/// <summary>
		/// Arranges and lays out child sections within the specified grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>Child sections are arranged left to right, with column widths determined by relative or absolute
		/// specifications. Padding is applied to each section before layout. If any section fails to layout, the operation
		/// returns <see langword="false"/>.</remarks>
		/// <param name="g">The PDF grid page on which the child sections are to be laid out.</param>
		/// <param name="m">The model instance containing data used for layout calculations and rendering.</param>
		/// <param name="bounds">The bounds defining the area and column/row constraints for layout within the grid page.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all child sections
		/// were successfully laid out; otherwise, <see langword="false"/>.</returns>
		protected override async Task<bool> OnLayoutChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Get style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);
			PdfSpacing padding = style.Padding.Resolve(g, m);

			//
			// Get a list of each section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(g, m)).ToArray();

			//
			// Determine the width of each item. First divide the list
			// into two sets: sections with a relative width and sections
			// without. Those sections without get the remaining space
			// evenly divided.
			//
			foreach (IPdfSection<TModel> section in sections.Where(t => t.RelativeWidths.Resolve(g, m)[0] != 0))
			{
				await section.SetActualColumns((int)(section.RelativeWidths.Resolve(g, m)[0] * bounds.Columns));
				await section.SetActualRows(bounds.Rows);
			}

			//
			// Get the sum of the height of the previous sections.
			//
			int usedColumns = sections.Where(t => t.RelativeWidths.Resolve(g, m)[0] != 0).Sum(t => t.ActualBounds.Columns);

			//
			// Get the remaining rows.
			//
			int remainingColumns = bounds.Columns - usedColumns;

			//
			// Get a count of sections where the relative height is not specified.
			//
			int nonRelativeSectionCount = sections.Where(t => t.RelativeWidths.Resolve(g, m)[0] == 0).Count();

			if (nonRelativeSectionCount > 0)
			{
				//
				// Divide the remaining columns evenly among these sections.
				//
				int columnsPerSection = (int)(remainingColumns / nonRelativeSectionCount);

				//
				// Assign the rows to the remaining sections.
				//
				IPdfSection<TModel>[] sectionList = sections.Where(t => t.RelativeWidths.Resolve(g, m)[0] == 0).ToArray();

				foreach (IPdfSection<TModel> section in sectionList)
				{
					if (section != sectionList.Last())
					{
						//
						// Assign the columns calculated dividing the remaining
						// columns by the number of sections.
						//
						await section.SetActualColumns(columnsPerSection);
						await section.SetActualRows(bounds.Rows);
						remainingColumns -= columnsPerSection;
					}
					else
					{
						//
						// If the remaining rows was not evenly divisible by the
						// number of sections, this will assign all remaining columns
						// to the last section.
						//
						await section.SetActualColumns(remainingColumns);
						await section.SetActualRows(bounds.Rows);
					}
				}
			}

			//
			// Now align the sections left to right.
			//
			int left = bounds.LeftColumn;

			foreach (IPdfSection<TModel> section in sections)
			{
				section.ActualBounds.LeftColumn = left;
				left = section.ActualBounds.RightColumn + 1;
				section.ActualBounds.TopRow = bounds.TopRow;
			}

			//
			// Apply padding
			//
			foreach (IPdfSection<TModel> section in sections)
			{
				section.ActualBounds = section.ActualBounds.SubtractBounds(g, m, padding);
			}

			//
			// Allow each section to perform a layout.
			//
			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.LayoutAsync(g, m))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}
	}
}
