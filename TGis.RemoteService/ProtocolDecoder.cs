using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TGis.RemoteService
{
    [StructLayout(LayoutKind.Sequential)]
    struct RawProtocolStatusInfo 
    {
        public byte Head; // $
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=5)]
        public byte[] SerialNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Date;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Latitude; // 纬度
        public byte Reverse1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Longitude; // 经度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reverse2;
        public byte Status;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Other;
    }
    [StructLayout(LayoutKind.Sequential)]
    struct RawProtocolStatusInfo2
    {
        public byte Head; // $
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] SerialNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Date;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Latitude; // 纬度
        public byte Reverse1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Longitude; // 经度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reverse2;
        public byte Status;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Other;
    }
    struct DecodedProtocolStatus
    {
        public string SerialNum;
        public DateTime Time;
        public double Latitude;
        public double Longitude;
        public int Speed;
        public bool RollForward;
    }
    class ProtocolDecoder
    {
        public static bool Decode(byte[] data, ref DecodedProtocolStatus status)
        {
            if (data.Length == 46)
                return Decode46(data, ref status);
            else if (data.Length == 32)
                return Decode32(data, ref status);
            return false;
        }
        public static bool Decode46(byte[] data, ref DecodedProtocolStatus status)
        {
            RawProtocolStatusInfo? rawInfo = BytesToStruct(data, typeof(RawProtocolStatusInfo)) as RawProtocolStatusInfo?;
            if (rawInfo == null) return false;
            if (rawInfo.Value.Head != 0x24) return false;
            status.SerialNum = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}", rawInfo.Value.SerialNum[0],
                rawInfo.Value.SerialNum[1], rawInfo.Value.SerialNum[2],
                rawInfo.Value.SerialNum[3], rawInfo.Value.SerialNum[4]);
            status.Time = DateTime.Now.Date;
            status.Time = status.Time.AddHours(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[0])));
            status.Time = status.Time.AddMinutes(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[1])));
            status.Time = status.Time.AddSeconds(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[2])));
            status.Latitude = GpsPosConvertLat(rawInfo.Value.Latitude);
            status.Longitude = GpsPosConvertLon(rawInfo.Value.Longitude);
            status.RollForward = ((rawInfo.Value.Status & 0x20) != 0);
            return true;
        }
        public static bool Decode32(byte[] data, ref DecodedProtocolStatus status)
        {
            RawProtocolStatusInfo2? rawInfo = BytesToStruct(data, typeof(RawProtocolStatusInfo2)) as RawProtocolStatusInfo2?;
            if (rawInfo == null) return false;
            if (rawInfo.Value.Head != 0x24) return false;
            status.SerialNum = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}", rawInfo.Value.SerialNum[0],
                rawInfo.Value.SerialNum[1], rawInfo.Value.SerialNum[2],
                rawInfo.Value.SerialNum[3], rawInfo.Value.SerialNum[4]);
            status.Time = DateTime.Now.Date;
            status.Time = status.Time.AddHours(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[0])));
            status.Time = status.Time.AddMinutes(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[1])));
            status.Time = status.Time.AddSeconds(Convert.ToDouble(string.Format("{0:x}", rawInfo.Value.Time[2])));
            status.Latitude = GpsPosConvertLat(rawInfo.Value.Latitude);
            status.Longitude = GpsPosConvertLon(rawInfo.Value.Longitude);
            status.RollForward = ((rawInfo.Value.Status & 0x20) != 0);
            return true;
        }
        static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return null;
        }
        static double GpsPosConvertLat(byte[] arr)
        {
            string x = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", arr[0]
                , arr[1], arr[2], arr[3]);
            double d1 = Convert.ToDouble(x.Substring(0, 2));
            double d2 = Convert.ToDouble(x.Substring(2, 6));
            d2 /= 10000;
            //double d3 = Convert.ToDouble(x.Substring(4, 4));

            return d1 + d2 / 60;
        }
        static double GpsPosConvertLon(byte[] arr)
        {
            string x = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}", arr[0]
                , arr[1], arr[2], arr[3], arr[4]);
            double d1 = Convert.ToDouble(x.Substring(0, 3));
            double d2 = Convert.ToDouble(x.Substring(3, 6));
            //double d3 = Convert.ToDouble(x.Substring(5, 4));
            d2 /= 10000;
            return d1 + d2 / 60;
        }
    }
}
