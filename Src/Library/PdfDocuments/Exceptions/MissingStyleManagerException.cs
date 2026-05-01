namespace PdfDocuments.Exceptions
{
	/// <summary>
	/// Represents an exception that is thrown when a required style manager is not set during PDF document generation.
	/// </summary>
	/// <remarks>This exception indicates that the PDF generator cannot proceed without a valid style manager.
	/// Ensure that a style manager is provided when initializing the PDF generator to avoid this exception.</remarks>
	public class MissingStyleManagerException : PdfDocumentsException
	{
		/// <summary>
		/// Initializes a new instance of the MissingStyleManagerException class with a default error message indicating that
		/// a style manager is required to generate PDF documents.
		/// </summary>
		/// <remarks>This exception is typically thrown when attempting to generate a PDF document without providing a
		/// valid style manager. Ensure that a style manager is supplied during PDF generator initialization to avoid this
		/// exception.</remarks>
		public MissingStyleManagerException()
			: base("Style manager not set. A style manager is required to generate PDF documents. Please provide a valid style manager when initializing the PDF generator.")
		{
		}
	}
}
