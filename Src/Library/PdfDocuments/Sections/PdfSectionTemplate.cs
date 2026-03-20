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

namespace PdfDocuments
{
	/// <summary>
	/// Represents an abstract template for a PDF section that supports hierarchical composition, styling, and data binding
	/// for rendering PDF documents.
	/// </summary>
	/// <remarks>PdfSectionTemplate provides a base for creating structured PDF sections with customizable layout,
	/// rendering, and style management. Sections can contain child sections, support conditional rendering, and bind
	/// properties to model data. Use this class as a foundation for building reusable and extensible PDF document
	/// components.</remarks>
	/// <typeparam name="TModel">The model type used for data binding within the section. Must implement the IPdfModel interface.</typeparam>
	public abstract class PdfSectionTemplate<TModel> : IPdfSection<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfSectionTemplate class.
		/// </summary>
		public PdfSectionTemplate()
		{
			this.SetKey();
		}

		/// <summary>
		/// Initializes a new instance of the PdfSectionTemplate class with the specified child sections.
		/// </summary>
		/// <remarks>Each child section provided is added to the template in the order specified. This constructor is
		/// useful for composing a section template from multiple subsections at initialization.</remarks>
		/// <param name="children">An array of IPdfSection<![CDATA[<TModel>]]> instances representing the child sections to add to this template. Cannot be null.</param>
		public PdfSectionTemplate(params IPdfSection<TModel>[] children)
			: this()
		{
			foreach (IPdfSection<TModel> child in children)
			{
				this.AddChildSection(child);
			}
		}

		/// <summary>
		/// Gets or sets the unique identifier associated with this instance.
		/// </summary>
		public virtual string Key { get; set; }

		/// <summary>
		/// Gets or sets the text value associated with the model.
		/// </summary>
		public virtual BindProperty<string, TModel> Text { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the parent section of the current PDF section.
		/// </summary>
		/// <remarks>Use this property to access or assign the hierarchical parent of the section. This can be useful
		/// for navigating or modifying the structure of a PDF document composed of nested sections.</remarks>
		public virtual IPdfSection<TModel> ParentSection { get; set; }

		/// <summary>
		/// Gets the collection of child sections contained within this section.
		/// </summary>
		/// <remarks>The returned collection provides access to all immediate child sections. Modifications to the
		/// collection may affect the structure of the document. The collection is read-only; to add or remove child sections,
		/// use the appropriate methods provided by the containing class, if available.</remarks>
		public virtual IList<IPdfSection<TModel>> Children { get; } = [];

		/// <summary>
		/// Gets or sets the actual bounding rectangle of the element in PDF coordinates.
		/// </summary>
		public virtual PdfBounds ActualBounds { get; set; } = new PdfBounds(0, 0, 0, 0);

		/// <summary>
		/// Gets or sets a value indicating whether the component should be rendered.
		/// </summary>
		public virtual BindProperty<bool, TModel> ShouldRender { get; set; } = true;

		/// <summary>
		/// Gets or sets the path to the image used as a watermark.
		/// </summary>
		/// <remarks>The watermark image path should reference a valid image file accessible to the application. This
		/// property can be used to visually brand or mark content with a custom image.</remarks>
		public virtual BindProperty<string, TModel> WaterMarkImagePath { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the collection of style names applied to the model.
		/// </summary>
		/// <remarks>The collection determines which styles are used when rendering or processing the model. The
		/// default value includes the standard style provided by the manager. Modifying this collection allows customization
		/// of appearance or behavior based on style definitions.</remarks>
		public virtual IEnumerable<string> StyleNames { get; set; } = [PdfStyleManager<TModel>.Default];

		private IPdfStyleManager<TModel> _styleManager = null;

		/// <summary>
		/// Gets or sets the style manager used to control PDF rendering styles for the current section and its child
		/// elements.
		/// </summary>
		/// <remarks>If not explicitly set, the style manager is inherited from the parent section. Setting this
		/// property overrides the inherited style manager for the current section.</remarks>
		public virtual IPdfStyleManager<TModel> StyleManager
		{
			get
			{
				IPdfSection<TModel> parent = this.ParentSection;

				while (parent != null && _styleManager == null)
				{
					_styleManager = parent.StyleManager;
					parent = parent.ParentSection;
				}

				return _styleManager;
			}
			set
			{
				_styleManager = value;
			}
		}

		/// <summary>
		/// Gets the relative height value for the current style, expressed as a bindable property.
		/// </summary>
		/// <remarks>Use this property to specify or retrieve the height of the element relative to its parent or
		/// container. The value is typically a ratio or percentage, depending on the layout system. Changes to this property
		/// may affect the layout and rendering of the element.</remarks>
		public virtual BindProperty<double, TModel> RelativeHeight => this.ResolveStyle(0).RelativeHeight;

		/// <summary>
		/// Gets the relative widths for each column in the layout, expressed as proportions of the total available space.
		/// </summary>
		/// <remarks>Use this property to specify or retrieve the proportional widths of columns when rendering or
		/// arranging elements. The values should sum to 1.0 to represent the full width allocation. This property is useful
		/// for creating flexible, responsive layouts where column sizes are determined by relative proportions rather than
		/// fixed pixel values.</remarks>
		public virtual BindProperty<double[], TModel> RelativeWidths => this.ResolveStyle(0).RelativeWidths;

		/// <summary>
		/// Performs asynchronous layout of the section and its children on the specified PDF grid page using the provided
		/// model.
		/// </summary>
		/// <remarks>The layout operation applies section margins and delegates layout to child elements if the base
		/// section layout succeeds. Override this method to customize layout behavior for derived sections.</remarks>
		/// <param name="g">The PDF grid page on which the section layout is performed.</param>
		/// <param name="m">The model containing data used for layout and style resolution.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the layout was
		/// successful; otherwise, <see langword="false"/>.</returns>
		public virtual async Task<bool> LayoutAsync(PdfGridPage g, TModel m)
		{
			bool returnValue = true;

			//
			// The first style is always used for the base section style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Apply margins.
			//
			PdfBounds bounds = this.ApplyMargins(g, m, style.Margin.Resolve(g, m));

			if (await this.OnLayoutAsync(g, m, bounds))
			{
				returnValue = await this.OnLayoutChildrenAsync(g, m, bounds);
			}
			else
			{
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Performs asynchronous layout operations for the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>Override this method to implement custom layout logic for a PDF grid page. The default
		/// implementation always returns <see langword="true"/>.</remarks>
		/// <param name="g">The PDF grid page to be laid out.</param>
		/// <param name="m">The data model used to generate content for the layout operation.</param>
		/// <param name="bounds">The bounds within which the layout should be performed.</param>
		/// <returns>A task that represents the asynchronous layout operation. The task result is <see langword="true"/> if the layout
		/// was successful; otherwise, <see langword="false"/>.</returns>
		protected virtual Task<bool> OnLayoutAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// Arranges and lays out the child sections of the current grid page asynchronously.
		/// </summary>
		/// <remarks>Only child sections that are marked to be rendered are processed. If any child section fails to
		/// layout, the operation stops and returns <see langword="false"/>.</remarks>
		/// <param name="g">The grid page on which the child sections will be laid out.</param>
		/// <param name="m">The model containing data used for rendering the child sections.</param>
		/// <param name="bounds">The bounds within which the child sections should be arranged.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all child sections
		/// are successfully laid out; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task<bool> OnLayoutChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Render child sections.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(g, m)).ToArray();

			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.LayoutAsync(g, m))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Asynchronously renders the section and its child sections onto the specified PDF grid page using the provided
		/// model.
		/// </summary>
		/// <remarks>Rendering includes applying margins, drawing backgrounds and borders, rendering child sections,
		/// and optionally drawing a watermark image if specified. The method returns <see langword="false"/> if any child
		/// section fails to render or if rendering is not performed.</remarks>
		/// <param name="g">The PDF grid page on which the section will be rendered.</param>
		/// <param name="m">The model containing data used to resolve styles and content for rendering.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the section and
		/// all child sections were rendered successfully; otherwise, <see langword="false"/>.</returns>
		public virtual async Task<bool> RenderAsync(PdfGridPage g, TModel m)
		{
			bool returnValue = true;

			//
			// The first style is always used for the base section style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Apply margins.
			//
			PdfBounds bounds = this.ApplyMargins(g, m, style.Margin.Resolve(g, m));

			if (this.OnShouldDrawBackground())
			{
				//
				// Draw the background if set.
				//
				XColor backgroundColor = style.BackgroundColor.Resolve(g, m);
				
				if (backgroundColor != XColor.Empty)
				{
					//
					// Draw the filled rectangle.
					//
					g.DrawFilledRectangle(bounds, backgroundColor);
				}
			}

			if (await this.OnRenderAsync(g, m, bounds))
			{
				//
				// Render child sections.
				//
				IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(g, m)).ToArray();

				foreach (IPdfSection<TModel> section in sections)
				{
					if (!await section.RenderAsync(g, m))
					{
						returnValue = false;
						break;
					}
				}

				if (this.OnShouldDrawBorder())
				{
					//
					// Render the border.
					//
					double borderWidth = style.BorderWidth.Resolve(g, m);
					if (borderWidth > 0.0)
					{
						g.DrawRectangle(bounds, borderWidth, style.BorderColor.Resolve(g, m));
					}
				}
			}
			else
			{
				returnValue = false;
			}

			//
			// Draw the page water mark.
			//
			string waterMarkPath = this.WaterMarkImagePath.Resolve(g, m);
			if (!string.IsNullOrWhiteSpace(waterMarkPath) && File.Exists(waterMarkPath))
			{
				g.DrawImage(waterMarkPath, this.ActualBounds, PdfHorizontalAlignment.Center, PdfVerticalAlignment.Center);
			}

			return returnValue;
		}

		/// <summary>
		/// Determines whether the background should be drawn for the current element.
		/// </summary>
		/// <returns>true if the background should be drawn; otherwise, false.</returns>
		protected virtual bool OnShouldDrawBackground()
		{
			return true;
		}

		/// <summary>
		/// Determines whether a border should be drawn for the current element.
		/// </summary>
		/// <remarks>Override this method in a derived class to customize the border drawing behavior based on
		/// specific conditions.</remarks>
		/// <returns>Returns <see langword="true"/> if a border should be drawn; otherwise, <see langword="false"/>.</returns>
		protected virtual bool OnShouldDrawBorder()
		{
			return true;
		}

		/// <summary>
		/// Renders debug overlays and outlines for the current section and its child sections asynchronously.
		/// </summary>
		/// <remarks>This method renders debug overlays for the section and recursively for all child sections that
		/// should be rendered. It can be used to visually inspect layout boundaries and section outlines during PDF
		/// generation.</remarks>
		/// <param name="g">The PDF grid page on which the debug overlays are rendered.</param>
		/// <param name="m">The model instance providing data for rendering the debug overlays.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all debug overlays
		/// were rendered successfully; otherwise, <see langword="false"/>.</returns>
		public virtual async Task<bool> RenderDebugAsync(PdfGridPage g, TModel m)
		{
			bool returnValue = true;

			//
			// The first style is always used for the base section style.
			//
			PdfStyle<TModel> style = this.ResolveStyle(0);

			//
			// Apply margins.
			//
			PdfBounds bounds = this.ApplyMargins(g, m, style.Margin.Resolve(g, m));

			//
			// Call the base to render the outline overlay.
			//
			await this.OnRenderDebugAsync(g, m, bounds);

			//
			// Get a list of section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(g, m)).ToArray();

			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.RenderDebugAsync(g, m))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Sets the actual number of rows for the current bounds asynchronously.
		/// </summary>
		/// <remarks>This method updates the actual row count and triggers any associated logic. Override this method
		/// to customize behavior when the actual rows are set.</remarks>
		/// <param name="rows">The number of rows to set. Must be a non-negative integer.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public virtual Task SetActualRows(int rows)
		{
			this.ActualBounds.Rows = rows;
			this.OnSetActualRows(rows);
			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets the actual number of columns for the current bounds asynchronously.
		/// </summary>
		/// <param name="columns">The number of columns to set. Must be a non-negative integer.</param>
		/// <returns>A task that represents the asynchronous operation. The task completes when the column count has been updated.</returns>
		public virtual Task SetActualColumns(int columns)
		{
			this.ActualBounds.Columns = columns;
			this.OnSetActualColumns(columns);
			return Task.FromResult(0);
		}

		/// <summary>
		/// Provides a mechanism for derived classes to respond when the actual number of rows is set.
		/// </summary>
		/// <remarks>Override this method in a derived class to implement custom behavior when the actual row count
		/// changes. The base implementation does nothing.</remarks>
		/// <param name="rows">The number of rows that have been set. Must be a non-negative integer.</param>
		protected virtual void OnSetActualRows(int rows)
		{
		}

		/// <summary>
		/// Provides a callback that is invoked when the actual number of columns is set.
		/// </summary>
		/// <remarks>Override this method to perform custom actions when the actual column count changes. The default
		/// implementation does nothing.</remarks>
		/// <param name="columns">The number of columns that have been set. Must be a non-negative integer.</param>
		protected virtual void OnSetActualColumns(int columns)
		{
		}

		/// <summary>
		/// Renders debug visual elements for the specified PDF grid page and model within the given bounds.
		/// </summary>
		/// <remarks>This method is intended for diagnostic or development purposes and may display visual cues such
		/// as borders and labels to assist in layout debugging. Override this method to customize debug rendering
		/// behavior.</remarks>
		/// <param name="g">The PDF grid page on which debug visuals are rendered.</param>
		/// <param name="m">The model instance providing context for rendering debug information.</param>
		/// <param name="bounds">The bounds within which debug visuals are drawn.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if debug rendering
		/// was performed; otherwise, <see langword="false"/>.</returns>
		protected virtual Task<bool> OnRenderDebugAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Create a random color
			//
			XColor color = XColorExtensions.RandomColor();

			//
			// Set the label color.
			//
			XColor labelColor = color.Contrast(XColors.White) > color.Contrast(XColors.Black) ? XColors.White : XColors.Black;

			//
			// Draw a border around the section.
			//
			g.DrawRectangle(this.ActualBounds, 1.0, color);

			//
			// Draw a border to indicate the margin.
			//
			XPen pen = new XPen(XColors.LightGray, 1);
			pen.DashStyle = XDashStyle.Dot;
			g.DrawRectangle(bounds, pen);

			//
			// Get the size of the text.
			//
			var style = this.StyleManager.GetStyle("Debug");
			XFont font = style.Font.Resolve(g, m);
			string label = $"{this.Key} [{this.ActualBounds.Columns} x {this.ActualBounds.Rows}]";
			PdfSize textSize = g.MeasureText(font, label);

			//
			// Draw a small filled rectangle behind the text.
			//
			PdfSpacing padding = (1, 1, 1, 1);
			PdfBounds rectBounds = new PdfBounds(this.ActualBounds.LeftColumn, this.ActualBounds.TopRow, textSize.Columns + padding.Left + (2 * padding.Right), textSize.Rows + padding.Top + padding.Bottom);
			g.DrawFilledRectangle(rectBounds, color);

			//
			// Draw the text label.
			//
			PdfBounds labelBounds = new PdfBounds(this.ActualBounds.LeftColumn + padding.Left, this.ActualBounds.TopRow, textSize.Columns + padding.Left + padding.Right, textSize.Rows + padding.Top + padding.Bottom);
			g.DrawText(label, font, labelBounds, XStringFormats.CenterLeft, labelColor, true);

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Asynchronously renders content onto the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>Override this method to implement custom rendering logic for a PDF grid page. The default
		/// implementation always returns <see langword="true"/>.</remarks>
		/// <param name="g">The PDF grid page on which the content will be rendered.</param>
		/// <param name="m">The model containing data used for rendering the content.</param>
		/// <param name="bounds">The bounds within which the content should be rendered on the page.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if rendering was
		/// successful; otherwise, <see langword="false"/>.</returns>
		protected virtual Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// Returns a string that represents the current object, including its type and key information.
		/// </summary>
		/// <remarks>The returned string is useful for debugging and logging purposes, as it provides a concise
		/// summary of the object's identity within its hierarchy.</remarks>
		/// <returns>A string containing the type name and key of the current object, formatted with its parent section key if
		/// available.</returns>
		public override string ToString()
		{
			string parent = this.ParentSection == null ? "None" : this.ParentSection.Key;
			return $"{this.GetType().Name} [{parent}.{this.Key}]";
		}
	}
}
