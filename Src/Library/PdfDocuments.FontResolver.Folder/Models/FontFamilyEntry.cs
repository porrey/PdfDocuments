namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Represents a font family and its associated font face entries for different styles such as regular, bold, italic,
	/// and bold italic.
	/// </summary>
	/// <remarks>Use this class to group related font faces under a common display family name. Each style property
	/// can be set to a specific font face entry, allowing for flexible font management in applications that require
	/// multiple font styles.</remarks>
	public class FontFamilyEntry
	{
		/// <summary>
		/// Initializes a new instance of the FontFamilyEntry class with the specified display family name.
		/// </summary>
		/// <param name="displayFamilyName">The name used to display the font family. Cannot be null or empty.</param>
		public FontFamilyEntry(string displayFamilyName)
		{
			this.DisplayFamilyName = displayFamilyName;
		}

		/// <summary>
		/// Gets the display-friendly family name associated with the entity.
		/// </summary>
		public string DisplayFamilyName { get; }

		/// <summary>
		/// Gets or sets the font face entry representing the regular style.
		/// </summary>
		public FontFaceEntry Regular { get; set; }

		/// <summary>
		/// Gets or sets the font face entry used for bold text.
		/// </summary>
		public FontFaceEntry Bold { get; set; }

		/// <summary>
		/// Gets or sets the font face entry representing the italic style.
		/// </summary>
		public FontFaceEntry Italic { get; set; }

		/// <summary>
		/// Gets or sets the font face entry representing the bold italic style.
		/// </summary>
		public FontFaceEntry BoldItalic { get; set; }
	}
}