namespace PdfDocuments.Exceptions
{	
	/// <summary>
	/// Represents errors that occur when a layout manager for a specified layout mode is not available.
	/// </summary>
	/// <remarks>This exception is typically thrown by layout management components when an unsupported or
	/// unrecognized layout mode is requested. It indicates that the operation cannot proceed due to the absence of a
	/// suitable layout manager for the given mode.</remarks>
	public class LayoutManagerException : PdfDocumentsException
	{
		/// <summary>
		/// Initializes a new instance of the LayoutManagerException class for the specified section layout mode.
		/// </summary>
		/// <param name="sectionLayoutMode">The section layout mode for which a layout manager is not available.</param>
		public LayoutManagerException(PdfSectionsLayoutMode sectionLayoutMode)
			: base($"A layout manager for layout mode '{Enum.GetName(typeof(PdfSectionsLayoutMode), sectionLayoutMode)}' is not available.")
		{
		}
	}
}
