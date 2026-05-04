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
using PdfSharp.Drawing;

namespace PdfDocuments.Example.Simple
{
	/// <summary>
	/// Generates a PDF document containing a styled 'Hello, World' message using a specified style manager.
	/// </summary>
	/// <remarks>This class customizes PDF generation by defining a specific text style and content section for the
	/// message. It is intended for scenarios where a simple, styled message needs to be rendered in a PDF format. The
	/// style manager provided to the constructor is used to manage and apply styles to the generated content.</remarks>
	public class HelloWorldPdf : PdfGenerator<Message>
	{
		/// <summary>
		/// Initializes a new instance of the HelloWorld class with the specified PDF style manager.
		/// </summary>
		/// <param name="styleManager">The style manager used to apply formatting to Message objects in generated PDF documents. Cannot be null.</param>
		public HelloWorldPdf(IPdfStyleManager<Message> styleManager)
			: base(styleManager)
		{
		}

		/// <summary>
		/// Initializes custom styles for PDF content generation.
		/// </summary>
		/// <remarks>Adds a style named "HelloWorld.Text" to the style manager, which can be referenced when creating
		/// PDF content. The style includes font, color, border, alignment, and padding settings.</remarks>
		/// <param name="styleManager">The style manager used to register and manage styles for PDF elements.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		protected override Task OnInitializeStylesAsync(IPdfStyleManager<Message> styleManager)
		{
			//
			// Add a style for the text. The style name is referenced in the
			// content creation.
			//
			this.StyleManager.Add("HelloWorld.Text", Style.Create<Message>()
						.UseFont("Arial", 48)
						.UseForegroundColor(XColors.Purple)
						.UseTextAlignment(XStringFormats.Center)
						.UsePadding(10, 10, 10, 10)
						.Build());

			return Task.CompletedTask;
		}

		/// <summary>
		/// Creates and returns a PDF section containing a text block for the specified message, applying the defined styles.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains a PDF section with the message text,
		/// styled using the "HelloWorld.Text" style and the current style manager.</returns>
		protected override Task<IPdfSection<Message>> OnAddContentAsync()
		{
			//
			// Add a basic text block using the style that was created.
			//
			return Task.FromResult(
				Pdf.TextBlockSection<Message>()
					.WithText((g, m) => m.Text)
					.WithStyles("HelloWorld.Text")
					.WithKey("HelloWorld.TextBlock")
					.WithStyleManager(this.StyleManager)
			);
		}
	}
}