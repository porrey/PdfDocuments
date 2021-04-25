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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PdfDocuments.Theme.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public abstract class PdfSection<TModel> : IPdfSection<TModel>
		where TModel : IPdfModel
	{
		public PdfSection()
		{
			this.SetKey();
		}

		public PdfSection(params IPdfSection<TModel>[] children)
			: this()
		{
			foreach (IPdfSection<TModel> child in children)
			{
				this.AddChildSection(child);
			}
		}

		public virtual string Key { get; set; }
		public BindProperty<string, TModel> Text { get; set; } = string.Empty;
		public BindProperty<XStringFormat, TModel> TextAlignment { get; set; } = XStringFormats.CenterLeft;

		public virtual BindProperty<double, TModel> RelativeHeight { get; set; } = 0.0;
		public virtual BindProperty<double, TModel> RelativeWidth { get; set; } = 0.0;

		public virtual PdfBounds ActualBounds { get; set; } = new PdfBounds(0, 0, 0, 0);
		public virtual PdfSpacing Padding { get; set; } = new PdfSpacing(1, 1, 1, 1);
		public virtual PdfSpacing Margin { get; set; } = new PdfSpacing(0, 0, 0, 0);

		public virtual IPdfSection<TModel> ParentSection { get; set; }

		public virtual BindProperty<bool, TModel> UsePadding { get; set; } = true;
		public virtual BindProperty<bool, TModel> UseMargins { get; set; } = true;

		public virtual BindProperty<bool, TModel> ShouldRender { get; set; } = true;

		public virtual BindProperty<XFont, TModel> Font { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyFont(); });
		public virtual BindProperty<XFont, TModel> DebugFont { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.DebugFont(); });

		public virtual IList<IPdfSection<TModel>> Children { get; } = new List<IPdfSection<TModel>>();

		public virtual BindProperty<double, TModel> BorderWidth { get; set; } = 0.0;
		public virtual BindProperty<XColor, TModel> BorderColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.BodyColor; });

		public virtual BindProperty<XColor, TModel> BackgroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.BodyBackgroundColor; });
		public virtual BindProperty<XColor, TModel> ForegroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.BodyColor; });

		public BindProperty<string, TModel> WaterMarkImagePath { get; set; } = string.Empty;

		public virtual async Task<bool> LayoutAsync(PdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Apply margins.
			//
			PdfBounds bounds = this.ApplyMargins(gridPage, model);

			if (await this.OnLayoutAsync(gridPage, model, bounds))
			{
				returnValue = await this.OnLayoutChildrenAsync(gridPage, model, bounds);
			}
			else
			{
				returnValue = false;
			}

			return returnValue;
		}

		protected virtual Task<bool> OnLayoutAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			return Task.FromResult(true);
		}

		protected virtual async Task<bool> OnLayoutChildrenAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			bool returnValue = true;

			//
			// Render child sections.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(gridPage, model)).ToArray();

			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.LayoutAsync(gridPage, model))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}

		public virtual async Task<bool> RenderAsync(PdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Draw the background if set.
			//
			XColor backgroundColor = this.BackgroundColor.Resolve(gridPage, model);
			if (backgroundColor != XColor.Empty)
			{
				//
				// Draw the filled rectangle.
				//
				gridPage.DrawFilledRectangle(this.ActualBounds, backgroundColor);
			}

			//
			// Apply margins.
			//
			PdfBounds bounds = this.ApplyMargins(gridPage, model);

			if (await this.OnRenderAsync(gridPage, model, bounds))
			{
				//
				// Render child sections.
				//
				IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(gridPage, model)).ToArray();

				foreach (IPdfSection<TModel> section in sections)
				{
					if (!await section.RenderAsync(gridPage, model))
					{
						returnValue = false;
						break;
					}
				}

				//
				// Render the border.
				//
				double borderWidth = this.BorderWidth.Resolve(gridPage, model);
				if (borderWidth > 0.0)
				{
					gridPage.DrawRectangle(this.ActualBounds, borderWidth, this.BorderColor.Resolve(gridPage, model));
				}
			}
			else
			{
				returnValue = false;
			}

			//
			// Draw the page water mark.
			//
			string waterMarkPath = this.WaterMarkImagePath.Resolve(gridPage, model);
			if (!string.IsNullOrWhiteSpace(waterMarkPath) && File.Exists(waterMarkPath))
			{
				gridPage.DrawImage(waterMarkPath, this.ActualBounds, PdfHorizontalAlignment.Center, PdfVerticalAlignment.Center);
			}

			return returnValue;
		}

		public virtual async Task<bool> RenderDebugAsync(PdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Get the margin bounds.
			//
			PdfBounds marginBounds = this.ApplyMargins(gridPage, model);

			//
			// Call the base to render the outline overlay.
			//
			await this.OnRenderDebugAsync(gridPage, model, marginBounds);

			//
			// Get a list of section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Resolve(gridPage, model)).ToArray();

			foreach (IPdfSection<TModel> section in sections)
			{
				if (!await section.RenderDebugAsync(gridPage, model))
				{
					returnValue = false;
					break;
				}
			}

			return returnValue;
		}

		public virtual Task SetActualRows(int rows)
		{
			this.ActualBounds.Rows = rows;
			this.OnSetActualRows(rows);
			return Task.FromResult(0);
		}

		public virtual Task SetActualColumns(int columns)
		{
			this.ActualBounds.Columns = columns;
			this.OnSetActualColumns(columns);
			return Task.FromResult(0);
		}

		protected virtual void OnSetActualRows(int rows)
		{
		}

		protected virtual void OnSetActualColumns(int columns)
		{
		}

		protected virtual Task<bool> OnRenderDebugAsync(PdfGridPage gridPage, TModel model, PdfBounds marginBounds)
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
			gridPage.DrawRectangle(this.ActualBounds, 1.0, color);

			//
			// Draw a border to indicate the margin.
			//
			XPen pen = new XPen(XColors.LightGray, 1);
			pen.DashStyle = XDashStyle.Dot;
			gridPage.DrawRectangle(marginBounds, pen);

			//
			// Get the size of the text.
			//
			XFont font = this.DebugFont.Resolve(gridPage, model);
			string label = $"{this.Key} [{this.ActualBounds.Columns} x {this.ActualBounds.Rows}]";
			PdfSize textSize = gridPage.MeasureText(font, label);

			//
			// Draw a small filled rectangle behind the text.
			//
			PdfSpacing padding = (1, 1, 1, 1);
			PdfBounds rectBounds = new PdfBounds(this.ActualBounds.LeftColumn, this.ActualBounds.TopRow, textSize.Columns + padding.Left + (2 * padding.Right), textSize.Rows + padding.Top + padding.Bottom);
			gridPage.DrawFilledRectangle(rectBounds, color);

			//
			// Draw the text label.
			//
			PdfBounds labelBounds = new PdfBounds(this.ActualBounds.LeftColumn + padding.Left, this.ActualBounds.TopRow, textSize.Columns + padding.Left + padding.Right, textSize.Rows + padding.Top + padding.Bottom);
			gridPage.DrawText(label, font, labelBounds, XStringFormats.CenterLeft, labelColor, true);

			return Task.FromResult(returnValue);
		}

		protected virtual Task<bool> OnRenderAsync(PdfGridPage gridPage, TModel model, PdfBounds bounds)
		{
			return Task.FromResult(true);
		}

		public override string ToString()
		{
			string parent = this.ParentSection == null ? "None" : this.ParentSection.Key;
			return $"{this.GetType().Name} [{parent}.{this.Key}]";
		}
	}
}
