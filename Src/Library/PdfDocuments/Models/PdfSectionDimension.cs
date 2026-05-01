namespace PdfDocuments
{
	/// <summary>
	/// Represents the dimension of a section in a PDF document, specifying whether the size is fixed or flexible and its
	/// associated value.
	/// </summary>
	public class PdfSectionDimension
	{
		/// <summary>
		/// Initializes a new instance of the PdfSectionDimension class with the specified dimension mode and value.
		/// </summary>
		/// <param name="isFixed">true to specify a fixed dimension; otherwise, false to indicate a flexible or automatic dimension.</param>
		/// <param name="value">The value of the dimension. The meaning of this value depends on the isFixed parameter.</param>
		public PdfSectionDimension(bool isFixed, int value)
		{
			this.Fixed = isFixed;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the value is fixed and cannot be changed.
		/// </summary>
		public bool Fixed { get; set; }

		/// <summary>
		/// Gets or sets the integer value represented by this property.
		/// </summary>
		public int Value { get; set; }
	}
}
