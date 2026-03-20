using System.Drawing;
using System.Drawing.Text;
using PdfSharp.Fonts;

namespace PdfDocuments.FontResolver.Windows
{
	/// <summary>
	/// Provides font resolution services for mapping font family names and styles to font file paths and retrieving font
	/// data. Implements the IFontResolver interface to support custom font selection and loading.
	/// </summary>
	/// <remarks>FontResolver enables applications to locate and load font files based on family name and style
	/// attributes such as bold and italic. It searches the system's font directory and supports simulation of bold or
	/// italic styles if the exact variant is unavailable. This class is thread-safe and caches font family mappings for
	/// performance. Use FontResolver when you need to resolve font resources dynamically or support custom font loading
	/// scenarios.</remarks>
	public class FontResolver : IFontResolver
	{
		/// <summary>
		/// Provides a lazily initialized mapping of font family names to their corresponding font family entries.
		/// </summary>
		/// <remarks>The dictionary is created on first access using the BuildFontFamilyMap method. Accessing this
		/// member is thread-safe due to the use of Lazy&lt;T&gt;.</remarks>
		private static readonly Lazy<Dictionary<string, FontFamilyEntry>> FontFamilies = new(BuildFontFamilyMap);

		/// <summary>
		/// Resolves the typeface information for a specified font family and style attributes.
		/// </summary>
		/// <remarks>If the requested style is not available in the specified font family, the method may simulate
		/// bold or italic styles as needed. The returned FontResolverInfo indicates whether simulation is required.</remarks>
		/// <param name="familyName">The name of the font family to resolve. Cannot be null, empty, or whitespace.</param>
		/// <param name="bold">A value indicating whether the bold style is requested.</param>
		/// <param name="italic">A value indicating whether the italic style is requested.</param>
		/// <returns>A FontResolverInfo instance containing the resolved font path and simulation flags, or null if the family or style
		/// cannot be resolved.</returns>
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

		/// <summary>
		/// Retrieves the font file as a byte array for the specified font face name.
		/// </summary>
		/// <param name="faceName">The path to the font file to retrieve. Cannot be null, empty, or whitespace.</param>
		/// <returns>A byte array containing the contents of the font file, or null if the file does not exist or the path is invalid.</returns>
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

		/// <summary>
		/// Builds a mapping of installed font family names to their corresponding font file entries for regular, bold,
		/// italic, and bold italic styles.
		/// </summary>
		/// <remarks>The mapping includes only font families for which at least one style variant is available. The
		/// search is limited to fonts installed in the Windows Fonts folder and supports TrueType, OpenType, and TrueType
		/// Collection formats. The dictionary uses a case-insensitive string comparer for font family names.</remarks>
		/// <returns>A dictionary containing font family names as keys and their associated font file entries as values. The dictionary
		/// is empty if no fonts are found.</returns>
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

		/// <summary>
		/// Finds the font file that best matches the specified font family and style attributes from a list of available font
		/// files.
		/// </summary>
		/// <remarks>The method performs a case-insensitive search for font files whose names contain the specified
		/// family name and ranks candidates based on their similarity to the requested style attributes.</remarks>
		/// <param name="fontFiles">An array of file paths representing available font files to search.</param>
		/// <param name="familyName">The name of the font family to match against the available font files.</param>
		/// <param name="bold">A value indicating whether the desired font style is bold. Set to <see langword="true"/> for bold; otherwise, <see
		/// langword="false"/>.</param>
		/// <param name="italic">A value indicating whether the desired font style is italic. Set to <see langword="true"/> for italic; otherwise,
		/// <see langword="false"/>.</param>
		/// <returns>The file path of the font file that best matches the specified family and style attributes, or <see
		/// langword="null"/> if no suitable match is found.</returns>
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

		/// <summary>
		/// Calculates a score indicating how well a font file matches the specified bold and italic attributes.
		/// </summary>
		/// <remarks>This method analyzes the font file name to determine if it contains keywords related to bold,
		/// italic, or regular styles. The score is adjusted based on how closely the file name matches the requested
		/// attributes.</remarks>
		/// <param name="path">The file path of the font to evaluate. Must refer to a valid font file.</param>
		/// <param name="bold">A value indicating whether the font should be bold.</param>
		/// <param name="italic">A value indicating whether the font should be italic.</param>
		/// <returns>An integer score representing the degree of match between the font's name and the requested bold and italic
		/// attributes. Lower scores indicate a better match.</returns>
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

		/// <summary>
		/// Returns a normalized version of the specified string containing only lowercase letters and digits.
		/// </summary>
		/// <remarks>Normalization removes all non-letter and non-digit characters and converts all letters to
		/// lowercase using the invariant culture.</remarks>
		/// <param name="value">The string to normalize. Can be null or empty; in such cases, the result will be an empty string.</param>
		/// <returns>A string consisting of the lowercase letters and digits from the input. If the input contains no letters or
		/// digits, returns an empty string.</returns>
		private static string Normalize(string value)
		{
			return new string([.. value
				.Where(char.IsLetterOrDigit)
				.Select(char.ToLowerInvariant)]);
		}

		/// <summary>
		/// Represents a set of font family style names, including regular, bold, italic, and bold italic variants.
		/// </summary>
		private sealed class FontFamilyEntry
		{
			public string Regular { get; set; }
			public string Bold { get; set; }
			public string Italic { get; set; }
			public string BoldItalic { get; set; }
		}
	}
}