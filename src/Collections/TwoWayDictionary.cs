using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Badgerodon.Collections
{
	public class TwoWayDictionaryJsonConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (typeof(TwoWayDictionary<,>).IsAssignableFrom(type))
			{
			}
			return null;
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			return new Dictionary<string, object>();
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				yield return typeof(TwoWayDictionary<,>);
			}
		}
	}

	/// <summary>
	/// A dictionary that goes in two directions
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	public class TwoWayDictionary<T1, T2> : IEnumerable<Tuple<T1, T2>>
	{
		private Dictionary<T1, T2> _d1;
		private Dictionary<T2, T1> _d2;

		public TwoWayDictionary()
		{
			_d1 = new Dictionary<T1, T2>();
			_d2 = new Dictionary<T2, T1>();
		}

		public T2 this[T1 key]
		{
			get
			{
				return _d1[key];
			}
			set
			{
				_d1[key] = value;
				_d2[value] = key;
			}
		}

		public T1 this[T2 key]
		{
			get
			{
				return _d2[key];
			}
			set
			{
				_d2[key] = value;
				_d1[value] = key;
			}
		}

		public void Add(T1 value1, T2 value2)
		{
			this[value1] = value2;
		}

		public IEnumerator<Tuple<T1, T2>> GetEnumerator()
		{
			foreach (var t in _d1.Select(kvp => Tuple.Create(kvp.Key, kvp.Value)))
			{
				yield return t;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (var t in _d1.Select(kvp => Tuple.Create(kvp.Key, kvp.Value)))
			{
				yield return t;
			}
		}
	}
}
