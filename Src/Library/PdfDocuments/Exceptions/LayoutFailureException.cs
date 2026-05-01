namespace PdfDocuments.Exceptions
{	
	/// <summary>
	/// Represents an exception that is thrown when the layout of child sections fails during PDF document generation.
	/// </summary>
	/// <remarks>This exception indicates a failure in arranging or rendering child sections within a parent
	/// section. It is typically thrown by layout or rendering components when a section cannot be properly composed. Catch
	/// this exception to handle layout-specific errors during PDF generation.</remarks>
	public class LayoutFailureException : PdfDocumentsException
	{
		/// <summary>
		/// Initializes a new instance of the LayoutFailureException class with a specified section key indicating which
		/// section's child layout failed.
		/// </summary>
		/// <param name="sectionKey">The key of the section for which the layout of child sections failed. Cannot be null.</param>
		public LayoutFailureException(string sectionKey)
			: base($"Layout of child sections failed for section '{sectionKey}'.")
		{
		}
	}
}
