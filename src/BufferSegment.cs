using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Badgerodon
{
	public struct BufferSegment : IEnumerable<byte>
	{
		public byte[] Buffer;
		public int Offset;
		public int Length;

		public BufferSegment(byte[] buffer)
			: this(buffer, 0, buffer.Length)
		{
		}

		public BufferSegment(byte[] buffer, int offset, int size)
		{
			Buffer = buffer;
			Offset = offset;
			Length = size;
		}

		public BufferSegment GetRange(int offset, int size)
		{
			if (offset < 0
				|| (size + offset) > Length)
			{
				throw new IndexOutOfRangeException();
			}

			return new BufferSegment(Buffer, Offset + offset, size);
		}

		public byte[] ToArray()
		{
			byte[] buffer = new byte[Length];
			System.Buffer.BlockCopy(Buffer, Offset, buffer, 0, Length);
			return buffer;
		}

		public BufferSegment Copy()
		{
			return new BufferSegment(ToArray());
		}

		public byte this[int position]
		{
			get
			{
				return Buffer[Offset + position];
			}
			set
			{
				Buffer[Offset + position] = value;
			}
		}

		public IEnumerator<byte> GetEnumerator()
		{
			for (var i = Offset; i < Length; i++)
			{
				yield return Buffer[Offset + i];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<byte>).GetEnumerator();
		}
	}
}

