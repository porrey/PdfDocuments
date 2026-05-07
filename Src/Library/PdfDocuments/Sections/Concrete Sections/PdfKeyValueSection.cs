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
namespace PdfDocuments
{
	/// <summary>
	/// Represents a PDF section that displays a collection of key-value pairs using a specified model type.
	/// </summary>
	/// <remarks>Use this class to render structured key-value data within a PDF document. Each item in the section
	/// is displayed with its key and corresponding value, formatted according to the resolved styles. The section is
	/// suitable for scenarios where labeled values need to be presented, such as metadata, summaries, or property
	/// lists.</remarks>
	/// <typeparam name="TModel">The type of model used to provide data for the key-value items. Must implement the IPdfModel interface.</typeparam>
	public class PdfKeyValueSection<TModel> : PdfSectionTemplate<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the object has been initialized.
		/// </summary>
		protected virtual bool IsInitialized { get; set; }

		/// <summary>
		/// Initializes a new instance of the PdfKeyValueSection class with the specified key-value items.
		/// </summary>
		/// <param name="values">An array of PdfKeyValueItem<![CDATA[<TModel>]]>; objects to include in the section. Can be empty to create an empty section.</param>
		public PdfKeyValueSection(params PdfKeyValueItem<TModel>[] values)
		{
			foreach (PdfKeyValueItem<TModel> value in values)
			{
				this.Items.Add(value);
			}
		}

		/// <summary>
		/// Gets the collection of key-value items associated with the current model.
		/// </summary>
		/// <remarks>The collection is read-only and contains all key-value pairs relevant to the instance.
		/// Modifications to the collection itself are not supported; however, individual items within the collection may be
		/// modified if their types allow it.</remarks>
		public virtual IList<PdfKeyValueItem<TModel>> Items { get; } = [];

		/// <summary>
		/// Performs asynchronous initialization logic for the grid page using the specified model and bounds.
		/// </summary>
		/// <param name="g">The grid page to initialize.</param>
		/// <param name="m">The model containing data or state used for initialization.</param>
		/// <param name="bounds">The bounds within which the grid page should be initialized.</param>
		/// <returns>A task that represents the asynchronous initialization operation.</returns>
		protected override Task OnInitializeAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (!this.IsInitialized)
			{
				List<IPdfSection<TModel>> innerItems = [];

				int i = 0;
				string controlStyleName = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string keyStyleName = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();
				string valueStyleName = i < this.StyleNames.Count() ? this.StyleNames.ElementAt(i++) : this.StyleNames.Last();

				this.StyleNames = [controlStyleName];

				int j = 0;

				foreach (PdfKeyValueItem<TModel> item in this.Items)
				{
					IPdfSection<TModel> section =
						Pdf.HorizontalStackSection<TModel>(
							Pdf.TextBlockSection<TModel>()
								.WithText(item.Key.Resolve(g, m))
								.WithStyles(keyStyleName)
								.WithZOrder(j++),
							Pdf.TextBlockSection<TModel>()
								.WithText(item.Value.Resolve(g, m))
								.WithStyles(valueStyleName)
								.WithZOrder(j++)
						).WithParentSection(this);

					innerItems.Add(section.WithParentSection(this));
				}

				this.Children = innerItems;
				this.Text = string.Empty;

				this.IsInitialized = true;
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Gets or sets the layout mode used for arranging sections within the PDF document.
		/// </summary>
		/// <remarks>The layout mode determines how sections are visually organized when rendering the document.
		/// Setting this property may have no effect if the implementation does not support changing the layout
		/// mode.</remarks>
		public override PdfSectionsLayoutMode SectionLayoutMode
		{
			get
			{
				return PdfSectionsLayoutMode.VerticalStacking;
			}
			set
			{
			}
		}
	}
}
