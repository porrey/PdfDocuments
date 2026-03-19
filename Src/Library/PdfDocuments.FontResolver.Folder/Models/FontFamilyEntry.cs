namespace PdfDocuments.FontResolver.Folder
{
	public class FontFamilyEntry
	{
		public FontFamilyEntry(string displayFamilyName)
		{
			this.DisplayFamilyName = displayFamilyName;
		}

		public string DisplayFamilyName { get; }
		public FontFaceEntry Regular { get; set; }
		public FontFaceEntry Bold { get; set; }
		public FontFaceEntry Italic { get; set; }
		public FontFaceEntry BoldItalic { get; set; }
	}
}