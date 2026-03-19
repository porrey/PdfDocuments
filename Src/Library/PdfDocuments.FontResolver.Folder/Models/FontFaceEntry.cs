namespace PdfDocuments.FontResolver.Folder
{
	public class FontFaceEntry
	{
		public FontFaceEntry(string faceName, string path, FontStyleKind style)
		{
			this.FaceName = faceName;
			this.Path = path;
			this.Style = style;
		}

		public string FaceName { get; }
		public string Path { get; }
		public FontStyleKind Style { get; }
	}
}