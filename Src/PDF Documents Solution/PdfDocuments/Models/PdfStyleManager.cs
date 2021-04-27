using System.Collections.Generic;

namespace PdfDocuments
{
	public class PdfStyleManager<TModel> : Dictionary<string, PdfStyle<TModel>>, IPdfStyleManager<TModel>
	{
		public const string Default = "Default";

		public PdfStyleManager()
		{
			//
			// Add default style.
			//
			this.Add(PdfStyleManager<TModel>.Default, new PdfStyle<TModel>());
		}

		public PdfStyle<TModel> GetStyle(string name)
		{
			PdfStyle<TModel> returnValue = this[PdfStyleManager<TModel>.Default];

			if (this.ContainsKey(name))
			{
				returnValue = this[name];
			}

			return returnValue;
		}

		public void Replace(string name, PdfStyle<TModel> style)
		{
			if (this.ContainsKey(name))
			{
				this.Remove(name);
				this.Add(name, style);
			}
			else
			{
				this.Add(name, style);
			}
		}
	}
}
