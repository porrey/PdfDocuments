using PdfSharp.Drawing;

namespace PdfDocuments
{
	public interface IHeaderBackgroundColor<TModel>
	{
		BindProperty<XColor, TModel> HeaderBackgroundColor { get; set; }
	}
}
