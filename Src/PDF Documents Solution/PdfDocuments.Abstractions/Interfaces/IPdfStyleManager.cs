using System.Collections.Generic;

namespace PdfDocuments
{
	public interface IPdfStyleManager<TModel> : IDictionary<string, PdfStyle<TModel>>
	{
		PdfStyle<TModel> GetStyle(string name);
		void Replace(string name, PdfStyle<TModel> style);
	}
}
