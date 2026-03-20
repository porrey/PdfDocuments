using PdfSharp.Fonts;

namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Provides font resolution services by loading font files from a specified folder and enabling lookup of font faces
	/// and families for rendering purposes.
	/// </summary>
	/// <remarks>FontResolver supports TrueType (.ttf) and OpenType (.otf) font files located in the given directory
	/// and its subdirectories. It allows resolving font faces based on family name and style (bold, italic, etc.), and
	/// retrieving font file data for use in rendering or embedding scenarios. Thread safety is not guaranteed; callers
	/// should ensure appropriate synchronization if accessing instances concurrently.</remarks>
	public class FontResolver : IFontResolver
	{
		private readonly Dictionary<string, FontFamilyEntry> _families;
		private readonly Dictionary<string, string> _faceNameToPath;

		/// <summary>
		/// Initializes a new instance of the FontResolver class using the specified font folder.
		/// </summary>
		/// <remarks>The constructor loads font files from the provided folder and prepares internal mappings for font
		/// resolution. The folder must exist and contain valid font files for correct operation.</remarks>
		/// <param name="fontFolder">The path to the folder containing font files to be loaded. Cannot be null, empty, or whitespace.</param>
		/// <exception cref="ArgumentException">Thrown if fontFolder is null, empty, or consists only of whitespace.</exception>
		/// <exception cref="DirectoryNotFoundException">Thrown if the specified font folder does not exist.</exception>
		public FontResolver(string fontFolder)
		{
			if (string.IsNullOrWhiteSpace(fontFolder))
			{
				throw new ArgumentException("The font folder cannot be null or empty.", nameof(fontFolder));
			}

			DirectoryInfo dirInfo = new(fontFolder);

			if (!dirInfo.Exists)
			{
				throw new DirectoryNotFoundException($"The font folder '{dirInfo.FullName}' was not found.");
			}

			this._families = new Dictionary<string, FontFamilyEntry>(StringComparer.OrdinalIgnoreCase);
			this._faceNameToPath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			this.LoadFonts(fontFolder);
		}

		/// <summary>
		/// Resolves a typeface for the specified font family and style attributes.
		/// </summary>
		/// <remarks>If the exact style requested is not available, the method may simulate bold or italic styles as
		/// needed. The returned FontResolverInfo indicates whether style simulation is required.</remarks>
		/// <param name="familyName">The name of the font family to resolve. Cannot be null, empty, or whitespace.</param>
		/// <param name="bold">A value indicating whether the resolved typeface should be bold.</param>
		/// <param name="italic">A value indicating whether the resolved typeface should be italic.</param>
		/// <returns>A FontResolverInfo instance representing the resolved typeface and any required style simulation; or null if the
		/// family or style cannot be resolved.</returns>
		public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
		{
			if (string.IsNullOrWhiteSpace(familyName))
			{
				return null;
			}

			string normalizedFamilyName = NormalizeFamilyKey(familyName);

			if (!this._families.TryGetValue(normalizedFamilyName, out FontFamilyEntry family))
			{
				return null;
			}

			FontFaceEntry selectedFace = null;
			bool mustSimulateBold = false;
			bool mustSimulateItalic = false;

			if (bold == true && italic == true)
			{
				selectedFace = family.BoldItalic;

				if (selectedFace == null)
				{
					selectedFace = family.Bold;

					if (selectedFace != null)
					{
						mustSimulateItalic = true;
					}
				}

				if (selectedFace == null)
				{
					selectedFace = family.Italic;

					if (selectedFace != null)
					{
						mustSimulateBold = true;
					}
				}

				if (selectedFace == null)
				{
					selectedFace = family.Regular;

					if (selectedFace != null)
					{
						mustSimulateBold = true;
						mustSimulateItalic = true;
					}
				}
			}
			else if (bold == true)
			{
				selectedFace = family.Bold;

				if (selectedFace == null)
				{
					selectedFace = family.BoldItalic;
				}

				if (selectedFace == null)
				{
					selectedFace = family.Regular;

					if (selectedFace != null)
					{
						mustSimulateBold = true;
					}
				}

				if (selectedFace == null)
				{
					selectedFace = family.Italic;

					if (selectedFace != null)
					{
						mustSimulateBold = true;
					}
				}
			}
			else if (italic == true)
			{
				selectedFace = family.Italic;

				selectedFace ??= family.BoldItalic;

				if (selectedFace == null)
				{
					selectedFace = family.Regular;

					if (selectedFace != null)
					{
						mustSimulateItalic = true;
					}
				}

				if (selectedFace == null)
				{
					selectedFace = family.Bold;

					if (selectedFace != null)
					{
						mustSimulateItalic = true;
					}
				}
			}
			else
			{
				selectedFace = family.Regular;
				selectedFace ??= family.Bold;
				selectedFace ??= family.Italic;
				selectedFace ??= family.BoldItalic;
			}

			if (selectedFace == null)
			{
				return null;
			}

			return new FontResolverInfo(selectedFace.FaceName, mustSimulateBold, mustSimulateItalic);
		}

		/// <summary>
		/// Retrieves the font file data for the specified font face name.
		/// </summary>
		/// <remarks>If the specified font face name does not exist or the corresponding file is not found, the method
		/// returns an empty array. The caller should check the returned array's length to determine if valid font data was
		/// retrieved.</remarks>
		/// <param name="faceName">The name of the font face to retrieve. Cannot be null, empty, or whitespace.</param>
		/// <returns>A byte array containing the font file data if the font face exists and the file is found; otherwise, an empty
		/// array.</returns>
		public byte[] GetFont(string faceName)
		{
			byte[] returnValue = [];

			if (!string.IsNullOrWhiteSpace(faceName))
			{
				if (this._faceNameToPath.TryGetValue(faceName, out string path))
				{
					if (File.Exists(path))
					{
						returnValue = File.ReadAllBytes(path);
					}
					else
					{
						throw new Exception($"The font path '{path}' for face name '{faceName}' was not found.");
					}
				}
				else
				{
					throw new Exception($"The font face name '{faceName}' was not found in the resolver.");
				}
			}
			else
			{
				throw new Exception("The font face name cannot be null, empty, or whitespace.");
			}

			return returnValue;
		}

		/// <summary>
		/// Loads font files from the specified directory and updates the internal font family and face collections.
		/// </summary>
		/// <remarks>This method scans the directory recursively for TrueType (.ttf) and OpenType (.otf) font files.
		/// Existing font families and faces are updated or added based on the discovered files. If multiple files for the
		/// same style are found, only the first is used.</remarks>
		/// <param name="fontFolder">The path to the directory containing font files. Must not be null or empty. All .ttf and .otf files in this
		/// directory and its subdirectories will be processed.</param>
		private void LoadFonts(string fontFolder)
		{
			string[] files = [.. Directory
				.EnumerateFiles(fontFolder, "*.*", SearchOption.AllDirectories)
				.Where(t =>
					t.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
					t.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
				.OrderBy(t => t, StringComparer.OrdinalIgnoreCase)];

			foreach (string file in files)
			{
				ParsedFontFile parsed = ParseFontFile(file);
				string familyKey = NormalizeFamilyKey(parsed.FamilyName);

				if (!this._families.TryGetValue(familyKey, out FontFamilyEntry family))
				{
					family = new FontFamilyEntry(parsed.FamilyName);
					this._families[familyKey] = family;
				}

				string faceName = BuildFaceName(family.DisplayFamilyName, parsed.Style);

				FontFaceEntry face = new FontFaceEntry(faceName, file, parsed.Style);
				this._faceNameToPath[faceName] = file;

				switch (parsed.Style)
				{
					case FontStyleKind.Regular:
						family.Regular ??= face;
						break;

					case FontStyleKind.Bold:
						family.Bold ??= face;
						break;

					case FontStyleKind.Italic:
						family.Italic ??= face;
						break;

					case FontStyleKind.BoldItalic:
						family.BoldItalic ??= face;
						break;
				}
			}
		}

		/// <summary>
		/// Builds a font face name by combining the specified font family name with a style suffix.
		/// </summary>
		/// <remarks>The returned face name follows the convention: family name plus a suffix indicating the style
		/// (e.g., "#B" for bold). If an unrecognized style is provided, the regular style suffix is used.</remarks>
		/// <param name="familyName">The name of the font family to use as the base for the face name. Cannot be null or empty.</param>
		/// <param name="style">The font style to apply. Determines the suffix appended to the family name.</param>
		/// <returns>A string representing the font face name, consisting of the family name and a style-specific suffix.</returns>
		private static string BuildFaceName(string familyName, FontStyleKind style)
		{
			return style switch
			{
				FontStyleKind.Regular => familyName + "#R",
				FontStyleKind.Bold => familyName + "#B",
				FontStyleKind.Italic => familyName + "#I",
				FontStyleKind.BoldItalic => familyName + "#BI",
				_ => familyName + "#R"
			};
		}

		/// <summary>
		/// Parses a font file path to extract the font family name and style information.
		/// </summary>
		/// <remarks>This method infers font style (such as bold or italic) and family name based on the file name
		/// conventions. Explicit 'Regular' tokens are ignored when determining style.</remarks>
		/// <param name="path">The full path to the font file to be parsed. Cannot be null or empty.</param>
		/// <returns>A ParsedFontFile instance containing the extracted font family name and style information from the specified file
		/// path.</returns>
		private static ParsedFontFile ParseFontFile(string path)
		{
			string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
			string cleaned = fileName.Replace('_', ' ').Replace('-', ' ');
			string[] rawParts = cleaned
				.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			List<string> familyParts = new List<string>();
			bool isBold = false;
			bool isItalic = false;

			foreach (string rawPart in rawParts)
			{
				string part = rawPart.Trim();
				string normalized = NormalizeToken(part);

				if (IsBoldToken(normalized))
				{
					isBold = true;
				}
				else if (IsItalicToken(normalized))
				{
					isItalic = true;
				}
				else if (IsRegularToken(normalized))
				{
					// Ignore explicit "Regular".
				}
				else
				{
					familyParts.Add(part);
				}
			}

			string familyName = familyParts.Count > 0
				? string.Join(" ", familyParts)
				: fileName;

			FontStyleKind style = GetStyle(isBold, isItalic);

			return new ParsedFontFile(path, familyName, style);
		}

		/// <summary>
		/// Determines the font style based on bold and italic flags.
		/// </summary>
		/// <param name="isBold">A value indicating whether the font should be bold. Set to <see langword="true"/> to apply bold styling;
		/// otherwise, <see langword="false"/>.</param>
		/// <param name="isItalic">A value indicating whether the font should be italic. Set to <see langword="true"/> to apply italic styling;
		/// otherwise, <see langword="false"/>.</param>
		/// <returns>A <see cref="FontStyleKind"/> value representing the combined font style. Returns <see
		/// cref="FontStyleKind.BoldItalic"/> if both bold and italic are specified; otherwise, returns <see
		/// cref="FontStyleKind.Bold"/>, <see cref="FontStyleKind.Italic"/>, or <see cref="FontStyleKind.Regular"/> as
		/// appropriate.</returns>
		private static FontStyleKind GetStyle(bool isBold, bool isItalic)
		{
			if (isBold == true && isItalic == true)
			{
				return FontStyleKind.BoldItalic;
			}

			if (isBold == true)
			{
				return FontStyleKind.Bold;
			}

			if (isItalic == true)
			{
				return FontStyleKind.Italic;
			}

			return FontStyleKind.Regular;
		}

		/// <summary>
		/// Determines whether the specified token represents a bold font weight.
		/// </summary>
		/// <remarks>The method recognizes common font weight names such as "bold", "semibold", "demibold",
		/// "extrabold", "ultrabold", "black", "heavy", and "medium" as bold weights.</remarks>
		/// <param name="token">The font weight token to evaluate. Cannot be null.</param>
		/// <returns>true if the token corresponds to a recognized bold font weight; otherwise, false.</returns>
		private static bool IsBoldToken(string token)
		{
			return token == "bold" ||
				   token == "semibold" ||
				   token == "demibold" ||
				   token == "extrabold" ||
				   token == "ultrabold" ||
				   token == "black" ||
				   token == "heavy" ||
				   token == "medium";
		}

		/// <summary>
		/// Determines whether the specified token represents an italic font style.
		/// </summary>
		/// <param name="token">The font style token to evaluate. Must not be null.</param>
		/// <returns>true if the token is "italic" or "oblique"; otherwise, false.</returns>
		private static bool IsItalicToken(string token)
		{
			return token == "italic" ||
				   token == "oblique";
		}

		/// <summary>
		/// Determines whether the specified token represents a regular font style.
		/// </summary>
		/// <remarks>The method recognizes "regular", "normal", "roman", and "book" as regular font style tokens.
		/// Token comparison is case-sensitive.</remarks>
		/// <param name="token">The token to evaluate. Must be a non-null, non-empty string representing a font style.</param>
		/// <returns>true if the token matches a regular font style; otherwise, false.</returns>
		private static bool IsRegularToken(string token)
		{
			return token == "regular" ||
				   token == "normal" ||
				   token == "roman" ||
				   token == "book";
		}

		/// <summary>
		/// Returns a normalized version of the specified family key, containing only lowercase alphanumeric characters.
		/// </summary>
		/// <remarks>Normalization removes all non-alphanumeric characters and converts all letters to lowercase. This
		/// method is useful for standardizing keys for comparison or storage.</remarks>
		/// <param name="value">The input string representing the family key to normalize. Cannot be null.</param>
		/// <returns>A string containing only the lowercase letters and digits from the input. If the input contains no alphanumeric
		/// characters, returns an empty string.</returns>
		private static string NormalizeFamilyKey(string value)
		{
			return new string(
				[.. value
					.Where(char.IsLetterOrDigit)
					.Select(char.ToLowerInvariant)]);
		}

		/// <summary>
		/// Normalizes the specified token by removing non-alphanumeric characters and converting all letters to lowercase.
		/// </summary>
		/// <remarks>Normalization is case-insensitive and excludes any characters that are not letters or digits.
		/// This method does not modify the original string.</remarks>
		/// <param name="value">The string token to normalize. Cannot be null.</param>
		/// <returns>A new string containing only the lowercase alphanumeric characters from the input token. Returns an empty string
		/// if no alphanumeric characters are present.</returns>
		private static string NormalizeToken(string value)
		{
			return new string(
				[.. value
					.Where(char.IsLetterOrDigit)
					.Select(char.ToLowerInvariant)]);
		}
	}
}