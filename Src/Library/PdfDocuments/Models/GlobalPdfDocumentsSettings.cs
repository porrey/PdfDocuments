namespace PdfDocuments
{
	/// <summary>
	/// Provides global settings for PDF document generation.
	/// </summary>
	/// <remarks>Use this class to configure default options that apply to all PDF documents created by the
	/// application. Settings defined here affect document appearance and behavior unless overridden by specific
	/// document-level settings.</remarks>
	public static class GlobalPdfDocumentsSettings
	{
		/// <summary>
		/// Gets or sets the default font name used for text rendering.
		/// </summary>
		public static string DefaultFontName { get; set; } = "Arial";
	}
}
