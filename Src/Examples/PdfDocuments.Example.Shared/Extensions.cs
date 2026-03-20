/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */
using System.Diagnostics;

namespace PdfDocuments.Example
{
	/// <summary>
	/// Provides extension methods for PDF generation and handling operations.
	/// </summary>
	/// <remarks>This class contains static extension methods that enhance the functionality of types implementing
	/// the IPdfGenerator<TModel> interface. Methods in this class are intended to simplify common PDF-related workflows,
	/// such as saving and opening generated documents. All methods are thread-safe and designed for asynchronous
	/// usage.</remarks>
	public static class Extensions
	{
		/// <summary>
		/// Generates a PDF from the specified model, saves it to the user's desktop, and opens it using the default PDF
		/// viewer.
		/// </summary>
		/// <remarks>The PDF file is saved to the user's desktop with a filename based on the model type and
		/// identifier. The method launches the system's default PDF viewer after saving the file. If PDF generation fails, no
		/// file is saved or opened.</remarks>
		/// <typeparam name="TModel">The type of the model used to generate the PDF. Must implement the IPdfModel interface.</typeparam>
		/// <param name="generator">The PDF generator instance used to build the PDF document.</param>
		/// <param name="model">The model containing the data to be rendered in the PDF. Must not be null.</param>
		/// <returns>true if the PDF was successfully generated, saved, and opened; otherwise, false.</returns>
		public static async Task<bool> SaveAndOpenPdfAsync<TModel>(this IPdfGenerator<TModel> generator, TModel model)
			where TModel : IPdfModel
		{
			bool returnValue = false;

			//
			// Generate the PDF.
			//
			(bool result, byte[] fileData) = await generator.BuildAsync(model);

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
