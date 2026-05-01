namespace PdfDocuments
{
	/// <summary>
	/// Defines a contract for creating layout manager instances.
	/// </summary>
	/// <remarks>Implement this interface to provide custom logic for instantiating layout managers in a user
	/// interface framework. The specific responsibilities and usage scenarios depend on the concrete
	/// implementations.</remarks>
	public interface IPdfLayoutManagerFactory
	{
		/// <summary>
		/// Asynchronously retrieves an instance of an <see cref="IPdfLayoutManager"/> configured for the specified PDF sections
		/// layout mode.
		/// </summary>
		/// <param name="layoutMode">The layout mode to use when configuring the layout manager. Determines how PDF sections are arranged.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IPdfLayoutManager"/>
		/// instance configured for the specified layout mode.</returns>
		Task<IPdfLayoutManager> GetLayoutManagerAsync(PdfSectionsLayoutMode layoutMode);
	}
}
