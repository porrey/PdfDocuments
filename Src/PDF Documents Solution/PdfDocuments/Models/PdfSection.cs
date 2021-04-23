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
	{
		public PdfSection()
		{
		}

		public PdfSection(params IPdfSection<TModel>[] children)
		{
			foreach (IPdfSection<TModel> child in children)
			{
				this.AddChildSection(child);
			}
		}

		public virtual string Key { get; set; }
		public BindProperty<string, TModel> Text { get; set; } = new BindPropertyAction<string, TModel>((gp, m) => { return string.Empty; });

		public virtual BindProperty<double, TModel> RelativeHeight { get; set; } = 0.0;
		public virtual BindProperty<double, TModel> RelativeWidth { get; set; } = 0.0;

		public virtual IPdfBounds ActualBounds { get; set; } = new PdfBounds() { TopRow = 0, LeftColumn = 0, Rows = 0, Columns = 0 };
		public virtual IPdfSpacing Padding { get; set; } = new PdfSpacing() { Left = 1, Top = 1, Bottom = 1, Right = 1 };
		public virtual IPdfSpacing Margin { get; set; } = new PdfSpacing() { Left = 1, Top = 1, Bottom = 1, Right = 1 };

		public virtual IPdfSection<TModel> ParentSection { get; set; }

		public virtual BindProperty<bool, TModel> UsePadding { get; set; } = true;
		public virtual BindProperty<bool, TModel> UseMargins { get; set; } = false;

		public virtual BindProperty<bool, TModel> ShouldRender { get; set; } = true;

		public virtual BindProperty<XFont, TModel> Font { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.BodyFont(); });
		public virtual BindProperty<XFont, TModel> DebugFont { get; set; } = new BindPropertyAction<XFont, TModel>((gp, m) => { return gp.DebugFont(); });

		private IList<IPdfSection<TModel>> _children = new List<IPdfSection<TModel>>();
		public virtual IList<IPdfSection<TModel>> Children
		{
			get
			{
				return _children;
			}
			set
			{
				_children = value;

				if (_children != null)
				{
					foreach (IPdfSection<TModel> child in _children)
					{
						child.ParentSection = this;
					}
				}
			}
		}

		public virtual BindProperty<double, TModel> BorderWidth { get; set; } = 0.0;
		public virtual BindProperty<XColor, TModel> BorderColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.BodyColor; });

		public virtual BindProperty<XColor, TModel> BackgroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return XColor.Empty; });
		public virtual BindProperty<XColor, TModel> ForegroundColor { get; set; } = new BindPropertyAction<XColor, TModel>((gp, m) => { return gp.Theme.Color.BodyColor; });

		public BindProperty<string, TModel> WaterMarkImagePath { get; set; } = string.Empty;

		public virtual async Task<bool> LayoutAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			if (await this.OnLayoutAsync(gridPage, model))
			{
				//
				// Adjust for margins.
				//
				if (this.UseMargins.Invoke(gridPage, model))
				{
					this.ActualBounds = this.ActualBounds.ApplyMargins(gridPage, this.Margin);
				}

				returnValue = await this.OnLayoutChildrenAsync(gridPage, model);
			}
			else
			{
				returnValue = false;
			}

			return returnValue;
		}

		protected virtual Task<bool> OnLayoutAsync(IPdfGridPage gridPage, TModel model)
		{
			return Task.FromResult(true);
		}

		protected virtual async Task<bool> OnLayoutChildrenAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Render child sections.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Invoke(gridPage, model)).ToArray();

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

		public virtual async Task<bool> RenderAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Draw the background if set.
			//
			XColor backgroundColor = this.BackgroundColor.Invoke(gridPage, model);
			if (backgroundColor != XColor.Empty)
			{
				//
				// Draw the filled rectangle.
				//
				gridPage.DrawFilledRectangle(this.ActualBounds, backgroundColor);
			}

			//
			// Call the default rendering routine.
			//
			if (await this.OnRenderAsync(gridPage, model))
			{
				//
				// Render child sections.
				//
				IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Invoke(gridPage, model)).ToArray();

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
				double borderWidth = this.BorderWidth.Invoke(gridPage, model);
				if (borderWidth > 0.0)
				{
					gridPage.DrawRectangle(this.ActualBounds, borderWidth, this.BorderColor.Invoke(gridPage, model));
				}
			}
			else
			{
				returnValue = false;
			}

			//
			// Draw the page water mark.
			//
			string waterMarkPath = this.WaterMarkImagePath.Invoke(gridPage, model);
			if (!string.IsNullOrWhiteSpace(waterMarkPath) && File.Exists(waterMarkPath))
			{
				gridPage.DrawImage(waterMarkPath, this.ActualBounds, HorizontalAlignment.Center, VerticalAlignment.Center);
			}

			return returnValue;
		}

		public virtual async Task<bool> RenderDebugAsync(IPdfGridPage gridPage, TModel model)
		{
			bool returnValue = true;

			//
			// Call the base to render the outline overlay.
			//
			await this.OnRenderDebugAsync(gridPage, model);

			//
			// Get a list of section to be rendered.
			//
			IPdfSection<TModel>[] sections = this.Children.Where(t => t.ShouldRender.Invoke(gridPage, model)).ToArray();

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

		public virtual void AddChildSection(IPdfSection<TModel> childSection)
		{
			if (childSection != null)
			{
				childSection.ParentSection = this;
				this.Children.Add(childSection);
			}
		}

		protected virtual void OnSetActualRows(int rows)
		{
		}

		protected virtual void OnSetActualColumns(int columns)
		{
		}

		protected virtual Task<bool> OnRenderDebugAsync(IPdfGridPage gridPage, TModel model)
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
			// Get the size of the text.
			//
			XFont font = this.DebugFont.Invoke(gridPage, model);
			string label = $"{this.Key} [{this.ActualBounds.Columns} x {this.ActualBounds.Rows}]";
			IPdfSize textSize = gridPage.MeasureText(font, label);

			//
			// Draw a small filled rectangle behind the text.
			//
			IPdfBounds rectBounds = new PdfBounds(this.ActualBounds.LeftColumn, this.ActualBounds.TopRow, textSize.Columns + this.Padding.Left + (2 * this.Padding.Right), textSize.Rows + this.Padding.Top + this.Padding.Bottom);
			gridPage.DrawFilledRectangle(rectBounds, color);

			//
			// Draw the text label.
			//
			IPdfBounds labelBounds = new PdfBounds(this.ActualBounds.LeftColumn + this.Padding.Left, this.ActualBounds.TopRow, textSize.Columns + this.Padding.Left + this.Padding.Right, textSize.Rows + this.Padding.Top + this.Padding.Bottom);
			gridPage.DrawText(label, font, labelBounds, XStringFormats.CenterLeft, labelColor, true);

			return Task.FromResult(returnValue);
		}

		protected virtual Task<bool> OnRenderAsync(IPdfGridPage gridPage, TModel model)
		{
			return Task.FromResult(true);
		}

		public override string ToString()
		{
			string parent = this.ParentSection == null ? "None" : this.ParentSection.Key;
			return $"{this.Key} [{parent}]";
		}
	}
}
