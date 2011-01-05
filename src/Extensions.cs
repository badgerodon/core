using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Badgerodon.Collections;

namespace System
{
	public static class Extensions
	{
		public static string AsDataUri(this byte[] bytes, string type)
		{
			return "data:" + type + ";base64," + Convert.ToBase64String(bytes);
		}

		public static string ToBase36(this int x, int padTo)
		{
			return ((long)x).ToBase36(padTo);
		}
		public static string ToBase36(this long x, int padTo)
		{
			char[] clist = new char[] { '0', '1', '2', '3', '4',
										'5', '6', '7', '8', '9',
										'a', 'b', 'c', 'd', 'e',
										'f', 'g', 'h', 'i', 'j',
										'k', 'l', 'm', 'n', 'o',
										'p', 'q', 'r', 's', 't',
										'u', 'v', 'w', 'x', 'y',
										'z' };

			List<char> cs = new List<char>(); ;
			while (x != 0)
			{
				cs.Add(clist[x % 36]);
				x /= 36;
			}
			while (cs.Count < padTo)
			{
				cs.Add('0');
			}
			cs.Reverse();
			return new string(cs.ToArray());
		}

		public static T FromJson<T>(this string str)
		{
			var serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new JavaScriptConverter[] {
				new TwoWayDictionaryJsonConverter()
			});
			return serializer.Deserialize<T>(str);
		}

		public static string Hash(this string str)
		{
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
				return hash.Hex();
			}
		}

		public static string Hash(this byte[] bytes)
		{
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] hash = sha1.ComputeHash(bytes);
				return hash.Hex();
			}
		}

		public static string Hex(this byte[] bytes)
		{
			byte[] str = new byte[bytes.Length * 2];
			byte b;
			for (int i = 0; i < bytes.Length; ++i)
			{
				b = (byte)(bytes[i] >> 4);
				str[i * 2] = (byte)(b > 9 ? b + 0x37 : b + 0x30);
				b = (byte)(bytes[i] & 0xF);
				str[i * 2 + 1] = (byte)(b > 9 ? b + 0x37 : b + 0x30);
			}

			return Encoding.ASCII.GetString(str);
		}

		public static byte[] Unhex(this string str)
		{
			if (str.Length % 2 == 1)
			{
				return new byte[0];
			}

			byte[] bytes = new byte[str.Length / 2];
			byte b1, b2;
			for (int i = 0; i < str.Length; i += 2)
			{
				b1 = (byte)str[i];
				b1 = (byte)(b1 > 0x40 ? 10 + (b1 - 0x41) : b1 - 0x30);
				b2 = (byte)str[i + 1];
				b2 = (byte)(b2 > 0x40 ? 10 + (b2 - 0x41) : b2 - 0x30);
				bytes[i / 2] = (byte)((b1 << 4) | (b2));
			}
			return bytes;
		}
	}
}
