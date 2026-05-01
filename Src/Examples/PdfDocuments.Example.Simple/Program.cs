
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
using System.Text;
using PdfSharp.Fonts;

namespace PdfDocuments.Example.Simple
{
	/// <summary>
	/// The main program.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The entry point for the application. Configures font settings, initializes required services, and generates a PDF
		/// document based on the provided model.
		/// </summary>
		/// <param name="args">An array of command-line arguments supplied to the application. Not used by this implementation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an exit code of 0 when the operation
		/// completes successfully.</returns>
		static async Task<int> Main(string[] _)
		{
			//
			// User windows fonts.
			//
			GlobalFontSettings.FontResolver = new FontResolver.Windows.FontResolver();

			//
			// Set the default font.
			//
			GlobalPdfDocumentsSettings.DefaultFontName = "Arial";

			//
			// Register an encoding provider.
			//
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			//
			// Create an instance of the style manager.
			//
			PdfStyleManager<Message> styleManager = [];

			//
			// Create an instance of the PDF generator.
			//
			HelloWorld helloWorld = new(styleManager);

			//
			// Set debug flags.
			//
			helloWorld.DebugMode = helloWorld.DebugMode
									.SetFlag(DebugMode.RevealGrid, true)
									.SetFlag(DebugMode.RevealLayout, true)
									.SetFlag(DebugMode.HideDetails, false)
									.SetFlag(DebugMode.RevealFontDetails, true)
									.SetFlag(DebugMode.OutlineText, true);

			//
			// Create an instance of the model.
			//
			Message model = new() { Id = "12345", Text = "Hello World" };

			//
			// Create, save and open the PDF.
			//
			await helloWorld.SaveAndOpenPdfAsync(model);

			return 0;
		}
	}
}
