using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Badgerodon.Collections
{
	public class Trie<T>
	{
		private struct Edge
		{
			public char Key;
			public TrieNode Child;
		}

		private class TrieNode
		{
			public TrieNode Parent;
			public bool HasValue;
			public T Value;
			public LinkedList<Edge> Children;

			public TrieNode()
			{
				Parent = null;
				HasValue = false;
				Children = new LinkedList<Edge>();
			}

			public bool Add(string key, int position, T value)
			{
				bool isNew = false;
				// No key, store it here
				if (position >= key.Length)
				{
					isNew = !HasValue;
					HasValue = true;
					Value = value;
				}
				else
				{

					char c = key[position];

					var node = Children.First;
					while (node != null)
					{
						if (node.Value.Key == c)
						{
							return node.Value.Child.Add(key, position + 1, value);
						}

						node = node.Next;
					}

					var edge = new Edge
					{
						Key = c,
						Child = new TrieNode
						{
							Parent = this
						}
					};
					isNew = edge.Child.Add(key, position + 1, value);
					Children.AddLast(edge);
				}
				return isNew;
			}

			public bool GetExact(string key, int position, out T value)
			{
				if (position >= key.Length)
				{
					if (HasValue)
					{
						value = Value;
						return true;
					}
					else
					{
						value = default(T);
						return false;
					}
				}

				char c = key[position];

				var node = Children.First;
				while (node != null)
				{
					if (node.Value.Key == c)
					{
						return node.Value.Child.GetExact(key, position + 1, out value);
					}
					node = node.Next;
				}

				value = default(T);
				return false;
			}
			public bool GetClosest(string key, int position, out T value)
			{
				if (position < key.Length)
				{
					char c = key[position];

					var node = Children.First;
					while (node != null)
					{
						if (node.Value.Key == c)
						{
							return node.Value.Child.GetClosest(key, position + 1, out value);
						}
						node = node.Next;
					}
				}

				var trie = this;
				while (trie != null)
				{
					if (trie.HasValue)
					{
						value = trie.Value;
						return true;
					}
					trie = trie.Parent;
				}
				value = default(T);
				return false;
			}
			public bool Remove(string key, int position)
			{
				if (position < key.Length)
				{
					char c = key[position];

					var node = Children.First;
					while (node != null)
					{
						if (node.Value.Key == c)
						{
							return node.Value.Child.Remove(key, position + 1);
						}
						node = node.Next;
					}

					return false;
				}
				else
				{
					// Found one
					if (HasValue)
					{
						// If we have children then just remove the value
						if (Children.Count > 0)
						{
							HasValue = false;
							Value = default(T);
						}
						// Remove this node
						else
						{
							var node = Parent;
							var child = this;
							while (node != null)
							{
								if (node.Children.Count <= 1 && !node.HasValue)
								{
									child = node;
									node = node.Parent;
								}
								else
								{
									break;
								}
							}
							if (node == null)
							{
								child.Children.Clear();
							}
							else
							{
								var edge = node.Children.Where(e => e.Child == child).FirstOrDefault();
								node.Children.Remove(edge);
							}
						}
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}

		private int _count;
		private TrieNode _root;

		public Trie()
		{
			_root = new TrieNode();
			_count = 0;
		}

		public void Add(string key, T value)
		{
			if (_root.Add(key, 0, value))
			{
				_count++;
			}
		}

		public bool GetExact(string key, out T value)
		{
			return _root.GetExact(key, 0, out value);
		}

		public bool GetClosest(string key, out T value)
		{
			return _root.GetClosest(key, 0, out value);
		}	

		public bool Remove(string key)
		{
			var removed = _root.Remove(key, 0);

			if (removed)
			{
				_count--;
			}

			return removed;
		}
		
	}
}
