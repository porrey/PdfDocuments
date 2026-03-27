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

namespace PdfDocuments.Tests.Decorators
{
	public class PdfDebugModeExtensionsTests
	{
		[Fact]
		public void HasFlag_NoneMode_ReturnsTrueForNone()
		{
			DebugMode mode = DebugMode.None;

			Assert.True(mode.HasFlag(DebugMode.None));
		}

		[Fact]
		public void HasFlag_WhenFlagIsSet_ReturnsTrue()
		{
			DebugMode mode = DebugMode.RevealGrid;

			Assert.True(mode.HasFlag(DebugMode.RevealGrid));
		}

		[Fact]
		public void HasFlag_WhenFlagIsNotSet_ReturnsFalse()
		{
			DebugMode mode = DebugMode.RevealGrid;

			Assert.False(mode.HasFlag(DebugMode.RevealLayout));
		}

		[Fact]
		public void HasFlag_CombinedMode_ReturnsTrueForSetFlags()
		{
			DebugMode mode = DebugMode.RevealGrid | DebugMode.HideDetails;

			Assert.True(mode.HasFlag(DebugMode.RevealGrid));
			Assert.True(mode.HasFlag(DebugMode.HideDetails));
		}

		[Fact]
		public void HasFlag_CombinedMode_ReturnsFalseForUnsetFlag()
		{
			DebugMode mode = DebugMode.RevealGrid | DebugMode.HideDetails;

			Assert.False(mode.HasFlag(DebugMode.RevealLayout));
		}

		[Fact]
		public void SetFlag_WithSetTrue_AddsFlag()
		{
			DebugMode mode = DebugMode.None;

			DebugMode result = mode.SetFlag(DebugMode.RevealGrid, true);

			Assert.True(result.HasFlag(DebugMode.RevealGrid));
		}

		[Fact]
		public void SetFlag_DefaultParameter_AddsFlag()
		{
			DebugMode mode = DebugMode.None;

			DebugMode result = mode.SetFlag(DebugMode.RevealLayout);

			Assert.True(result.HasFlag(DebugMode.RevealLayout));
		}

		[Fact]
		public void SetFlag_WithSetFalse_ClearsFlag()
		{
			DebugMode mode = DebugMode.RevealGrid | DebugMode.HideDetails;

			DebugMode result = mode.SetFlag(DebugMode.RevealGrid, false);

			Assert.False(result.HasFlag(DebugMode.RevealGrid));
			Assert.True(result.HasFlag(DebugMode.HideDetails));
		}

		[Fact]
		public void SetFlag_PreservesOtherFlags_WhenSettingNewFlag()
		{
			DebugMode mode = DebugMode.HideDetails;

			DebugMode result = mode.SetFlag(DebugMode.RevealGrid, true);

			Assert.True(result.HasFlag(DebugMode.RevealGrid));
			Assert.True(result.HasFlag(DebugMode.HideDetails));
		}

		[Fact]
		public void SetFlag_SetAlreadySetFlag_HasNoEffect()
		{
			DebugMode mode = DebugMode.RevealGrid;

			DebugMode result = mode.SetFlag(DebugMode.RevealGrid, true);

			Assert.True(result.HasFlag(DebugMode.RevealGrid));
		}
	}
}
