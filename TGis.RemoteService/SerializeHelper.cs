using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TGis.RemoteService
{
    public static class DataContractFormatSerializer
    {
        public static string SerializeToBase64String<T>(T obj, bool compress)
        {
            byte[] ret = Serialize<T>(obj, compress);
            return Convert.ToBase64String(ret);
        }



        public static byte[] Serialize<T>(T obj, bool compress)
        {
            if (obj == null)
            {
                return null;
            }
            byte[] info;
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    serializer.WriteObject(binaryDictionaryWriter, obj);
                    binaryDictionaryWriter.Flush();
                }
                info = stream.ToArray();
            }
            return info;
        }

        public static T DeserializeFromBase64String<T>(string baseString, bool decompress)
        {
            if (String.IsNullOrEmpty(baseString))
                return default(T);

            byte[] buffer = Convert.FromBase64String(baseString);
            return Deserialize<T>(buffer, buffer.Length, decompress);
        }

        public static T Deserialize<T>(byte[] info, int len, bool decompress)
        {
            T ret = default(T);
            if (info == null || info.Length <= 0)
            {
                return ret;
            }
            using (MemoryStream stream = new MemoryStream(info, 0, len))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (XmlDictionaryReader binaryDictionaryReader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    ret = (T)serializer.ReadObject(binaryDictionaryReader);
                }
            }
            return ret;
        }
    }
}
