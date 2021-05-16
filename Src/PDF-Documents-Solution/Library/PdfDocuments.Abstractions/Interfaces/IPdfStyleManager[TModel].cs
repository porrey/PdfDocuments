using System.Collections.Generic;

namespace PdfDocuments
{
	public interface IPdfStyleManager<TModel> : IDictionary<string, PdfStyle<TModel>>
			where TModel : IPdfModel
	{
		PdfStyle<TModel> GetStyle(string name);
		void Replace(string name, PdfStyle<TModel> style);
	}
}
