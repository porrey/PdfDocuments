using PdfSharp.Drawing;

namespace PdfDocuments
{
	public interface IHeaderForegroundColor<TModel>
	{
		BindProperty<XColor, TModel> HeaderForegroundColor { get; set; }
	}
}
