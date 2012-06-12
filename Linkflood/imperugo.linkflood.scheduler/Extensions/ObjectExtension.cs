using System;
using System.IO;
using System.Xml.Serialization;

namespace System{
	/// <summary>
	/// 	Adds behaviors to the <c>Object</c> class.
	/// </summary>
	public static class ObjectExtension{
		/// <summary>
		/// 	Serializes the specified obj.
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <returns> </returns>
		public static string Serialize(this object obj){
			if (obj == null){
				throw new ArgumentNullException();
			}

			XmlSerializer s = new XmlSerializer(obj.GetType());
			using (TextWriter w = new StringWriter()){
				s.Serialize(w, obj);
				return w.ToString();
			}
		}
	}
}