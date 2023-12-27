/*
 *	MIT License
 *
 *	Copyright (c) 2021-2024 Daniel Porrey
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
		public virtual BindProperty<string, TModel> Text { get; set; } = string.Empty;
		public virtual IPdfSection<TModel> ParentSection { get; set; }
		public virtual IList<IPdfSection<TModel>> Children { get; } = new List<IPdfSection<TModel>>();
		public virtual PdfBounds ActualBounds { get; set; } = new PdfBounds(0, 0, 0, 0);
		public virtual BindProperty<bool, TModel> ShouldRender { get; set; } = true;
		public virtual BindProperty<string, TModel> WaterMarkImagePath { get; set; } = string.Empty;
		public virtual IEnumerable<string> StyleNames { get; set; } = new string[] { PdfStyleManager<TModel>.Default };

		IPdfStyleManager<TModel> _styleManager = null;
		public virtual  IPdfStyleManager<TModel> StyleManager
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

		public virtual BindProperty<double, TModel> RelativeHeight => this.ResolveStyle(0).RelativeHeight;
		public virtual BindProperty<double[], TModel> RelativeWidths => this.ResolveStyle(0).RelativeWidths;

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

		protected virtual Task<bool> OnLayoutAsync(PdfGridPage g, TModel m, PdfBounds bounds)
		{
			return Task.FromResult(true);
		}

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

		protected virtual bool OnShouldDrawBackground()
		{
			return true;
		}

		protected virtual bool OnShouldDrawBorder()
		{
			return true;
		}

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

		protected virtual Task<bool> OnRenderAsync(PdfGridPage g, TModel m, PdfBounds bounds)
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
