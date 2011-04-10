using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TGis.RemoteContract
{
    
    [ServiceContract]
    [ServiceKnownType(typeof(GisSessionReason))]
    public interface IGisServiceAblity
    {
        [OperationContract]
        int GetVersion();

        [OperationContract]
        bool VerifyPassword(byte[] pass);

        [OperationContract]
        void ModifyPassword(byte[] newPass);
       
        [OperationContract]
        DateTime GetCurrentTime();

        [OperationContract]
        GisCarInfo[] GetCarInfo();

        [OperationContract]
        GisPathInfo[] GetPathInfo();

        [OperationContract]
        GisSessionInfo[] QuerySessionInfo(DateTime tmStart, DateTime tmEnd);

        [OperationContract]
        GisEventInfo[] QueryEventInfo(DateTime tmStart, DateTime tmEnd, out bool bTobeContinue);

        [OperationContract]
        bool AddCarInfo(GisCarInfo info);

        [OperationContract]
        bool UpdateCarInfo(GisCarInfo info);
       
        [OperationContract]
        bool RemoveCarInfo(int id);

        [OperationContract]
        bool AddPathInfo(GisPathInfo info);

        [OperationContract]
        bool UpdatePathInfo(GisPathInfo info);

        [OperationContract]
        bool RemovePathInfo(int id);
    }

    // 使用下面示例中说明的数据协定将复合类型添加到服务操作
    [DataContract]
    public class GisCarInfo
    {
        int id;
        string name;
        int pathId;


        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public int PathId
        {
            get { return pathId; }
            set { pathId = value; }
        }
    }

    [DataContract]
    public class GisPathInfo
    {
        int id;
        string name;
        double[] points;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get{return name;}
            set{name = value;}
        }

        [DataMember]
        public double[] Points
        {
            get { return points; }
            set { points = value; }
        }

    }

    [DataContract]
    public enum GisSessionReason
    {
        [EnumMember]
        Add = 0,
        [EnumMember]
        Update = 1,
        [EnumMember]
        Remove = 2,
    }

    [DataContract]
    public class GisSessionInfo
    {
        int carId;
        double x;
        double y;
        bool bRoolDirection; // true:正转
        bool bOutOfPath;
        bool bAlive;
        GisSessionReason reason;
        DateTime time;

        [DataMember]
        public int CarId
        {
            get { return carId; }
            set { carId = value; }
        }

        [DataMember]
        public double X
        {
          get { return x; }
          set { x = value; }
        }

        [DataMember]
        public double Y
        {
          get { return y; }
          set { y = value; }
        }

        [DataMember]
        public bool RoolDirection
        {
          get { return bRoolDirection; }
          set { bRoolDirection = value; }
        }

        [DataMember]
        public bool OutOfPath
        {
          get { return bOutOfPath; }
          set { bOutOfPath = value; }
        }

        [DataMember]
        public bool Alive
        {
          get { return bAlive; }
          set { bAlive = value; }
        }

        [DataMember]
        public GisSessionReason Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }

    [DataContract]
    public enum GisEventType
    {
        [EnumMember]
        Connect = 0,
        [EnumMember]
        DisConnect = 1,
        [EnumMember]
        OutOfPath = 2,
        [EnumMember]
        RollBackward = 3,
    }

    [DataContract]
    public class GisEventInfo
    {
        private GisEventType type;
        private int carId;
        private double x;
        private double y;
        private DateTime time;

        [DataMember]
        public GisEventType Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public int CarId
        {
            get { return carId; }
            set { carId = value; }
        }
        [DataMember]
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        [DataMember]
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
