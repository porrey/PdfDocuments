namespace PdfDocuments
{
	/// <summary>
	/// Defines a contract for laying out PDF sections on a page asynchronously.
	/// </summary>
	/// <remarks>Implementations of this interface arrange one or more PDF sections within the specified bounds of a
	/// page, returning the resulting layout positions. This interface is typically used to support custom or dynamic PDF
	/// document generation scenarios.</remarks>
	public interface IPdfLayoutManager
	{
		/// <summary>
		/// Asynchronously arranges the specified sections within the given bounds on the PDF grid page using the provided
		/// model.
		/// </summary>
		/// <typeparam name="TModel">The type of the model used for layout. Must implement IPdfModel.</typeparam>
		/// <param name="g">The PDF grid page on which the sections will be laid out.</param>
		/// <param name="m">The model instance providing data for the layout operation. Cannot be null.</param>
		/// <param name="parentSection">The parent section that contains the sections to be arranged. Cannot be null.</param>
		/// <param name="sections">An array of sections to arrange within the parent section. Cannot be null or contain null sections.</param>
		/// <param name="bounds">The bounds within which the sections should be arranged on the page.</param>
		/// <returns>A task that represents the asynchronous layout operation.</returns>
		Task LayoutAsync<TModel>(PdfGridPage g, TModel m, IPdfSection<TModel> parentSection, IPdfSection<TModel>[] sections, PdfBounds bounds) 
			where TModel : IPdfModel;
	}
}
