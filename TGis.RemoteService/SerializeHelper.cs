﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.IO.Compression;
using System.Collections;

namespace TGis.RemoteService
{
    public class MemoryStreamStack
    {
        private Stack _streams = new Stack();
        public MemoryStream GetMemoryStream()
        {
            MemoryStream stream = null;
            if (_streams.Count > 0)
            {
                lock (_streams)
                {
                    if (_streams.Count > 0)
                    {
                        stream = (MemoryStream)_streams.Pop();
                    }
                }
            }
            if (stream == null)
            {
                stream = new MemoryStream(0x800);
            }
            return stream;
        }
        public void ReleaseMemoryStream(MemoryStream stream)
        {
            if (stream == null)
            {
                return;
            }
            stream.Position = 0L;
            stream.SetLength(0L);
            lock (_streams)
            {
                _streams.Push(stream);
            }
        }
        ~MemoryStreamStack()
        {
            foreach (MemoryStream memory in _streams)
            {
                memory.Dispose();
            }
            _streams.Clear();
            _streams = null;
        }
    }
    public static class CompressHelper
    {

        private readonly static MemoryStreamStack MemoryStreamStacker = new MemoryStreamStack();

        public static string CompressBytesToBase64String(byte[] buffer)
        {

            if (buffer == null || buffer.Length == 0)
            {

                return "";

            }

            byte[] compressedData = CompressBytes(buffer);

            return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);

        }

        public static string ConvertBytesToBase64String(byte[] buffer)
        {

            if (buffer == null || buffer.Length == 0)
            {

                return "";

            }

            return System.Convert.ToBase64String(buffer, 0, buffer.Length);

        }

        public static byte[] ConvertBase64StringToBytes(string deCompressString)
        {

            if (string.IsNullOrEmpty(deCompressString))
            {

                return new byte[0];

            }

            byte[] buffer = System.Convert.FromBase64String(deCompressString);

            return buffer;

        }

        public static byte[] DeCompressBase64StringToBytes(string deCompressString)
        {

            if (string.IsNullOrEmpty(deCompressString))
            {

                return new byte[0];

            }

            byte[] buffer = System.Convert.FromBase64String(deCompressString); byte[] ret = DeCompressBytes(buffer);

            return ret;

        }

        public static byte[] CompressBytes(byte[] buffer)
        {

            if (buffer == null || buffer.Length == 0)
            {

                return buffer;

            }

            byte[] compressedData;

            MemoryStream ms = MemoryStreamStacker.GetMemoryStream();

            try
            {

                using (DeflateStream compressedzipStream = new DeflateStream(ms, CompressionMode.Compress, true))
                {

                    compressedzipStream.Write(buffer, 0, buffer.Length);

                }

                ms.SetLength(ms.Position);

                //如果得到的结果长度比原来还大,则不需要压宿,返回原来的,并带上一位标识数据是否已压宿过

                if (ms.Length >= buffer.Length)
                {

                    compressedData = new byte[buffer.Length + 1];

                    buffer.CopyTo(compressedData, 0);

                    compressedData[compressedData.Length - 1] = 0;

                }

                else
                {

                    compressedData = new byte[ms.Length + 1];

                    ms.ToArray().CopyTo(compressedData, 0);

                    compressedData[compressedData.Length - 1] = 1;

                }

                //ms.Close();

            }

            finally
            {

                MemoryStreamStacker.ReleaseMemoryStream(ms);

            }

            return compressedData;

        }

        private static MemoryStream DeCompressMemoryToMemory(MemoryStream ms)
        {

            MemoryStream data = MemoryStreamStacker.GetMemoryStream();

            using (DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress))
            {

                byte[] writeData = new byte[8192];

                // Use the ReadAllBytesFromStream to read the stream.

                while (true)
                {

                    int size = zipStream.Read(writeData, 0, writeData.Length);

                    if (size > 0)
                    {

                        data.Write(writeData, 0, size);

                    }

                    else
                    {

                        break;

                    }

                }

            }

            return data;

        }

        public static byte[] DeCompressBytes(byte[] buffer)
        {

            if (buffer == null || buffer.Length <= 0)
            {

                return buffer;

            }

            byte[] bytes = new byte[buffer.Length - 1];

            Array.Copy(buffer, bytes, bytes.Length);

            //如果最后一位是0,说明没有被压宿,那么也不需要解压速

            if (buffer[buffer.Length - 1] == 0)
            {

                return bytes;

            }

            using (MemoryStream ms = new MemoryStream(buffer))
            {

                MemoryStream stream = null;

                try
                {

                    stream = DeCompressMemoryToMemory(ms);

                    stream.SetLength(stream.Position);

                    bytes = stream.ToArray();

                }

                finally
                {

                    MemoryStreamStacker.ReleaseMemoryStream(stream);

                }

            }

            return bytes;

        }

    }
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
                if (compress)
                {
                    info = CompressHelper.CompressBytes(info);
                }
            }
            return info;
        }

        public static T DeserializeFromBase64String<T>(string baseString, bool decompress)
        {
            if (String.IsNullOrEmpty(baseString))
                return default(T);

            byte[] buffer = Convert.FromBase64String(baseString);
            return Deserialize<T>(buffer, decompress);
        }

        public static T Deserialize<T>(byte[] info, bool decompress)
        {
            T ret = default(T);
            if (info == null || info.Length <= 0)
            {
                return ret;
            }
            if (decompress)
            {
                info = CompressHelper.DeCompressBytes(info);
            }
            using (MemoryStream stream = new MemoryStream(info))
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
