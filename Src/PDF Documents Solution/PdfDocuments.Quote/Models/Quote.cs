using System;
using System.Collections.Generic;
using PdfDocument.DocumentShared;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;

namespace PdfDocument.QuoteDocument
{
	public class Quote : InsightPdfModel
	{
		public override string Id => this.QuoteNumber;

		public CarrierDetails Carrier { get; set; }
		public int TransitDays { get; set; }
		public DateTimeOffset ExpirationDateTime { get; set; }
		public int? GroupId { get; set; }
		public string TariffId { get; set; }
		public double FuelSurchargePercent { get; set; }
		public double FuelPrice { get; set; }
		public DateTime FuelEffectiveDate { get; set; }

		public IEnumerable<Address> Addresses { get; set; }
		public IEnumerable<Charge> Charges { get; set; }
	}
}
