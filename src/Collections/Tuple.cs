using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Badgerodon.Collections
{

	public static class Tuple
	{
		public static Tuple<T1> Create<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}
	}

	public struct Tuple<T1>
	{
		public T1 Item1;

		public Tuple(T1 item1)
		{
			Item1 = item1;
		}
	}

	public struct Tuple<T1, T2>
	{
		public T1 Item1;
		public T2 Item2;

		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}
	}
}
