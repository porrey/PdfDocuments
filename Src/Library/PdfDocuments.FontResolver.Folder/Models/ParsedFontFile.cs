namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Represents a parsed font file, including its file path, font family name, and style information.
	/// </summary>
	/// <remarks>Use this class to access metadata about a font file after it has been parsed. The properties
	/// provide read-only access to the file's location, family name, and style, which can be used for font selection or
	/// rendering operations.</remarks>
	public class ParsedFontFile
	{
		/// <summary>
		/// Initializes a new instance of the ParsedFontFile class with the specified font file path, family name, and style.
		/// </summary>
		/// <param name="path">The file system path to the font file. Cannot be null or empty.</param>
		/// <param name="familyName">The name of the font family represented by the font file. Cannot be null or empty.</param>
		/// <param name="style">The style of the font, such as regular, bold, or italic.</param>
		public ParsedFontFile(string path, string familyName, FontStyleKind style)
		{
			this.Path = path;
			this.FamilyName = familyName;
			this.Style = style;
		}

		/// <summary>
		/// Gets the file system path associated with the current instance.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Gets the family name associated with the entity.
		/// </summary>
		public string FamilyName { get; }

		/// <summary>
		/// Gets the font style applied to the text.
		/// </summary>
		public FontStyleKind Style { get; }
	}
}