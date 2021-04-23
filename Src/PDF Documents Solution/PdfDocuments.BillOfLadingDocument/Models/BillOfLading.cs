using System;
using System.Collections.Generic;
using PdfDocument.DocumentShared;
using Lsc.Logistics.Insight.Shared.Rating.Abstractions;

namespace PdfDocument.BillOfLadingDocument
{
	public class BillOfLading : InsightPdfModel
	{
		public override string Id => this.ShipmentId.ToString();

		public DateTimeOffset PickupDateTime { get; set; }
		
		/// <summary>
		/// Contains information about thew carrier.
		/// </summary>
		public CarrierDetails Carrier { get; set; }
		
		/// <summary>
		/// Contains information important to the pickup location or in regards to scheduling a pickup.
		/// </summary>
		public string PickupNote { get; set; }

		/// <summary>
		/// Contains information important to the delivery location or in regards to scheduling a delivery.
		/// </summary>
		public string DeliveryNote { get; set; }

		/// <summary>
		/// Contains special information for the carrier.
		/// </summary>
		public string SpecialInstructions { get; set; }

		public Reference Reference1 { get; set; }
		public Reference Reference2 { get; set; }
		public Reference Reference3 { get; set; }
		public Reference Reference4 { get; set; }

		public Address FreightBillTo { get; set; } = new Address()
		{
			Name = "LSC Communications MCL, LLC Accounts Payable",
			Address1 = "1000 WINDHAM PARKWAY",
			Address2 = null,
			City = "BOLINGBROOK",
			State = "IL",
			Zip = "60490",
			Comment = "Invoice (paper or electronic) MUST contain the BOL number and be labeled 'MANIFEST'."
		};

		public IEnumerable<AccessorialResponse> Accessorials { get; set; }
		public IEnumerable<LineItem> LineItems { get; set; }
		public string FreightTerms { get; set; } = "PREPAID";
		public DateTime? Delivered { get; set; }
		public string DeliveryReceivedBy { get; set; }
	}
}
