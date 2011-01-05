using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Badgerodon
{
	/// <summary>
	/// Represents a buffer across multiple segments
	/// </summary>
	public class MultiBuffer : IEnumerable<byte>
	{
		private List<BufferSegment> _segments;

		/// <summary>
		/// The length of all the segments
		/// </summary>
		public int Length { get; private set; }

		/// <summary>
		/// Create a new MultiBuffer
		/// </summary>
		public MultiBuffer()
		{
			_segments = new List<BufferSegment>();
			Length = 0;
		}

		/// <summary>
		/// Get a byte at the passed position
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public byte this[int position]
		{
			get
			{
				int i = 0;
				while (i < _segments.Count && position >= _segments[i].Length)
				{
					position -= _segments[i].Length;
					i++;
				}

				if (i < _segments.Count)
				{
					return _segments[i][position];
				}
				else
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// Add a segment
		/// </summary>
		/// <param name="segment"></param>
		public void Add(BufferSegment segment)
		{
			_segments.Add(segment);
			Length += segment.Length;
		}

		/// <summary>
		/// Add a segment
		/// </summary>
		/// <param name="segment"></param>
		public void Add(byte[] segment)
		{
			_segments.Add(new BufferSegment(segment, 0, segment.Length));
			Length += segment.Length;
		}

		/// <summary>
		/// Create a new MultiBuffer
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public MultiBuffer GetRange(int offset, int size)
		{
			if (offset < 0 || (offset + size) > Length)
			{
				throw new IndexOutOfRangeException();
			}

			var multiBuffer = new MultiBuffer();

			int start = 0;
			while (offset >= _segments[start].Length)
			{
				offset -= _segments[start].Length;
				start++;
			}

			int left = size;
			while (left > 0)
			{
				var n = Math.Min(left, _segments[start].Length - offset);
				// Get this segment
				var segment = _segments[start].GetRange(offset, n);
				multiBuffer.Add(segment);

				left -= n;
				offset = 0;
				// Move to the next buffer
				start++;
			}

			return multiBuffer;
		}

		/// <summary>
		/// Convert the multi buffer into a single byte array
		/// </summary>
		/// <returns></returns>
		public byte[] ToArray()
		{
			byte[] buffer = new byte[Length];

			int offset = 0;
			foreach (var segment in _segments)
			{
				Buffer.BlockCopy(segment.Buffer, segment.Offset, buffer, offset, segment.Length);
				offset += segment.Length;
			}

			return buffer;
		}

		public IEnumerator<byte> GetEnumerator()
		{
			foreach (var segment in _segments)
			{
				foreach (var b in segment)
				{
					yield return b;
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<byte>).GetEnumerator();
		}
	}
}

