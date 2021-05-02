using System.Text;
using System.Threading.Tasks;
using PdfDocuments;

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
									.SetFlag(DebugMode.RevealGrid, true)
									.SetFlag(DebugMode.RevealLayout, true)
									.SetFlag(DebugMode.HideDetails, false)
									.SetFlag(DebugMode.RevealFontDetails, true)
									.SetFlag(DebugMode.OutlineText, false);

			//
			// Create an instance of the model.
			//
			Message model = new Message() { Id = "12345", Text = "Hello World" };

			//
			// Create, save and open the PDF.
			//
			await helloWorld.SaveAndOpenPdfAsync(model);

			return 0;
		}
	}
}
