namespace PdfDocuments
{
	public interface IPdfElement<TModel> where TModel : IPdfModel
	{
		PdfSize Measure(PdfGridPage g, TModel m, PdfStyle<TModel> style);
		void Render(PdfGridPage g, TModel m, PdfBounds bounds, PdfStyle<TModel> style);
	}
}