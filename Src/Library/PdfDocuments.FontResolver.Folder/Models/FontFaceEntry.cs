namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Represents a font face entry, including its name, file path, and style information.
	/// </summary>
	/// <remarks>Use this class to describe a specific font face for font management or rendering operations. Each
	/// instance provides the identifying name, the location of the font file, and the associated style.</remarks>
	public class FontFaceEntry
	{
		/// <summary>
		/// Initializes a new instance of the FontFaceEntry class with the specified font face name, file path, and style.
		/// </summary>
		/// <param name="faceName">The name of the font face to associate with this entry. Cannot be null or empty.</param>
		/// <param name="path">The file system path to the font file. Must be a valid, accessible path.</param>
		/// <param name="style">The style of the font face, such as regular, bold, or italic.</param>
		public FontFaceEntry(string faceName, string path, FontStyleKind style)
		{
			this.FaceName = faceName;
			this.Path = path;
			this.Style = style;
		}

		/// <summary>
		/// Gets the name of the font face used by the object.
		/// </summary>
		public string FaceName { get; }

		/// <summary>
		/// Gets the file system path associated with the current instance.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Gets the font style applied to the text element.
		/// </summary>
		public FontStyleKind Style { get; }
	}
}