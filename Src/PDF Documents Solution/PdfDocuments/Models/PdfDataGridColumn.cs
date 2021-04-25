using System.Linq.Expressions;
using PdfSharp.Drawing;

namespace PdfDocuments
{
	public class PdfDataGridColumn
	{
		public string ColumnHeader { get; set; }
		public MemberExpression MemberExpression { get; set; }
		public double RelativeWidth { get; set; }
		public string Format { get; set; }
		public XStringFormat Alignment { get; set; }
	}
}
