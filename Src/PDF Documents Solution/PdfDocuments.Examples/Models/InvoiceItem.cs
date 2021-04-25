namespace PdfDocuments.Example
{
	public class InvoiceItem
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Amount => this.Quantity * this.UnitPrice;
	}
}
