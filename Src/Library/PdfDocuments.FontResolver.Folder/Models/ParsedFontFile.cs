namespace PdfDocuments.FontResolver.Folder
{
	public class ParsedFontFile
	{
		public ParsedFontFile(string path, string familyName, FontStyleKind style)
		{
			this.Path = path;
			this.FamilyName = familyName;
			this.Style = style;
		}

		public string Path { get; }

		public string FamilyName { get; }

		public FontStyleKind Style { get; }
	}
}