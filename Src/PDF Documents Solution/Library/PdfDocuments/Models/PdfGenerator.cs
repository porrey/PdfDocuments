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
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PdfDocuments
{
	public abstract class PdfGenerator<TModel> : IPdfGenerator<TModel>
		where TModel : IPdfModel
	{
		public PdfGenerator(IPdfStyleManager<TModel> styleManager)
		{
			this.StyleManager = styleManager;
		}

		public virtual async Task<(bool, byte[])> Build(TModel model)
		{
			(bool result, byte[] pdf) = (false, null);

			//
			//
			//
			using (PdfDocument document = new PdfDocument())
			{
				//
				//
				//
				result = await this.OnCreatePdfAsync(document, model);

				//
				//
				//
				if (result)
				{
					pdf = await this.GetPdfByteArrayAsync(document);
				}
			}

			return (result, pdf);
		}

		public IPdfStyleManager<TModel> StyleManager { get; set; }
		public virtual Type DocumentModelType => typeof(TModel);
		public virtual string DocumentTitle { get; set; }
		public virtual DebugMode DebugMode { get; set; } = DebugMode.None;
		public virtual IPdfSection<TModel> RootSection { get; protected set; }

		protected virtual PdfGrid Grid { get; set; }
		protected virtual double PageWidth(PdfPage page) => page.Width - page.TrimMargins.Left - page.TrimMargins.Right;
		protected virtual double PageHeight(PdfPage page) => page.Height - page.TrimMargins.Top - page.TrimMargins.Bottom;

		protected virtual async Task<bool> OnCreatePdfAsync(PdfDocument document, TModel model)
		{
			bool returnValue = false;

			//
			// Initialize styles.
			//
			await this.OnInitializeStylesAsync(this.StyleManager);

			//
			// Create the PDF pages.
			//
			await this.OnCreatePagesAsync(document, model);

			//
			// Create the document sections.
			//
			this.RootSection = await this.OnAddContentAsync();
			this.RootSection.StyleManager = this.StyleManager;

			//
			// Render the pages.
			//
			int pageNumber = 1;

			foreach (PdfPage page in document.Pages)
			{
				//
				// Set page layout.
				//
				await this.OnSetPageLayoutAsync(page);

				//
				// Set grid layout.
				//
				this.Grid = await this.OnSetPageGridAsync(page);

				//
				// Render the document page.
				//
				returnValue = await this.OnRenderDocumentAsync(document, page, pageNumber, model);

				//
				// Increment the page number.
				//
				pageNumber++;
			}

			return returnValue;
		}

		protected virtual Task OnInitializeStylesAsync(IPdfStyleManager<TModel> styleManager)
		{
			return Task.CompletedTask;
		}

		protected virtual Task<string> OnGetDocumentTitleAsync(TModel model)
		{
			return Task.FromResult("No Document Title");
		}

		protected virtual Task<PdfGrid> OnSetPageGridAsync(PdfPage page)
		{
			return Task.FromResult(new PdfGrid(this.PageWidth(page), this.PageHeight(page), 200, 80));
		}

		protected virtual Task<int> OnGetPageCountAsync(TModel model)
		{
			return Task.FromResult(1);
		}

		protected virtual async Task OnCreatePagesAsync(PdfDocument document, TModel model)
		{
			int pageCount = await this.OnGetPageCountAsync(model);

			for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
			{
				document.AddPage();
			}
		}

		protected virtual Task OnSetPageLayoutAsync(PdfPage page)
		{
			page.Orientation = PdfSharp.PageOrientation.Portrait;
			page.Size = PdfSharp.PageSize.Letter;
			page.TrimMargins.Left = XUnit.FromInch(.35);
			page.TrimMargins.Right = XUnit.FromInch(.35);
			page.TrimMargins.Top = XUnit.FromInch(.32);
			page.TrimMargins.Bottom = XUnit.FromInch(.32);

			return Task.FromResult(0);
		}

		protected virtual Task<IPdfSection<TModel>> OnAddContentAsync()
		{
			return Task.FromResult<IPdfSection<TModel>>(new PdfContentSection<TModel>());
		}

		protected virtual async Task<bool> OnRenderDocumentAsync(PdfDocument document, PdfPage page, int pageNumber, TModel model)
		{
			bool returnValue = false;

			using (XGraphics gfx = XGraphics.FromPdfPage(page))
			{
				using (XForm form = new XForm(document, this.PageWidth(page), this.PageHeight(page)))
				{
					using (XGraphics g = XGraphics.FromForm(form))
					{
						//
						// Get the document title.
						//
						this.DocumentTitle = await this.OnGetDocumentTitleAsync(model);

						//
						// Create the page grid instance.
						//
						PdfGridPage gridPage = new PdfGridPage()
						{
							DocumentTitle = this.DocumentTitle,
							Document = document,
							Page = page,
							Graphics = g,
							Grid = this.Grid,
							PageNumber = pageNumber,
							DebugMode = this.DebugMode
						};

						//
						// Layout and render the sections.
						//
						if (await this.OnLayoutSectionsAsync(gridPage, model))
						{
							if (!this.DebugMode.HasFlag(DebugMode.HideDetails))
							{
								returnValue = await this.OnRenderSectionsAsync(gridPage, model);
							}
							else
							{
								returnValue = true;
							}
						}

						#region Debug Elements
						//
						// Draw extra items to help debug layout issues.
						//
						if (this.DebugMode.HasFlag(DebugMode.RevealLayout))
						{
							//
							// Have the primary section render the debug elements.
							//
							await this.RootSection.RenderDebugAsync(gridPage, model);
						}

						if (this.DebugMode.HasFlag(DebugMode.RevealGrid))
						{
							//
							// Draw the grid for debugging.
							//
							gridPage.DrawGrid(XColor.FromArgb(60, XColors.Red), .3);
						}
						#endregion

						//
						// Draw the page.
						//
						gfx.DrawImage(form, page.TrimMargins.Left, page.TrimMargins.Top);
					}
				}
			}

			return returnValue;
		}

		protected virtual async Task<bool> OnLayoutSectionsAsync(PdfGridPage g, TModel m)
		{
			bool returnValue = false;

			//
			// Set the report section bounds to the full page.
			//
			this.RootSection.ActualBounds = g.Grid.GetBounds();

			//
			// Call layout on the primary section if it is being rendered.
			//
			if (this.RootSection.ShouldRender.Resolve(g, m))
			{
				if (await this.RootSection.LayoutAsync(g, m))
				{
					returnValue = true;
				}
			}

			return returnValue;
		}

		protected virtual async Task<bool> OnRenderSectionsAsync(PdfGridPage g, TModel m)
		{
			bool returnValue = false;

			//
			// Set the report section bounds to the full page.
			//
			this.RootSection.ActualBounds = g.Grid.GetBounds();

			//
			// Call layout on the primary section if it is being rendered.
			//
			if (this.RootSection.ShouldRender.Resolve(g, m))
			{
				if (await this.RootSection.RenderAsync(g, m))
				{
					returnValue = true;
				}
			}

			return returnValue;
		}

		protected virtual Task<byte[]> GetPdfByteArrayAsync(PdfDocument document)
		{
			byte[] returnValue = null;

			using (MemoryStream stream = new MemoryStream())
			{
				document.Save(stream, false);
				returnValue = stream.ToArray();
			}

			return Task.FromResult(returnValue);
		}
	}
}
