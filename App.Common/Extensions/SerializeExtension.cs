using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App.Common.Extensions
{
    public static class SerializeExtension
    {
        /// <summary>
        /// Serializes the specified object to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="raw">The raw.</param>
        /// <returns></returns>
        public static string Serialize<T>(this T raw)
            where T : class
        {
            var stringBuilder = new StringBuilder();
            StringWriter outStream = new StringWriter(stringBuilder);
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(outStream, raw);
            return stringBuilder.ToString();
        }

        public static byte[] BinarySerialize<T>(this T raw)
            where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, raw);
                return memoryStream.ToArray();
            }
        }


        /// <summary>
        /// Deserializes the specified string to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawString">The raw string.</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string rawString)
            where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T account = serializer.Deserialize(new StringReader(rawString)) as T;
            return account;
        }

        public static T BinaryDeserialize<T>(this byte[] raw)
            where T : class
        {
            using (var memoryStream = new MemoryStream(raw))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }
    }
}
