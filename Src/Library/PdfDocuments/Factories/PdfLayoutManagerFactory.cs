namespace PdfDocuments.Layout_Manager
{
	/// <summary>
	/// Provides a factory for creating layout manager instances based on the specified PDF sections layout mode.
	/// </summary>
	/// <remarks>Use this class to obtain an appropriate layout manager for arranging PDF sections according to the
	/// desired layout strategy. The factory abstracts the creation logic, allowing clients to request layout managers
	/// without needing to know the specific implementation details.</remarks>
	public class PdfLayoutManagerFactory : IPdfLayoutManagerFactory
	{
		/// <summary>
		/// Asynchronously retrieves an instance of an <see cref="IPdfLayoutManager"/> configured for the specified PDF sections
		/// layout mode.
		/// </summary>
		/// <param name="layoutMode">The layout mode that determines how PDF sections are arranged. Specifies whether sections are stacked vertically,
		/// horizontally, or overlaid.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IPdfLayoutManager"/>
		/// instance configured for the specified layout mode.</returns>
		public Task<IPdfLayoutManager> GetLayoutManagerAsync(PdfSectionsLayoutMode layoutMode)
		{
			IPdfLayoutManager returnValue = null;

			switch (layoutMode)
			{
				case PdfSectionsLayoutMode.VerticalStacking:
					returnValue = new StackingLayoutManager();
					break;
				case PdfSectionsLayoutMode.HorizontalStacking:
					returnValue = new StackingLayoutManager();
					break;
				case PdfSectionsLayoutMode.OverlayStacking:
					returnValue = new StackingLayoutManager();
					break;
			}

			return Task.FromResult(returnValue);
		}
	}
}
