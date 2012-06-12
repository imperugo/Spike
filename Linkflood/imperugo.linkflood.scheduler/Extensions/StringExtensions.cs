using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace System
{
	/// <summary>
	/// 	Adds behaviors to the <c>String</c> class.
	/// </summary>
	public static class StringExtensions
	{
		///<summary>
		///	Convert a serialized string to a concrete class
		///</summary>
		///<param name = "xml"></param>
		///<typeparam name = "T"></typeparam>
		///<returns></returns>
		public static T Deserialize<T>(this string xml)
		{
			XmlSerializer s = new XmlSerializer(typeof(T));
			using (TextReader r = new StringReader(xml))
			{
				return (T)s.Deserialize(r);
			}
		}
	}
}
