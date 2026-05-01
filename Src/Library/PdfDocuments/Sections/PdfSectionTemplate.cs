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
using PdfDocuments.Exceptions;
using PdfDocuments.Layout_Manager;
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
		/// Backing field for the StyleManager property. This field is used to store the style manager instance that controls
		/// </summary>
		private IPdfStyleManager<TModel> _styleManager = null;

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
		/// Gets or sets the layout mode used for arranging child sections.
		/// </summary>
		/// <remarks>Use this property to control how child sections are positioned and displayed within the parent
		/// container. The selected layout mode determines the arrangement behavior for all immediate child
		/// sections.</remarks>
		public PdfSectionsLayoutMode SectionLayoutMode { get; set; }

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
		/// Gets or sets the actual bounding rectangle of the section in PDF coordinates.
		/// </summary>
		public virtual PdfBounds SectionArea { get; set; } = new PdfBounds(0, 0, 0, 0);

		/// <summary>
		/// Gets or sets the renderable bounding rectangle of the section in PDF coordinates.
		/// </summary>
		public virtual PdfBounds RenderArea { get; set; } = new PdfBounds(0, 0, 0, 0);

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
		/// default value includes the standard style provided by the manager. Modifying this collection allows
		/// customization of appearance or behavior based on style definitions. Typical sections will have one
		/// style name, but sections that have multiple subsections can use more than one style.</remarks>
		public virtual IEnumerable<string> StyleNames { get; set; } = [PdfStyleManager<TModel>.Default];

		/// <summary>
		/// Gets or sets the style manager used to control PDF rendering styles for the current section and its child
		/// sections.
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
		/// Gets the factory used to create layout manager instances for arranging UI sections.
		/// </summary>
		/// <remarks>The returned factory provides layout managers that control the positioning and sizing of child
		/// sections. Override this property in a derived class to supply a custom layout manager factory if specialized
		/// layout behavior is required.</remarks>
		public virtual IPdfLayoutManagerFactory LayoutManagerFactory { get; } = new PdfLayoutManagerFactory();

		/// <summary>
		/// Asynchronously renders the specified grid page using the provided model and bounds.
		/// </summary>
		/// <param name="gridPage">The grid page to render. Cannot be null.</param>
		/// <param name="model">The data model used to populate the grid page. Cannot be null.</param>
		/// <param name="bounds">The bounds within which the grid page should be rendered.</param>
		/// <returns>A task that represents the asynchronous render operation. The task result is <see langword="true"/> if rendering
		/// succeeds; otherwise, <see langword="false"/>.</returns>
		public Task RenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			return this.OnRenderAsync(gridPage, model, bounds);
		}

		/// <summary>
		/// Renders debug overlays and outlines for the current section and its child sections asynchronously.
		/// </summary>
		/// <remarks>This method renders debug overlays for the section and recursively for all child sections that
		/// should be rendered. It can be used to visually inspect layout boundaries and section outlines during PDF
		/// generation.</remarks>
		/// <param name="g">The PDF grid page on which the debug overlays are rendered.</param>
		/// <param name="m">The model instance providing data for rendering the debug overlays.</param>
		/// <param name="bounds">The bounds within which the grid page should be rendered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all debug overlays
		/// were rendered successfully; otherwise, <see langword="false"/>.</returns>
		public virtual Task RenderDebugAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			return this.OnRenderDebugAsync(g, m, bounds);
		}

		/// <summary>
		/// Determines whether the background should be drawn for the current section.
		/// </summary>
		/// <returns>true if the background should be drawn; otherwise, false.</returns>
		protected virtual bool OnShouldDrawBackground()
		{
			return true;
		}

		/// <summary>
		/// Determines whether a border should be drawn for the current section.
		/// </summary>
		/// <remarks>Override this method in a derived class to customize the border drawing behavior based on
		/// specific conditions.</remarks>
		/// <returns>Returns <see langword="true"/> if a border should be drawn; otherwise, <see langword="false"/>.</returns>
		protected virtual bool OnShouldDrawBorder()
		{
			return true;
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
		protected virtual async Task OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Check if this section should be rendered or not.
			//
			if (this.ShouldRender.Resolve(g, m))
			{
				//
				// The first style is always used for the base section style.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

				//
				// Apply padding.
				//
				PdfSpacing padding = style.Padding.Resolve(g, m);
				PdfBounds paddedBounds = this.ApplyPadding(g, m, padding);

				//
				// Render the background with padding.
				//
				await this.OnRenderBackgroundAsync(g, m, paddedBounds);

				//
				// Render the border with padding.
				//
				await this.OnRenderBorderAsync(g, m, paddedBounds);

				//
				// Render the text.
				//
				await this.OnRenderTextAsync(g, m, paddedBounds);

				//
				// Render the child sections.
				//
				await this.OnRenderChildrenAsync(g, m, paddedBounds);

				//
				// Render the watermark on top of all other sections.
				//
				await this.OnRenderWaterMarkAsync(g, m, paddedBounds);
			}
		}

		/// <summary>
		/// Renders debug visual sections for the specified PDF grid page and model within the given bounds.
		/// </summary>
		/// <remarks>This method is intended for diagnostic or development purposes and may display visual cues such
		/// as borders and labels to assist in layout debugging. Override this method to customize debug rendering
		/// behavior.</remarks>
		/// <param name="g">The PDF grid page on which debug visuals are rendered.</param>
		/// <param name="m">The model instance providing context for rendering debug information.</param>
		/// <param name="bounds">The bounds within which debug visuals are drawn.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if debug rendering
		/// was performed; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task OnRenderDebugAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Create a random color
			//
			XColor color = XColorExtensions.RandomColor();

			//
			// Set the label color.
			//
			XColor labelColor = color.Contrast(XColors.White) > color.Contrast(XColors.Black) ? XColors.White : XColors.Black;

			//
			// Draw a border around the renderable area and fill the it
			// with a lighter version of the color using alpha so the
			// fill does not cover up the rendered section. The background
			// will have diagonal lines through it. This fill will
			// highlight the margin area only.
			//
			if (this.RenderArea != this.SectionArea)
			{
				XColor backgroundColor = color.WithLuminosity(.65).WithAlpha(.15);
				XColor lineColor = color.WithLuminosity(.55).WithAlpha(.15);
				g.DrawFilledRectangle(this.SectionArea, this.RenderArea, backgroundColor, lineColor);
			}

			g.DrawRectangle(this.RenderArea, 1.0, color);

			//
			// Get a pen for the border around the section.
			//
			XPen pen = new(color, 1)
			{
				DashStyle = XDashStyle.Dash
			};

			//
			// Draw a border around the section area.
			//
			g.DrawRectangle(this.SectionArea, pen);

			//
			// Get the parent count.
			//
			int parentCount = -1;
			IPdfSection<TModel> parent = this.ParentSection;

			while (parent != null)
			{
				parentCount++;
				parent = parent.ParentSection;
			}

			//
			// Get the size of the text.
			//
			PdfStyle<TModel> style = this.StyleManager.GetStyle("Debug");
			XFont font = style.Font.Resolve(g, m);
			string label = $"{this.Key} [x{this.RenderArea.LeftColumn}, y{this.RenderArea.TopRow}, w{this.RenderArea.Columns}, h{this.RenderArea.Rows}]";
			PdfSize textSize = g.MeasureText(font, label);

			//
			// Create padding fo the box and text.
			//
			PdfSpacing padding = (1, 1, 1, 1);

			//
			// Draw a small filled rectangle behind the text.
			//
			PdfBounds rectBounds = new(this.RenderArea.LeftColumn, this.RenderArea.TopRow, textSize.Columns + padding.Left + (2 * padding.Right), textSize.Rows + padding.Top + padding.Bottom);
			g.DrawFilledRectangle(rectBounds, color);

			//
			// Draw the text label.
			//			
			PdfBounds labelBounds = new(this.RenderArea.LeftColumn + padding.Left, this.RenderArea.TopRow, textSize.Columns + padding.Left + padding.Right, textSize.Rows + padding.Top + padding.Bottom);
			g.DrawText(label, font, labelBounds, XStringFormats.CenterLeft, labelColor, true);

			//
			// Get a list of section to be rendered.
			//
			IPdfSection<TModel>[] sections = [.. this.Children.Where(t => t.ShouldRender.Resolve(g, m))];

			foreach (IPdfSection<TModel> section in sections)
			{
				await section.RenderDebugAsync(g, m, section.SectionArea);
			}
		}

		/// <summary>
		/// Renders the background for the grid page using the resolved style and model data.
		/// </summary>
		/// <remarks>Override this method to customize how the background is rendered for a grid page. The default
		/// implementation uses the first style to determine the background color and draws a filled rectangle if a background
		/// color is specified.</remarks>
		/// <param name="g">The <see cref="PdfGridPage"/> on which the background will be rendered.</param>
		/// <param name="m">The model data used to resolve style and background color.</param>
		/// <param name="bounds">The bounds within which the background should be rendered.</param>
		/// <returns>A task that represents the asynchronous operation. The task is completed when the background rendering is
		/// finished.</returns>
		protected virtual Task OnRenderBackgroundAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (this.OnShouldDrawBackground())
			{
				//
				// The first style is always used for the base section style.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

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

			return Task.CompletedTask;
		}

		/// <summary>
		/// Renders the border for the current grid section if border rendering is enabled and the border width is greater
		/// than zero.
		/// </summary>
		/// <remarks>Override this method to customize border rendering behavior for derived grid sections. The border
		/// is only rendered if the resolved border width is greater than zero.</remarks>
		/// <param name="g">The PDF grid page on which the border will be rendered.</param>
		/// <param name="m">The model instance providing data for style and rendering resolution.</param>
		/// <param name="bounds">The bounds within which the border should be drawn.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		protected virtual Task OnRenderBorderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			if (this.OnShouldDrawBorder())
			{
				//
				// The first style is always used for the base section style.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

				//
				// Render the border.
				//
				double borderWidth = style.BorderWidth.Resolve(g, m);

				if (borderWidth > 0.0)
				{
					g.DrawRectangle(bounds, borderWidth, style.BorderColor.Resolve(g, m));
				}
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Asynchronously renders text content for a grid cell within the specified bounds on a PDF page.
		/// </summary>
		/// <remarks>Override this method in a derived class to customize how text is rendered for a grid cell. The
		/// default implementation returns <see langword="true"/>.</remarks>
		/// <param name="g">The PDF grid page on which the text will be rendered.</param>
		/// <param name="m">The data model containing the information to be rendered as text.</param>
		/// <param name="bounds">The bounds that define the area within which the text should be rendered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the text was
		/// rendered successfully; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task OnRenderTextAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Get the text.
			//
			string text = this.Text.Resolve(g, m);

			if (text != null)
			{
				//
				// Get the style.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

				//
				// Create a new text element.
				//
				PdfTextElement<TModel> textElement = new(text);

				//
				// Render the text element.
				//
				await textElement.RenderAsync(g, m, bounds, style);
			}
		}

		/// <summary>
		/// Renders a watermark image onto the specified PDF grid page using the provided model and bounds.
		/// </summary>
		/// <remarks>Override this method to customize watermark rendering behavior. The default implementation
		/// resolves the watermark image path and draws the image centered within the specified bounds if the image
		/// exists.</remarks>
		/// <param name="g">The PDF grid page on which to render the watermark.</param>
		/// <param name="m">The model containing data used to resolve the watermark image path.</param>
		/// <param name="bounds">The bounds within which the watermark should be rendered.</param>
		/// <returns>A completed task that represents the asynchronous operation.</returns>
		protected virtual async Task OnRenderWaterMarkAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Get the path.
			//
			string path = this.WaterMarkImagePath.Resolve(g, m);

			if (path != null)
			{
				//
				// Get the style.
				//
				PdfStyle<TModel> style = this.ResolveStyle(0);

				//
				// Create a new text element.
				//
				PdfImageElement<TModel> imageElement = new(path);

				//
				// Render the text element.
				//
				await imageElement.RenderAsync(g, m, bounds, style);
			}
		}

		/// <summary>
		/// Asynchronously renders all child sections within the specified bounds using the provided PDF grid page and model.
		/// </summary>
		/// <remarks>Rendering stops at the first child section that fails to render successfully.</remarks>
		/// <param name="g">The PDF grid page on which the child sections are rendered.</param>
		/// <param name="m">The model containing data used for rendering the child sections.</param>
		/// <param name="bounds">The bounds within which the child sections are rendered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all child sections
		/// are rendered successfully; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task OnRenderChildrenAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			//
			// Get the child sections that should be rendered. If a child section is not
			// to be rendered, it will be ignored when calcuating the layout (making it hidden).
			//
			IPdfSection<TModel>[] sections = [.. this.Children.Where(t => t.ShouldRender.Resolve(g, m))];

			if (sections.Length != 0)
			{
				//
				// Perform the layout of the child sections.
				//
				await this.OnLayoutChildrenAsync(g, m, sections, bounds);

				//
				// Render each child section.
				//
				foreach (IPdfSection<TModel> section in sections)
				{
					//
					// Render the child section.
					//
					await section.RenderAsync(g, m, section.RenderArea);
				}
			}
		}

		/// <summary>
		/// Asynchronously arranges the child sections within the specified bounds within the PDF section.
		/// </summary>
		/// <param name="g">The PDF grid page on which the child sections are to be laid out.</param>
		/// <param name="m">The model containing data used for layout calculations.</param>
		/// <param name="sections">An array of sections to be arranged within the grid page.</param>
		/// <param name="bounds">The bounds within which the child sections should be laid out.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an array of bounds representing the
		/// layout of each child section.</returns>
		protected virtual async Task OnLayoutChildrenAsync(PdfGridPage g, TModel m, IPdfSection<TModel>[] sections, PdfBounds bounds)
		{
			//
			// Get the layout manager.
			//
			IPdfLayoutManager layoutManager = await this.LayoutManagerFactory.GetLayoutManagerAsync(this.SectionLayoutMode);

			if (layoutManager != null)
			{
				//
				// Perform the layout of the child sections. These bounds will have the 
				// margin applied, so they will be the actual bounds used for rendering
				// the child sections. The layout manager will also set the actual bounds
				// for each child section
				//
				await layoutManager.LayoutAsync(g, m, this, sections, bounds);
			}
			else
			{
				//
				// Throw a layout manager exception if the layout manager factory does not
				// return a layout manager for the specified layout mode.
				//
				throw new LayoutManagerException(this.SectionLayoutMode);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the style has been overridden for this instance.
		/// </summary>
		public virtual bool StyleOverridden { get; set; }

		/// <summary>
		/// Gets the style settings applied to this instance of a PDF section.
		/// </summary>
		public virtual PdfStyle<TModel> Style { get; } = new PdfStyle<TModel>();

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
