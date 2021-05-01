using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PdfDocuments.Example
{
	public static class Extensions
	{
		public static async Task<bool> SaveAndOpenPdfAsync<TModel>(this IPdfGenerator<TModel> generator, TModel model)
			where TModel : IPdfModel
		{
			bool returnValue = false;

			//
			// Generate the PDF.
			//
			(bool result, byte[] fileData) = await generator.Build(model);

			if (result)
			{
				//
				// Save the PDF to the desktop.
				//
				string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\{model.GetType().Name} [{model.Id}].pdf";
				File.WriteAllBytes(fileName, fileData);

				//
				// Launch the system PDF viewer.
				//
				Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
				returnValue = true;
			}

			return returnValue;
		}
	}
}
