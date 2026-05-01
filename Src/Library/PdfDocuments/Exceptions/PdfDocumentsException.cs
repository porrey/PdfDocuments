namespace PdfDocuments
{
	/// <summary>
	/// Represents errors that occur during PDF document processing operations.
	/// </summary>
	/// <remarks>This is the base class for all exceptions thrown by the PDF documents library. Catch this exception
	/// to handle errors specific to PDF document operations.</remarks>
	public abstract class PdfDocumentsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the PdfDocumentsException class.
		/// </summary>
		public PdfDocumentsException() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the PdfDocumentsException class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public PdfDocumentsException(string message) 
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfDocumentsException class with a specified error message and a reference to
		/// the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public PdfDocumentsException(string message, Exception innerException) 
			: base(message, innerException) 
		{ 
		}
	}
}
