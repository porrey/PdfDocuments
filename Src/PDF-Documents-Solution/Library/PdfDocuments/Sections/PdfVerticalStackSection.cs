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
using System.Threading.Tasks;

namespace PdfDocuments
{
	public class PdfVerticalStackSection<TModel> : PdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfVerticalStackSection()
		{
		}

		public PdfVerticalStackSection(params IPdfSection<TModel>[] children)
			: base(children)
		{
		}

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
			foreach (IPdfSection<TModel> section in sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) != 0))
			{
				await section.SetActualRows((int)(section.RelativeHeight.Resolve(gridPage, model) * bounds.Rows));
				await section.SetActualColumns(bounds.Columns);
			}

			//
			// Get the sum of the height of the previous sections.
			//
			int usedRows = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) != 0).Sum(t => t.ActualBounds.Rows);

			//
			// Get the remaining rows.
			//
			int remainingRows = bounds.Rows - usedRows;

			//
			// Get a count of sections where the relative height is not specified.
			//
			int nonRelativeSectionCount = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) == 0).Count();

			if (nonRelativeSectionCount > 0)
			{
				//
				// Divide the remaining rows evenly among these sections.
				//
				int rowsPerSection = (int)(remainingRows / nonRelativeSectionCount);

				//
				// Assign the rows to the remaining sections.
				//
				IPdfSection<TModel>[] sectionList = sections.Where(t => t.RelativeHeight.Resolve(gridPage, model) == 0).ToArray();

				foreach (IPdfSection<TModel> section in sectionList)
				{
					if (section != sectionList.Last())
					{
						//
						// Assign the rows calculated dividing the remaining
						// rows by the number of sections.
						//
						await section.SetActualRows(rowsPerSection);
						await section.SetActualColumns(bounds.Columns);
						remainingRows -= rowsPerSection;
					}
					else
					{
						//
						// If the remaining rows was not evenly divisible by the
						// number of sections, this will assign ll remaining rows
						// to the last section.
						//
						await section.SetActualRows(remainingRows);
						await section.SetActualColumns(bounds.Columns);
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
