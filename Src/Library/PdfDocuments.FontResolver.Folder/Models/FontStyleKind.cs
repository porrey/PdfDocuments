namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Specifies the available font style options for text rendering.
	/// </summary>
	/// <remarks>Use this enumeration to indicate the desired style when displaying or formatting text. The values
	/// represent common typographic styles and can be used to control appearance in UI elements or document
	/// generation.</remarks>
	public enum FontStyleKind
	{
		/// <summary>
		/// Represents the regular mode or default option in the enumeration.
		/// </summary>
		Regular = 0,
		/// <summary>
		/// Specifies that text is displayed in a bold font style.
		/// </summary>
		Bold = 1,
		/// <summary>
		/// Specifies that text is displayed in an italic style.
		/// </summary>
		Italic = 2,
		/// <summary>
		/// Specifies that both bold and italic formatting are applied.
		/// </summary>
		BoldItalic = 3
	}
}