/*
 *	MIT License
 *
 *	Copyright (c) 2021 Daniel Porrey
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

namespace PdfDocuments
{
	public class PdfSpacing
	{
		public PdfSpacing()
		{
		}

		public PdfSpacing(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public int Top { get; set; }
		public int Bottom { get; set; }
		public int Right { get; set; }
		public int Left { get; set; }

		public static implicit operator PdfSpacing((int Left, int Top, int Right, int Bottom) item)
		{
			return new PdfSpacing(item.Left, item.Top, item.Right, item.Bottom);
		}

		public override string ToString()
		{
			return $"{this.Left}, {this.Top}, {this.Right}, {this.Bottom}";
		}
	}
}
