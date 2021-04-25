namespace PdfDocuments
{
	public interface IPdfTitle<TModel>
	{
		BindProperty<string, TModel> Title { get; set; }
	}
}
