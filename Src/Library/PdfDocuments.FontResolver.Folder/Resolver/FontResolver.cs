using PdfSharp.Fonts;

namespace PdfDocuments.FontResolver.Folder
{
	public class FontResolver : IFontResolver
	{
		private readonly Dictionary<string, FontFamilyEntry> _families;
		private readonly Dictionary<string, string> _faceNameToPath;

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

		private static bool IsItalicToken(string token)
		{
			return token == "italic" ||
				   token == "oblique";
		}

		private static bool IsRegularToken(string token)
		{
			return token == "regular" ||
				   token == "normal" ||
				   token == "roman" ||
				   token == "book";
		}

		private static string NormalizeFamilyKey(string value)
		{
			return new string(
				[.. value
					.Where(char.IsLetterOrDigit)
					.Select(char.ToLowerInvariant)]);
		}

		private static string NormalizeToken(string value)
		{
			return new string(
				[.. value
					.Where(char.IsLetterOrDigit)
					.Select(char.ToLowerInvariant)]);
		}
	}
}