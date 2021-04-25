using PdfSharp.Drawing;

namespace PdfDocuments
{
	public interface IPdfValueFont<TModel>
	{
		BindProperty<XFont, TModel> ValueFont { get; set; }
	}
}
