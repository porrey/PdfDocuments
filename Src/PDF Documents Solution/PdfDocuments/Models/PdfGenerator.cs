/*
	MIT License

	Copyright (c) 2021 Daniel Porrey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System;
using System.IO;
using System.Threading.Tasks;
using PdfDocuments.Barcode;
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfDocuments
{
	public abstract class PdfGenerator<TModel> : IPdfGenerator<TModel>
		where TModel : IPdfModel
	{
		public PdfGenerator(ITheme theme, IBarcodeGenerator barcodeGenerator)
		{
			this.Theme = theme;
			this.BarcodeGenerator = barcodeGenerator;
		}

		public virtual async Task<(bool, byte[])> CreatePdfAsync(TModel model)
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

		public virtual Type DocumentType => typeof(TModel);
		public virtual ITheme Theme { get; set; }
		public virtual string DocumentTitle { get; set; }
		public virtual DebugMode DebugMode { get; set; } = DebugMode.None;
		public virtual PdfSpacing Padding { get; } = new PdfSpacing { Top = 1, Bottom = 1, Left = 1, Right = 1 };
		public virtual IPdfSection<TModel> ReportSection { get; } = new PdfVerticalStackSection<TModel>();
		public virtual IBarcodeGenerator BarcodeGenerator { get; set; }

		protected virtual PdfGrid Grid { get; set; }
		protected virtual double PageWidth(PdfPage page) => page.Width - page.TrimMargins.Left - page.TrimMargins.Right;
		protected virtual double PageHeight(PdfPage page) => page.Height - page.TrimMargins.Top - page.TrimMargins.Bottom;

		protected virtual string OnGetDocumentTitle(TModel model)
		{
			return "No Document Title";
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

		protected virtual Task<bool> OnCreateSectionsAsync()
		{
			bool returnValue = false;

			//
			// The report contains one primary section that contains
			// sub-sections.
			this.ReportSection.Key = "Document";
			this.ReportSection.AddChildSection(this.OnCreatePageHeaderSection());
			this.ReportSection.AddChildSection(this.OnCreateReportHeaderSection());
			this.ReportSection.AddChildSection(this.OnCreateContentSection());
			this.ReportSection.AddChildSection(this.OnCreateReportFooterSection());
			this.ReportSection.AddChildSection(this.OnCreatePageFooterSection());

			return Task.FromResult(returnValue);
		}

		protected virtual IPdfSection<TModel> OnCreatePageHeaderSection()
		{
			return new PageHeaderSection<TModel>();
		}

		protected virtual IPdfSection<TModel> OnCreateReportHeaderSection()
		{
			return new ReportHeaderSection<TModel>();
		}

		protected virtual IPdfSection<TModel> OnCreateContentSection()
		{
			return new ContentSection<TModel>();
		}

		protected virtual IPdfSection<TModel> OnCreateReportFooterSection()
		{
			return new ReportFooterSection<TModel>();
		}

		protected virtual IPdfSection<TModel> OnCreatePageFooterSection()
		{
			return new PageFooterSection<TModel>();
		}

		protected virtual async Task<bool> OnCreatePdfAsync(PdfDocument document, TModel model)
		{
			bool returnValue = false;

			//
			//
			//
			await this.OnCreatePagesAsync(document, model);
			int pageNumber = 1;

			//
			//
			//
			await this.OnCreateSectionsAsync();

			foreach (PdfPage page in document.Pages)
			{
				//
				//
				//
				await this.OnSetPageLayoutAsync(page);

				//
				//
				//
				this.Grid = await this.OnSetPageGridAsync(page);

				//
				//
				//
				returnValue = await this.OnRenderDocument(document, page, pageNumber, model);

				//
				//
				//
				pageNumber++;
			}

			return returnValue;
		}

		protected virtual async Task<bool> OnRenderDocument(PdfDocument document, PdfPage page, int pageNumber, TModel model)
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
						this.DocumentTitle = this.OnGetDocumentTitle(model);

						//
						// Create the page grid instance.
						//
						PdfGridPage gridPage = new PdfGridPage()
						{
							Theme = this.Theme,
							DocumentTitle = this.DocumentTitle,
							Document = document,
							Page = page,
							Graphics = g,
							Grid = this.Grid,
							PageNumber = pageNumber,
							DebugMode = this.DebugMode,
							BarcodeGenerator = this.BarcodeGenerator
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
							await this.ReportSection.RenderDebugAsync(gridPage, model);
						}

						if (this.DebugMode.HasFlag(DebugMode.RevealGrid))
						{
							//
							// Draw the grid for debugging.
							//
							gridPage.DrawGrid(XColor.FromArgb(60, XColors.Red), this.Theme.Drawing.LineWeightNormal / 2.0);
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

		protected virtual async Task<bool> OnLayoutSectionsAsync(PdfGridPage gridPage, TModel model)
		{
			bool returnValue = false;

			//
			// Set the report section bounds to the full page.
			//
			this.ReportSection.ActualBounds = gridPage.Grid.GetBounds();

			//
			// Call layout on the primary section if it is being rendered.
			//
			if (this.ReportSection.ShouldRender.Invoke(gridPage, model))
			{
				if (await this.ReportSection.LayoutAsync(gridPage, model))
				{
					returnValue = true;
				}
			}

			return returnValue;
		}

		protected virtual async Task<bool> OnRenderSectionsAsync(PdfGridPage gridPage, TModel model)
		{
			bool returnValue = false;

			//
			// Set the report section bounds to the full page.
			//
			this.ReportSection.ActualBounds = gridPage.Grid.GetBounds();

			//
			// Call layout on the primary section if it is being rendered.
			//
			if (this.ReportSection.ShouldRender.Invoke(gridPage, model))
			{
				if (await this.ReportSection.RenderAsync(gridPage, model))
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
