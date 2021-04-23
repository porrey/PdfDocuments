using System.ComponentModel.DataAnnotations;

namespace PdfDocument
{
	/// <summary>
	/// Contains the elements of the PDF document.
	/// </summary>
	public class Pdf
	{
		/// <summary>
		/// The name that should be used as the file name when saving this document.
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// The binary data containing the PDF file.
		/// </summary>
		[Required]
		public byte[] Data { get; set; }

		/// <summary>
		/// Indicates whether or not the binary data is compressed with
		/// a GZip stream or not. The default is false.
		/// </summary>
		public bool IsCompressed { get; set; }
	}
}
