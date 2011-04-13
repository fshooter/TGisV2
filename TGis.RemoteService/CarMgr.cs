using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TGis.Common;
using System.Threading;

namespace TGis.RemoteService
{
    class CarStateChangeArgs
    {
        public Car CarArg;
        public enum Reason
        {
            Add,
            Remove,
            Update,
        };
        public Reason ReasonArg;
    }
    delegate void CarStateChangeHandler(object sender, CarStateChangeArgs args);
    class Car
    {
        public int Id;
        public int PathId;
        public string Name;
        public string SerialNum;
        
        public Car()
        {
            Id = -1;
            PathId = -1;
            Name = "未赋值";
            SerialNum = "";
        }
        public Car(int id, string name, string serialNum, int pathId)
        {
            Id = id;
            SerialNum = serialNum;
            PathId = pathId;
            Name = name;
        }
    }
    class CarMgr
    {
        private IDictionary<int, Car> dictCars = new Dictionary<int, Car>();
        private IDbConnection connection;

        public CarMgr(IDbConnection conn)
        {
            connection = conn;
            UpdateFromDb();
        }
        public Car[] Cars
        {
            get 
            {
                Car[] r = new Car[dictCars.Count];
                int i = 0;
                lock (this)
                {
                    foreach (Car c in dictCars.Values)
                        r[i++] = c;
                }
                return r;
            }
        }
        
        public CarStateChangeHandler OnCarStateChanged = null;
        
        public bool TryGetCar(int id, out Car c)
        {
            lock (this)
                return TryGetCarInner(id, out c);
        }
        public bool TryGetCarInner(int id, out Car c)
        {
            return dictCars.TryGetValue(id, out c);
        }
        public void RemoveCar(Car c)
        {
            lock (this)
                RemoveCarInner(c);
        }
        public void RemoveCarInner(Car c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format("delete from cars where cid = {0}", c.Id);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Remove Failed");
            }
            Car cr;
            dictCars.TryGetValue(c.Id, out cr);
            if(!dictCars.Remove(c.Id))
                throw new ApplicationException("Remove Failed");
            DispatchStateChangeMsg(c, CarStateChangeArgs.Reason.Remove);
        }
        public void UpdateCar(Car c)
        {
            lock (this)
                UpdateCarInner(c);
        }
        public void UpdateCarInner(Car c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format("update cars set name = '{0}', pathid = {1}, serial = '{2}' where cid = {3}",
                    c.Name, c.PathId, c.SerialNum, c.Id);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Update Failed");
            }
            dictCars[c.Id] = c;
            DispatchStateChangeMsg(c, CarStateChangeArgs.Reason.Update);
        }
        public void InsertCar(Car c)
        {
            lock (this)
                InsertCarInner(c);
        }
        public void InsertCarInner(Car c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format(@"insert into cars (name, serial, pathid) values ('{0}', '{1}', {2})",
                    c.Name, c.SerialNum, c.PathId);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Insert Failed");
            }
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format(@"select * from cars where name = '{0}'", c.Name);
                using (IDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        int id = rd.GetInt32(0);
                        string name = rd.GetString(1);
                        string serial = rd.GetString(2);
                        int pid = rd.GetInt32(3);
                        Car newc = new Car(id, name, serial, pid);
                        dictCars.Add(id, newc);
                        DispatchStateChangeMsg(newc, CarStateChangeArgs.Reason.Add);
                    }
                }
            }
        }
        public void UpdateFromDb()
        {
            lock (this)
                UpdateFromDbInner();
        }
        public void UpdateFromDbInner()
        {
            dictCars.Clear();
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists cars 
                                    (cid INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, serial TEXT, pathid INTEGER)";
                cmd.ExecuteNonQuery();
            }
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from cars";
                using (IDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        int id = rd.GetInt32(0);
                        string name = rd.GetString(1);
                        string seril = rd.GetString(2);
                        int pid = rd.GetInt32(3);
                        Car newc = new Car(id, name, seril, pid);
                        dictCars.Add(id, newc);
                    }
                }
            }
            foreach(Car c in Cars)
                DispatchStateChangeMsg(c, CarStateChangeArgs.Reason.Add);
        }

        public bool MapSerialNumToCarId(string serial, out int id)
        {
            var cars = this.Cars;
            foreach (var c in cars)
            {
                if (c.SerialNum == serial)
                {
                    id = c.Id;
                    return true;
                }
            }
            id = -1;
            return false;
        }
        
        private void DispatchStateChangeMsg(Car c, CarStateChangeArgs.Reason reason)
        {
            if (OnCarStateChanged == null)
                return;
            CarStateChangeArgs args = new CarStateChangeArgs();
            args.CarArg = c;
            args.ReasonArg = reason;
            OnCarStateChanged(this, args);
        }
    }
}
