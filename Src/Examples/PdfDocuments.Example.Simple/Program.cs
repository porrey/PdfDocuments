
/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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
using System.Threading.Tasks;

namespace PdfDocuments.Example.Simple
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			//
			// Register an encoding provider.
			//
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			//
			// Create an instance of the style manager.
			//
			PdfStyleManager<Message> styleManager = new();

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
									.SetFlag(DebugMode.OutlineText, false);

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
