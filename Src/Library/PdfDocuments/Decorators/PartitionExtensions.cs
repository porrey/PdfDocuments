namespace Enru.Yantra.Commands.CreateContainerReport
{
	/// <summary>
	/// Provides extension methods for partitioning sequences into lists of specified sizes.
	/// </summary>
	/// <remarks>These methods enable flexible partitioning strategies for enumerable collections, such as dividing
	/// items evenly or creating partitions suitable for reporting scenarios. The returned partitions are materialized as
	/// lists, and the original sequence is enumerated only once. All methods are implemented as extension methods for any
	/// enumerable type.</remarks>
	public static class PartitionExtensions
	{
		/// <summary>
		/// Partitions the elements of a sequence into lists of up to a specified maximum number of items per partition.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
		/// <param name="source">The sequence of elements to partition. Cannot be null.</param>
		/// <param name="maxItemsPerPartition">The maximum number of items allowed in each partition. Must be greater than zero.</param>
		/// <returns>An enumerable collection of lists, where each list contains up to the specified maximum number of items from the
		/// source sequence. The last list may contain fewer items if the total number of elements is not evenly divisible.</returns>
		public static IEnumerable<List<T>> PartitionEvenly<T>(this IEnumerable<T> source, int maxItemsPerPartition)
		{
			return source.Select((item, index) => new { item, index }).GroupBy(x => x.index / maxItemsPerPartition).Select(g => g.Select(x => x.item).ToList());
		}

		/// <summary>
		/// Partitions the source sequence into a series of lists for reporting, using specified sizes for the first, middle,
		/// and last partitions.
		/// </summary>
		/// <remarks>If the total number of elements is less than or equal to firstPartitionSize, a single partition
		/// containing all elements is returned. The method enumerates the source sequence immediately and buffers it in
		/// memory.</remarks>
		/// <typeparam name="T">The type of elements in the source sequence.</typeparam>
		/// <param name="source">The sequence of elements to partition. Cannot be null.</param>
		/// <param name="firstPartitionSize">The number of elements in the first partition. Must be greater than 0.</param>
		/// <param name="partitionSize">The number of elements in each middle partition. Must be greater than 0.</param>
		/// <param name="lastPartitionSize">The number of elements in the last partition. Must be greater than 0.</param>
		/// <returns>An enumerable collection of lists, where each list contains a partition of the source sequence. The first and last
		/// partitions have the specified sizes, and any remaining elements are divided into partitions of the given middle
		/// size.</returns>
		/// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if firstPartitionSize, partitionSize, or lastPartitionSize is less than or equal to 0.</exception>
		public static IEnumerable<List<T>> PartitionForReport<T>(this IEnumerable<T> source, int firstPartitionSize, int partitionSize, int lastPartitionSize)
		{
			ArgumentNullException.ThrowIfNull(source);

			if (firstPartitionSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(firstPartitionSize), "The first partition size must be greater than 0.");
			}

			if (partitionSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(partitionSize), "The partition size must be greater than 0.");
			}

			if (lastPartitionSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lastPartitionSize), "The last partition size must be greater than 0.");
			}

			List<T> items = source.ToList();
			int totalCount = items.Count;
			int index = 0;

			if (totalCount == 0)
			{
				yield break;
			}

			if (totalCount <= firstPartitionSize)
			{
				yield return items;
				yield break;
			}

			yield return items.GetRange(index, firstPartitionSize);
			index += firstPartitionSize;

			while ((totalCount - index) > partitionSize)
			{
				yield return items.GetRange(index, partitionSize);
				index += partitionSize;
			}

			if (index < totalCount)
			{
				yield return items.GetRange(index, totalCount - index);
			}
		}

		/// <summary>
		/// Partitions the source sequence into a first partition of a specified size, followed by subsequent partitions of
		/// another specified size.
		/// </summary>
		/// <remarks>This method enumerates the source sequence eagerly to determine partition boundaries. The last
		/// partition may contain fewer elements than the specified partition size if there are not enough remaining
		/// elements.</remarks>
		/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
		/// <param name="source">The sequence of elements to partition. Cannot be null.</param>
		/// <param name="firstPartitionSize">The number of elements in the first partition. Must be greater than 0.</param>
		/// <param name="partitionSize">The number of elements in each subsequent partition. Must be greater than 0.</param>
		/// <returns>An enumerable collection of lists, where the first list contains up to the specified number of elements for the
		/// first partition, and each subsequent list contains up to the specified partition size. If the source sequence is
		/// empty, the result is empty.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if firstPartitionSize or partitionSize is less than or equal to 0.</exception>
		public static IEnumerable<List<T>> PartitionForReport<T>(this IEnumerable<T> source, int firstPartitionSize, int partitionSize)
		{
			ArgumentNullException.ThrowIfNull(source);

			if (firstPartitionSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(firstPartitionSize), "The first partition size must be greater than 0.");
			}

			if (partitionSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(partitionSize), "The partition size must be greater than 0.");
			}

			List<T> items = source.ToList();
			int totalCount = items.Count;
			int index = 0;

			if (totalCount == 0)
			{
				yield break;
			}

			int firstCount = Math.Min(firstPartitionSize, totalCount);
			yield return items.GetRange(index, firstCount);
			index += firstCount;

			while (index < totalCount)
			{
				int count = Math.Min(partitionSize, totalCount - index);
				yield return items.GetRange(index, count);
				index += count;
			}
		}
	}
}
