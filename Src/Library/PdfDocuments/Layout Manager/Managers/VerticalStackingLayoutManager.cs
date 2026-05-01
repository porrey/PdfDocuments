using PdfDocuments.Exceptions;

namespace PdfDocuments
{
	/// <summary>
	/// Provides a layout manager that arranges PDF sections vertically within a specified bounds on a page.
	/// </summary>
	/// <remarks>Use this layout manager to stack multiple PDF sections from top to bottom on a page. This is useful
	/// for creating documents where content should flow in a vertical sequence, such as reports or forms. The layout
	/// manager is typically used in conjunction with other PDF generation components to control the visual arrangement of
	/// content.</remarks>
	public class VerticalStackingLayoutManager : IPdfLayoutManager
	{
		/// <summary>
		/// Asynchronously arranges the specified PDF sections within the given bounds on the page using the provided model.
		/// </summary>
		/// <typeparam name="TModel">The type of the model used for layout, which must implement the IPdfModel interface.</typeparam>
		/// <param name="g">The PDF grid page on which the sections will be laid out.</param>
		/// <param name="m">The model instance providing data for the layout operation. Must not be null.</param>
		/// <param name="parentSection">The parent section containing the sections to be laid out. May be null if there is no parent.</param>
		/// <param name="sections">An array of sections to be arranged within the specified bounds. Cannot be null.</param>
		/// <param name="bounds">The bounds within which the sections should be arranged, excluding margins.</param>
		/// <returns>A task that represents the asynchronous layout operation.</returns>
		/// <exception cref="LayoutFailureException">Thrown if the layout operation fails due to a mismatch between the calculated bounds and the number of sections.</exception>
		public async Task LayoutAsync<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			//
			// Get the full bounds for the sections to be rendered within, excluding margins.
			//
			PdfBounds[] fullBounds = await this.StackSections(g, m, parentSection, sections, bounds);

			if (fullBounds.Length == sections.Length)
			{
				int i = 0;

				foreach (IPdfSection<TModel> section in sections)
				{
					//
					// Get the margin for the section.
					//
					PdfSpacing margin = section.ResolveStyle(0).Margin.Resolve(g, m);

					//
					// Set the full section bounds.
					//
					section.SectionArea = fullBounds[i].Constrain(g, m, bounds);

					//
					// Set the renderable bounds for the section to the full bounds with
					// the margin subtracted.
					//
					section.RenderArea = section.SectionArea.SubtractBounds(g, m, margin);

					//
					// Increment i.
					//
					i++;
				}
			}
			else
			{
				//
				// If the number of calculated bounds does not match the number of
				// sections, throw a layout failure exception.
				//
				throw new LayoutFailureException(parentSection.Key);
			}
		}

		private async Task<PdfBounds[]> StackSections<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			List<PdfBounds> returnValue = [];

			//
			// Get the starting column and row for rendering child sections.
			//
			int leftColumn = bounds.LeftColumn;

			//
			// Get the starting row for rendering child sections.
			//
			int topRow = bounds.TopRow;

			//
			// Calculate the heights for each section to be rendered within the specified bounds.
			//
			SectionDimension[] sectionHeights = await this.CalculateSectionHeights(g, m, sections, bounds);

			//
			// Calculate the widths for each section to be rendered within the specified bounds.
			//
			SectionDimension[] sectionWidths = await this.CalculateSectionWidths(g, m, sections, bounds);

			if (sectionHeights.Length == sections.Length && sectionWidths.Length == sections.Length)
			{
				//
				// Get the size for child sections based on the calculated heights and widths.
				//
				int i = 0;

				foreach (IPdfSection<TModel> section in sections)
				{
					//
					// Create the bounds for the child section.
					//
					PdfBounds sectionBounds = new(leftColumn, topRow, sectionWidths[i].Value, sectionHeights[i].Value);

					//
					// Add the child bounds to the return value list.
					//
					returnValue.Add(sectionBounds);

					//
					// Update the top row for the next child section if the layout mode is vertical stacking.
					//
					topRow += sectionBounds.Rows;

					//
					// Update the left column for the next child section if the layout mode is horizontal stacking.
					//
					i++;
				}
			}
			else
			{
				//
				// If the number of calculated section heights or widths do not match the number of sections,
				// throw a layout failure exception.
				//
				throw new LayoutFailureException(parentSection.Key);
			}

			return [.. returnValue];
		}

		private async Task<SectionDimension[]> CalculateSectionHeights<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			List<SectionDimension> returnValue = [];

			//
			// Calculate the heights for each section to be rendered within the specified bounds.
			//
			foreach (IPdfSection<TModel> section in sections)
			{
				//
				// Check if there is a fixed height specified.
				//
				int? fixedHeight = section.ResolveStyle(0)?.FixedHeight?.Resolve(g, m);

				if (fixedHeight.HasValue)
				{
					int rows = fixedHeight.Value;
					returnValue.Add(new SectionDimension(true, rows));
				}
				else
				{
					double height = section.ResolveStyle(0).RelativeHeight.Resolve(g, m);
					int rows = (int)(bounds.Rows * (height == 0 ? 1 : height));
					returnValue.Add(new SectionDimension(false, rows));
				}
			}

			if (returnValue.Sum(t => t.Value) > bounds.Rows)
			{
				//
				// If the total calculated height exceeds the available rows in the bounds,
				// normalize the section heights to fit within the bounds.
				//
				await this.NormalizeSectionDimensions(returnValue, bounds.Rows);
			}

			return [.. returnValue];
		}

		private async Task<SectionDimension[]> CalculateSectionWidths<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			List<SectionDimension> returnValue = [];

			foreach (IPdfSection<TModel> section in sections)
			{
				//
				// Check if there is a fixed width specified.
				//
				int[] fixedWidth = section.ResolveStyle(0).FixedWidths.Resolve(g, m);

				if (fixedWidth.Length > 0)
				{
					int columns = fixedWidth[0];
					returnValue.Add(new SectionDimension(true, columns));
				}
				else
				{
					double width = section.ResolveStyle(0).RelativeWidths.Resolve(g, m)[0];
					int columns = (int)(bounds.Columns * (width == 0 ? 1 : width));
					returnValue.Add(new SectionDimension(false, columns));
				}
			}

			//if (returnValue.Sum(t => t.Value) > bounds.Columns)
			//{
			//	//
			//	// If the total calculated width exceeds the available columns in the bounds,
			//	// normalize the section widths to fit within the bounds.
			//	//
			//	await this.NormalizeSectionDimensions(returnValue, bounds.Columns);
			//}

			return [.. returnValue];
		}

		private Task NormalizeSectionDimensions(List<SectionDimension> sectionDimensions, int totalSize)
		{
			int fixedSize = sectionDimensions.Where(s => s.Fixed).Sum(s => s.Value);
			int remainingSize = totalSize - fixedSize;
			int flexibleSections = sectionDimensions.Count(s => !s.Fixed);

			if (flexibleSections > 0)
			{
				int rowsPerFlexibleSection = remainingSize / flexibleSections;
			
				foreach (SectionDimension section in sectionDimensions.Where(s => !s.Fixed))
				{
					section.Value = rowsPerFlexibleSection;
				}
			}

			return Task.CompletedTask;
		}
	}
}
