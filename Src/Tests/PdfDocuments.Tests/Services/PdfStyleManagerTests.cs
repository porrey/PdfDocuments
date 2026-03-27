/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */
using Xunit;

namespace PdfDocuments.Tests.Services
{
	[Collection("FontTests")]
	public class PdfStyleManagerTests(FontFixture _)
	{
		[Fact]
		public void DefaultConstructor_AddsTwoDefaultStyles()
		{
			PdfStyleManager<NullModel> manager = new();

			Assert.True(manager.ContainsKey(PdfStyleManager<NullModel>.Default));
			Assert.True(manager.ContainsKey(PdfStyleManager<NullModel>.Debug));
			Assert.Equal(2, manager.Count);
		}

		[Fact]
		public void ParameterizedConstructor_AddsTwoStyles()
		{
			PdfStyleManager<NullModel> manager = new("Arial", 10);

			Assert.True(manager.ContainsKey(PdfStyleManager<NullModel>.Default));
			Assert.True(manager.ContainsKey(PdfStyleManager<NullModel>.Debug));
		}

		[Fact]
		public void GetStyle_ExistingKey_ReturnsStyle()
		{
			PdfStyleManager<NullModel> manager = new();
			PdfStyle<NullModel> customStyle = new();
			manager.Add("Custom", customStyle);

			PdfStyle<NullModel> result = manager.GetStyle("Custom");

			Assert.Same(customStyle, result);
		}

		[Fact]
		public void GetStyle_NonExistingKey_ReturnsDefaultStyle()
		{
			PdfStyleManager<NullModel> manager = new();
			PdfStyle<NullModel> defaultStyle = manager[PdfStyleManager<NullModel>.Default];

			PdfStyle<NullModel> result = manager.GetStyle("NonExistent");

			Assert.Same(defaultStyle, result);
		}

		[Fact]
		public void GetStyle_DefaultKey_ReturnsDefaultStyle()
		{
			PdfStyleManager<NullModel> manager = new();

			PdfStyle<NullModel> result = manager.GetStyle(PdfStyleManager<NullModel>.Default);

			Assert.NotNull(result);
		}

		[Fact]
		public void GetStyle_DebugKey_ReturnsDebugStyle()
		{
			PdfStyleManager<NullModel> manager = new();

			PdfStyle<NullModel> result = manager.GetStyle(PdfStyleManager<NullModel>.Debug);

			Assert.NotNull(result);
		}

		[Fact]
		public void Replace_ExistingKey_ReplacesStyle()
		{
			PdfStyleManager<NullModel> manager = new();
			PdfStyle<NullModel> newDefaultStyle = new();

			manager.Replace(PdfStyleManager<NullModel>.Default, newDefaultStyle);

			Assert.Same(newDefaultStyle, manager[PdfStyleManager<NullModel>.Default]);
			Assert.Equal(2, manager.Count);
		}

		[Fact]
		public void Replace_NonExistingKey_AddsNewStyle()
		{
			PdfStyleManager<NullModel> manager = new();
			PdfStyle<NullModel> newStyle = new();
			int countBefore = manager.Count;

			manager.Replace("NewStyle", newStyle);

			Assert.True(manager.ContainsKey("NewStyle"));
			Assert.Equal(countBefore + 1, manager.Count);
		}

		[Fact]
		public void DefaultConstantValue_IsDefault()
		{
			Assert.Equal("Default", PdfStyleManager<NullModel>.Default);
		}

		[Fact]
		public void DebugConstantValue_IsDebug()
		{
			Assert.Equal("Debug", PdfStyleManager<NullModel>.Debug);
		}
	}
}
