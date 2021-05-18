/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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
