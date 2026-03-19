using System.Drawing;
using System.Drawing.Text;
using PdfSharp.Fonts;

namespace PdfDocuments.FontResolver.Windows
{
	public class FontResolver : IFontResolver
	{
		private static readonly Lazy<Dictionary<string, FontFamilyEntry>> FontFamilies = new(BuildFontFamilyMap);

		public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
		{
			if (string.IsNullOrWhiteSpace(familyName))
			{
				return null;
			}

			Dictionary<string, FontFamilyEntry> families = FontFamilies.Value;

			if (!families.TryGetValue(familyName.Trim(), out FontFamilyEntry family))
			{
				return null;
			}

			string selectedPath = null;
			bool mustSimulateBold = false;
			bool mustSimulateItalic = false;

			if (bold && italic)
			{
				selectedPath = family.BoldItalic
					?? family.Bold
					?? family.Italic
					?? family.Regular;

				mustSimulateBold = family.BoldItalic == null && family.Bold == null;
				mustSimulateItalic = family.BoldItalic == null && family.Italic == null;
			}
			else if (bold)
			{
				selectedPath = family.Bold
					?? family.Regular
					?? family.Italic
					?? family.BoldItalic;

				mustSimulateBold = family.Bold == null;
			}
			else if (italic)
			{
				selectedPath = family.Italic
					?? family.Regular
					?? family.Bold
					?? family.BoldItalic;

				mustSimulateItalic = family.Italic == null;
			}
			else
			{
				selectedPath = family.Regular
					?? family.Bold
					?? family.Italic
					?? family.BoldItalic;
			}

			if (string.IsNullOrWhiteSpace(selectedPath))
			{
				return null;
			}

			return new FontResolverInfo(selectedPath, mustSimulateBold, mustSimulateItalic);
		}

		public byte[] GetFont(string faceName)
		{
			if (string.IsNullOrWhiteSpace(faceName))
			{
				return null;
			}

			if (!File.Exists(faceName))
			{
				return null;
			}

			return File.ReadAllBytes(faceName);
		}

		private static Dictionary<string, FontFamilyEntry> BuildFontFamilyMap()
		{
			string fontsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");

			Dictionary<string, FontFamilyEntry> result = new Dictionary<string, FontFamilyEntry>(StringComparer.OrdinalIgnoreCase);

			if (!Directory.Exists(fontsFolder))
			{
				return result;
			}

			string[] fontFiles = [.. Directory
				.EnumerateFiles(fontsFolder, "*.*", SearchOption.TopDirectoryOnly)
				.Where(path =>
					path.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
					path.EndsWith(".otf", StringComparison.OrdinalIgnoreCase) ||
					path.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase))];

			InstalledFontCollection installedFonts = new();

			foreach (FontFamily family in installedFonts.Families)
			{
				string familyName = family.Name;

				FontFamilyEntry entry = new()
				{
					Regular = FindBestMatch(fontFiles, familyName, false, false),
					Bold = FindBestMatch(fontFiles, familyName, true, false),
					Italic = FindBestMatch(fontFiles, familyName, false, true),
					BoldItalic = FindBestMatch(fontFiles, familyName, true, true)
				};

				if (entry.Regular != null ||
					entry.Bold != null ||
					entry.Italic != null ||
					entry.BoldItalic != null)
				{
					result[familyName] = entry;
				}
			}

			return result;
		}

		private static string FindBestMatch(string[] fontFiles, string familyName, bool bold, bool italic)
		{
			string normalizedFamily = Normalize(familyName);

			IEnumerable<string> candidates = fontFiles.Where(path =>
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				string normalizedFileName = Normalize(fileName);

				return normalizedFileName.Contains(normalizedFamily);
			});

			candidates = candidates.OrderBy(path => Score(path, bold, italic));

			return candidates.FirstOrDefault();
		}

		private static int Score(string path, bool bold, bool italic)
		{
			string name = Normalize(Path.GetFileNameWithoutExtension(path));
			int score = 0;

			bool hasBold = name.Contains("bold") || name.EndsWith("bd");
			bool hasItalic = name.Contains("italic") || name.Contains("it") || name.Contains("oblique");
			bool hasRegular = name.Contains("regular") || name.Contains("normal");

			if (bold == hasBold)
			{
				score -= 10;
			}

			if (italic == hasItalic)
			{
				score -= 10;
			}

			if (!bold && !italic && hasRegular)
			{
				score -= 5;
			}

			if (bold && italic && hasBold && hasItalic)
			{
				score -= 5;
			}

			return score;
		}

		private static string Normalize(string value)
		{
			return new string([.. value
				.Where(char.IsLetterOrDigit)
				.Select(char.ToLowerInvariant)]);
		}

		private sealed class FontFamilyEntry
		{
			public string Regular { get; set; }
			public string Bold { get; set; }
			public string Italic { get; set; }
			public string BoldItalic { get; set; }
		}
	}
}