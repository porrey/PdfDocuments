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
	public class StackingLayoutManager : IPdfLayoutManager
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
					section.RenderArea = section.SectionArea.SubtractSpacing(g, m, margin);

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

		/// <summary>
		/// Calculates and returns the layout bounds for each child section to be stacked within the specified parent bounds
		/// on a PDF grid page.
		/// </summary>
		/// <remarks>This method asynchronously determines the size and position of each child section based on the
		/// calculated heights and widths. The stacking layout may be vertical or horizontal depending on the implementation
		/// of the section dimension calculations.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF sections. Must implement IPdfModel.</typeparam>
		/// <param name="g">The PDF grid page on which the sections will be rendered.</param>
		/// <param name="m">The model instance providing data for the sections.</param>
		/// <param name="parentSection">The parent section containing the child sections to be stacked.</param>
		/// <param name="sections">An array of child sections to be stacked within the parent section.</param>
		/// <param name="bounds">The bounds within which the child sections should be arranged.</param>
		/// <returns>An array of PdfBounds representing the calculated layout for each child section. The order of the array
		/// corresponds to the order of the input sections.</returns>
		/// <exception cref="LayoutFailureException">Thrown if the number of calculated section heights or widths does not match the number of sections, indicating a
		/// layout calculation failure.</exception>
		protected virtual async Task<PdfBounds[]> StackSections<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds)
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
			PdfSectionDimension[] sectionHeights = await this.CalculateSectionHeights(g, m, parentSection, sections, bounds);

			//
			// Calculate the widths for each section to be rendered within the specified bounds.
			//
			PdfSectionDimension[] sectionWidths = await this.CalculateSectionWidths(g, m, parentSection, sections, bounds);

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

					if (parentSection.SectionLayoutMode == PdfSectionsLayoutMode.HorizontalStacking)
					{
						//
						// Update the left column for the next child section if the layout mode is horizontal stacking.
						//
						leftColumn += sectionBounds.Columns;
					}

					if (parentSection.SectionLayoutMode == PdfSectionsLayoutMode.VerticalStacking)
					{
						//
						// Update the top row for the next child section if the layout mode is vertical stacking.
						//
						topRow += sectionBounds.Rows;
					}

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

		/// <summary>
		/// Calculates the row heights for each section to be rendered within the specified bounds on a PDF grid page.
		/// </summary>
		/// <remarks>If the total calculated height of all sections exceeds the available rows and the layout mode is
		/// vertical stacking, the section heights are normalized to fit within the bounds.</remarks>
		/// <typeparam name="TModel">The type of the model associated with the PDF sections. Must implement <see cref="IPdfModel"/>.</typeparam>
		/// <param name="g">The PDF grid page on which the sections will be rendered.</param>
		/// <param name="m">The model instance providing data for the sections.</param>
		/// <param name="parentSection">The parent section containing the sections to be measured.</param>
		/// <param name="sections">An array of sections for which to calculate heights.</param>
		/// <param name="bounds">The bounds specifying the available rows for rendering sections.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an array of <see
		/// cref="PdfSectionDimension"/> objects, each representing the calculated height and whether it is fixed for a
		/// section.</returns>
		protected virtual async Task<PdfSectionDimension[]> CalculateSectionHeights<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			List<PdfSectionDimension> returnValue = [];

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
					returnValue.Add(new PdfSectionDimension(true, rows));
				}
				else
				{
					double height = section.ResolveStyle(0).RelativeHeight.Resolve(g, m);
					int rows = (int)(bounds.Rows * (height == 0 ? 1 : height));
					returnValue.Add(new PdfSectionDimension(false, rows));
				}
			}

			if (parentSection.SectionLayoutMode == PdfSectionsLayoutMode.VerticalStacking &&
				returnValue.Sum(t => t.Value) > bounds.Rows)
			{
				//
				// If the total calculated height exceeds the available rows in the bounds,
				// normalize the section heights to fit within the bounds.
				//
				await this.NormalizeSectionDimensions(returnValue, bounds.Rows);
			}

			return [.. returnValue];
		}

		/// <summary>
		/// Calculates the column widths for each section within a PDF grid layout based on fixed or relative sizing rules.
		/// </summary>
		/// <remarks>If the total calculated widths exceed the available columns in horizontal stacking mode, the
		/// widths are normalized to fit within the bounds.</remarks>
		/// <typeparam name="TModel">The type of the model used for resolving section styles. Must implement IPdfModel.</typeparam>
		/// <param name="g">The PDF grid page context used for resolving section dimensions.</param>
		/// <param name="m">The model instance providing data for style resolution.</param>
		/// <param name="parentSection">The parent section containing the sections for which widths are being calculated.</param>
		/// <param name="sections">An array of sections whose widths are to be determined.</param>
		/// <param name="bounds">The bounds specifying the available columns for layout.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an array of PdfSectionDimension
		/// objects, each representing the calculated width and whether it is fixed for a section.</returns>
		protected virtual async Task<PdfSectionDimension[]> CalculateSectionWidths<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds)
			where TModel : IPdfModel
		{
			List<PdfSectionDimension> returnValue = [];

			foreach (IPdfSection<TModel> section in sections)
			{
				//
				// Check if there is a fixed width specified.
				//
				int[] fixedWidth = section.ResolveStyle(0).FixedWidths.Resolve(g, m);

				if (fixedWidth.Length > 0)
				{
					int columns = fixedWidth[0];
					returnValue.Add(new PdfSectionDimension(true, columns));
				}
				else
				{
					double width = section.ResolveStyle(0).RelativeWidths.Resolve(g, m)[0];
					int columns = (int)(bounds.Columns * (width == 0 ? 1 : width));
					returnValue.Add(new PdfSectionDimension(false, columns));
				}
			}

			if (parentSection.SectionLayoutMode == PdfSectionsLayoutMode.HorizontalStacking &&
				returnValue.Sum(t => t.Value) > bounds.Columns)
			{
				//
				// If the total calculated width exceeds the available columns in the bounds,
				// normalize the section widths to fit within the bounds.
				//
				await this.NormalizeSectionDimensions(returnValue, bounds.Columns);
			}

			return [.. returnValue];
		}

		/// <summary>
		/// Normalizes the values of flexible section dimensions so that their total, combined with fixed sections, matches
		/// the specified total size.
		/// </summary>
		/// <remarks>If there are no flexible sections, the method does not modify any values. This method does not
		/// perform validation on the input list or values.</remarks>
		/// <param name="sectionDimensions">A list of section dimensions to normalize. Sections marked as fixed retain their values; non-fixed sections are
		/// adjusted to evenly distribute the remaining size.</param>
		/// <param name="totalSize">The total size that the sum of all section values should equal after normalization.</param>
		/// <returns>A completed task that represents the asynchronous normalization operation.</returns>
		protected virtual Task NormalizeSectionDimensions(List<PdfSectionDimension> sectionDimensions, int totalSize)
		{
			int fixedSize = sectionDimensions.Where(s => s.Fixed).Sum(s => s.Value);
			int remainingSize = totalSize - fixedSize;
			int flexibleSections = sectionDimensions.Count(s => !s.Fixed);

			if (flexibleSections > 0)
			{
				int rowsPerFlexibleSection = remainingSize / flexibleSections;

				foreach (PdfSectionDimension section in sectionDimensions.Where(s => !s.Fixed))
				{
					section.Value = rowsPerFlexibleSection;
				}
			}

			return Task.CompletedTask;
		}
	}
}