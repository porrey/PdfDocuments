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
using System.Linq;
using System.Threading.Tasks;
using PdfDocuments.Abstractions;

namespace PdfDocuments
{
	public class PdfHorizontalStackSection<TModel> : PdfSection<TModel>
	{
		protected override async Task<bool> OnLayoutChildrenAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Get a list of each section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Invoke(gridPage, model)).ToArray();

			//
			// Determine the width of each item. First divide the list
			// into two sets: sections with a relative width and sections
			// without. Those sections without get the remaining space
			// evenly divided.
			//
			foreach (IPdfSection<TModel> section in sections.Where(t => t.RelativeWidth.Invoke(gridPage, model) != 0))
			{
				await section.SetActualColumns((int)(section.RelativeWidth.Invoke(gridPage, model) * this.ActualBounds.Columns));
				await section.SetActualRows(this.ActualBounds.Rows);
			}

			//
			// Get the sum of the height of the previous sections.
			//
			int usedColumns = sections.Where(t => t.RelativeWidth.Invoke(gridPage, model) != 0).Sum(t => t.ActualBounds.Columns);

			//
			// Get the remaining rows.
			//
			int remainingColumns = this.ActualBounds.Columns - usedColumns;

			//
			// Get a count of sections where the relative height is not specified.
			//
			int nonRelativeSectionCount = sections.Where(t => t.RelativeWidth.Invoke(gridPage, model) == 0).Count();

			if (nonRelativeSectionCount > 0)
			{
				//
				// Divide the remaining columns evenly among these sections.
				//
				int columnsPerSection = (int)(remainingColumns / nonRelativeSectionCount);

				//
				// Assign the rows to the remaining sections.
				//
				IPdfSection<TModel>[] sectionList = sections.Where(t => t.RelativeWidth.Invoke(gridPage, model) == 0).ToArray();

				foreach (IPdfSection<TModel> section in sectionList)
				{
					if (section != sectionList.Last())
					{
						//
						// Assign the columns calculated dividing the remaining
						// columns by the number of sections.
						//
						await section.SetActualColumns(columnsPerSection);
						await section.SetActualRows(this.ActualBounds.Rows);
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
						await section.SetActualRows(this.ActualBounds.Rows);
					}
				}
			}

			//
			// Now align the sections left to right.
			//
			int left = this.ActualBounds.LeftColumn;

			foreach (IPdfSection<TModel> section in sections)
			{
				section.ActualBounds.LeftColumn = left;
				left = section.ActualBounds.RightColumn + 1;
				section.ActualBounds.TopRow = this.ActualBounds.TopRow;
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
