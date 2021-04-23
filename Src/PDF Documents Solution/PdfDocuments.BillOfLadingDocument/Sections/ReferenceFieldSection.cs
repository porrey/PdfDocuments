using System.Threading.Tasks;
using PdfDocument.Abstractions;
using PdfSharp.Drawing;

namespace PdfDocument.BillOfLadingDocument
{
	public class ReferenceFieldSection : PdfSection<BillOfLading>
	{
		public override Task<bool> RenderAsync(IPdfGridPage gridPage, BillOfLading model)
		{
			bool returnValue = true;

			// ***
			// *** Define the font.
			// ***
			XFont bodyFont = gridPage.BodyLightFont().WithSize(gridPage.Theme.FontSize.Body);

			int top = this.ActualBounds.TopRow + this.Padding.Top;
			int height = this.ActualBounds.Rows - this.Padding.Top - this.Padding.Bottom;
			int width = (this.ActualBounds.Columns - this.Padding.Left - this.Padding.Right) / 4;
			int left = this.Padding.Left;

			gridPage.DrawText($"{model.Reference1.Name}: {model.Reference1.Value}", bodyFont, left, top, width, height, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

			left += width;
			gridPage.DrawText($"{model.Reference2.Name}: {model.Reference2.Value}", bodyFont, left, top, width, height, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

			left += width;
			gridPage.DrawText($"{model.Reference3.Name}: {model.Reference3.Value}", bodyFont, left, top, width, height, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

			left += width;
			gridPage.DrawText($"{model.Reference4.Name}: {model.Reference4.Value}", bodyFont, left, top, width, height, XStringFormats.CenterLeft, gridPage.Theme.Color.BodyLightColor);

			return Task.FromResult(returnValue);
		}
	}
}
