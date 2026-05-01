namespace PdfDocuments
{
	internal class SectionDimension
	{
		public SectionDimension(bool isFixed, int value)
		{
			this.Fixed = isFixed;
			this.Value = value;
		}

		public bool Fixed { get; set; }
		public int Value { get; set; }
	}
}
