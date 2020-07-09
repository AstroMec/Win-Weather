using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace API_Helper
{
    /// <summary>
    /// This will allow me to cache the weather and location.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Serilizer<T> where T : class
    {
        public static void Serilize(T obj, string path)
        {
            using (Stream xmlFile = new FileStream(path, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(xmlFile, obj);
            }
        }

        public static T Deserialize(string path)
        {
            T result;

            using (Stream xmlFile = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                result = serializer.Deserialize(xmlFile) as T;
            }

            return result;
        }
    }
}
