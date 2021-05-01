﻿using System.Text;
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
			PdfStyleManager<Message> styleManager = new PdfStyleManager<Message>();

			//
			// Create an instance of the PDF generator.
			//
			HelloWorld helloWorld = new HelloWorld(styleManager);

			//
			// Set debug flags.
			//
			helloWorld.DebugMode = helloWorld.DebugMode
									.SetFlag(DebugMode.RevealGrid, false)
									.SetFlag(DebugMode.RevealLayout, false)
									.SetFlag(DebugMode.HideDetails, false)
									.SetFlag(DebugMode.RevealFontDetails, false)
									.SetFlag(DebugMode.OutlineText, false);

			//
			// Create, save and open the PDF.
			//
			await helloWorld.SaveAndOpenPdfAsync(new Message() { Id = "12345", Text = "Hello World" });

			return 0;
		}
	}
}