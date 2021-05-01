﻿using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfTextElement<TModel> : IPdfElement<TModel> where TModel : IPdfModel
	{
		public PdfTextElement(string text)
		{
			this.Text = text;
		}

		public string Text { get; set; }

		public PdfSize Measure(PdfGridPage g, TModel m, PdfStyle<TModel> style)
		{
			PdfSize returnValue = new PdfSize();

			//
			// Measure the text.
			//
			returnValue = g.MeasureText(style.Font.Resolve(g, m), this.Text);

			//
			// Add the margin
			//
			PdfSpacing margin = style.Margin.Resolve(g, m);
			returnValue.Columns += margin.Left + margin.Right;
			returnValue.Rows += margin.Top + margin.Bottom;

			//
			// Add the padding
			//
			PdfSpacing padding = style.Padding.Resolve(g, m);
			returnValue.Columns += padding.Left + padding.Right;
			returnValue.Rows += padding.Top + padding.Bottom;

			//
			// Add the cell padding
			//
			PdfSpacing cellPaddng = style.CellPadding.Resolve(g, m);
			returnValue.Columns += cellPaddng.Left + cellPaddng.Right;
			returnValue.Rows += cellPaddng.Top + cellPaddng.Bottom;


			return returnValue;
		}

		public void Render(PdfGridPage g, TModel m, PdfBounds bounds, PdfStyle<TModel> style)
		{
			//
			// Apply margin. Nothing is drawn in the margin.
			//
			PdfBounds marginBounds = bounds.SubtractBounds(g, m, style.Margin.Resolve(g, m));

			//
			// Draw the background.
			//
			g.DrawFilledRectangle(marginBounds, style.BackgroundColor.Resolve(g, m));

			//
			// Draw the border.
			//
			XPen pen = new XPen(style.BorderColor.Resolve(g, m), style.BorderWidth.Resolve(g, m));
			g.DrawRectangle(marginBounds, pen);

			//
			// Apply padding. This is where the fill and border will extend to.
			//
			PdfBounds elementBounds = marginBounds.SubtractBounds(g, m, style.Padding.Resolve(g, m));
			
			//
			// Pad the text. This allows the border and fill to extend beyond the text.
			//
			PdfBounds textBounds = elementBounds.SubtractBounds(g, m, style.CellPadding.Resolve(g, m));

			//
			// Draw the text.
			//
			g.DrawText(this.Text, style.Font.Resolve(g, m), textBounds, style.TextAlignment.Resolve(g, m), style.ForegroundColor.Resolve(g, m));
		}
	}
}