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
	public class PartitionExtensionsTests
	{
		// ─── PartitionEvenly ────────────────────────────────────────────────────────

		[Fact]
		public void PartitionEvenly_EmptyList_ReturnsEmptyResult()
		{
			var items = Enumerable.Empty<int>();

			var result = items.PartitionEvenly(3).ToList();

			Assert.Empty(result);
		}

		[Fact]
		public void PartitionEvenly_ItemsFitExactlyInOnePartition_ReturnsSinglePartition()
		{
			int[] items = [1, 2, 3];

			var result = items.PartitionEvenly(3).ToList();

			Assert.Single(result);
			Assert.Equal([1, 2, 3], result[0]);
		}

		[Fact]
		public void PartitionEvenly_EvenlyDivisible_SplitsIntoEqualPartitions()
		{
			int[] items = [1, 2, 3, 4, 5, 6];

			var result = items.PartitionEvenly(2).ToList();

			Assert.Equal(3, result.Count);
			Assert.Equal([1, 2], result[0]);
			Assert.Equal([3, 4], result[1]);
			Assert.Equal([5, 6], result[2]);
		}

		[Fact]
		public void PartitionEvenly_NotEvenlyDivisible_LastPartitionHasFewerItems()
		{
			int[] items = [1, 2, 3, 4, 5];

			var result = items.PartitionEvenly(2).ToList();

			Assert.Equal(3, result.Count);
			Assert.Equal([1, 2], result[0]);
			Assert.Equal([3, 4], result[1]);
			Assert.Equal([5], result[2]);
		}

		[Fact]
		public void PartitionEvenly_MaxItemsLargerThanList_ReturnsSinglePartition()
		{
			int[] items = [1, 2, 3];

			var result = items.PartitionEvenly(10).ToList();

			Assert.Single(result);
			Assert.Equal([1, 2, 3], result[0]);
		}

		// ─── PartitionForReport (three-parameter overload) ───────────────────────────

		[Fact]
		public void PartitionForReport_ThreeParams_EmptySource_ReturnsEmpty()
		{
			var items = Enumerable.Empty<int>();

			var result = items.PartitionForReport(5, 3, 4).ToList();

			Assert.Empty(result);
		}

		[Fact]
		public void PartitionForReport_ThreeParams_FewerItemsThanFirst_ReturnsSinglePartition()
		{
			int[] items = [1, 2, 3];

			var result = items.PartitionForReport(5, 3, 4).ToList();

			Assert.Single(result);
			Assert.Equal([1, 2, 3], result[0]);
		}

		[Fact]
		public void PartitionForReport_ThreeParams_ExactlyFirstPartition_ReturnsSinglePartition()
		{
			int[] items = [1, 2, 3, 4, 5];

			var result = items.PartitionForReport(5, 3, 4).ToList();

			Assert.Single(result);
			Assert.Equal([1, 2, 3, 4, 5], result[0]);
		}

		[Fact]
		public void PartitionForReport_ThreeParams_MultipleMiddlePartitions_SplitsCorrectly()
		{
			// first=2, middle=2, last=2; items=[1..8]
			int[] items = [1, 2, 3, 4, 5, 6, 7, 8];

			var result = items.PartitionForReport(firstPartitionSize: 2, partitionSize: 2, lastPartitionSize: 2).ToList();

			// First partition: [1,2], then middle partitions of 2, then remainder
			Assert.True(result.Count >= 2);
			Assert.Equal([1, 2], result[0]);
		}

		[Fact]
		public void PartitionForReport_ThreeParams_ZeroFirstSize_ThrowsArgumentOutOfRangeException()
		{
			int[] items = [1, 2, 3];

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				items.PartitionForReport(firstPartitionSize: 0, partitionSize: 2, lastPartitionSize: 2).ToList());
		}

		[Fact]
		public void PartitionForReport_ThreeParams_ZeroMiddleSize_ThrowsArgumentOutOfRangeException()
		{
			int[] items = [1, 2, 3];

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				items.PartitionForReport(firstPartitionSize: 2, partitionSize: 0, lastPartitionSize: 2).ToList());
		}

		[Fact]
		public void PartitionForReport_ThreeParams_ZeroLastSize_ThrowsArgumentOutOfRangeException()
		{
			int[] items = [1, 2, 3];

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				items.PartitionForReport(firstPartitionSize: 2, partitionSize: 2, lastPartitionSize: 0).ToList());
		}

		[Fact]
		public void PartitionForReport_ThreeParams_NullSource_ThrowsArgumentNullException()
		{
			IEnumerable<int>? items = null;

			Assert.Throws<ArgumentNullException>(() =>
				items!.PartitionForReport(firstPartitionSize: 2, partitionSize: 2, lastPartitionSize: 2).ToList());
		}

		// ─── PartitionForReport (two-parameter overload) ─────────────────────────────

		[Fact]
		public void PartitionForReport_TwoParams_EmptySource_ReturnsEmpty()
		{
			var items = Enumerable.Empty<int>();

			var result = items.PartitionForReport(5, 3).ToList();

			Assert.Empty(result);
		}

		[Fact]
		public void PartitionForReport_TwoParams_SinglePartitionFitsAll_ReturnsSinglePartition()
		{
			int[] items = [1, 2, 3];

			var result = items.PartitionForReport(firstPartitionSize: 5, partitionSize: 3).ToList();

			Assert.Single(result);
			Assert.Equal([1, 2, 3], result[0]);
		}

		[Fact]
		public void PartitionForReport_TwoParams_MultiplePartitions_SplitsCorrectly()
		{
			int[] items = [1, 2, 3, 4, 5, 6, 7];

			var result = items.PartitionForReport(firstPartitionSize: 3, partitionSize: 2).ToList();

			// First = [1,2,3], then [4,5], then [6,7]
			Assert.Equal(3, result.Count);
			Assert.Equal([1, 2, 3], result[0]);
			Assert.Equal([4, 5], result[1]);
			Assert.Equal([6, 7], result[2]);
		}

		[Fact]
		public void PartitionForReport_TwoParams_ZeroFirstSize_ThrowsArgumentOutOfRangeException()
		{
			int[] items = [1, 2, 3];

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				items.PartitionForReport(firstPartitionSize: 0, partitionSize: 2).ToList());
		}

		[Fact]
		public void PartitionForReport_TwoParams_ZeroPartitionSize_ThrowsArgumentOutOfRangeException()
		{
			int[] items = [1, 2, 3];

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				items.PartitionForReport(firstPartitionSize: 2, partitionSize: 0).ToList());
		}

		[Fact]
		public void PartitionForReport_TwoParams_NullSource_ThrowsArgumentNullException()
		{
			IEnumerable<int>? items = null;

			Assert.Throws<ArgumentNullException>(() =>
				items!.PartitionForReport(firstPartitionSize: 2, partitionSize: 2).ToList());
		}
	}
}
