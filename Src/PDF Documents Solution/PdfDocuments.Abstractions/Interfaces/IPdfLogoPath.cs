namespace PdfDocuments
{
	public interface IPdfLogoPath<TModel>
	{
		BindProperty<string, TModel> LogoPath { get; set; }
	}
}
