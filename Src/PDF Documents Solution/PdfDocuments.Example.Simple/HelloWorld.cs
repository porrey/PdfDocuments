using PdfSharp.Drawing;
using System.Threading.Tasks;

namespace PdfDocuments.Example.Simple
{
	public class HelloWorld : PdfGenerator<Message>
	{
		public HelloWorld(IPdfStyleManager<Message> styleManager)
			: base(styleManager)
		{
		}

		protected override Task OnInitializeStylesAsync(IPdfStyleManager<Message> styleManager)
		{
			//
			// Add a style for the text. The style name is referenced in the
			// content creation.
			//
			this.StyleManager.Add("HelloWorld.Text", Style.Create<Message>()
						.UseFont("Arial", 48)
						.UseForegroundColor(XColors.Purple)
						.UseBorderWidth(1)
						.UseTextAlignment(XStringFormats.Center)
						.UsePadding(10, 10, 10, 10)
						.Build());

			return Task.CompletedTask;
		}

		protected override Task<IPdfSection<Message>> OnAddContentAsync()
		{
			//
			// Add a basic text block using the style that was created.
			//
			return Task.FromResult(Pdf.TextBlockSection<Message>()
									  .WithText((g, m) => m.Text)
									  .WithStyles("HelloWorld.Text"));
		}
	}
}
