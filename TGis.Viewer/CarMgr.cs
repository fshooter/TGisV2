using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TGis.Common;
using System.Threading;

namespace TGis.Viewer
{
    class CarStateChangeArgs
    {
        public enum Reason
        {
            Add,
            Update,
            Remove,
            UpdateTemporary,
        };
        public Car CarArg;
        public Reason ReasonArg;
    }
    delegate void CarStateChangeHandler(object sender, CarStateChangeArgs args);
    class Car
    {
        public int Id;
        public int PathId;
        public string Name;
        public double X;
        public double Y;
        public bool Alive;
        public CarRollDirection RollDirection;
        public DateTime LastUpdateTime;
        public Car()
        {
            Id = -1;
            PathId = -1;
            Name = "未赋值";
            X = Y = 0;
            Alive = false;
            RollDirection = CarRollDirection.Forward;
            LastUpdateTime = DateTime.MinValue;
        }
        public Car(int id, string name, int pathId)
        {
            Id = id;
            PathId = pathId;
            Name = name;
            X = Y = 0;
            Alive = false;
            RollDirection = CarRollDirection.Forward;
            LastUpdateTime = DateTime.MinValue;
        }
    }
    class CarMgr
    {
        private IDictionary<int, Car> dictCars = new Dictionary<int, Car>();
        private IDbConnection connection;
        private ICarTerminalAbility terminal;

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
                        r[i] = c;
                }
                return r;
            }
        }
        public ICarTerminalAbility Terminal
        {
            get { return terminal; }
            set {
                lock (this)
                {
                    if (terminal != null)
                        terminal.OnCarStateChanged -= new CarTerminalStateChangeHandler(TerminalCarHandler);
                    terminal = value;
                    if (terminal != null)
                        terminal.OnCarStateChanged += new CarTerminalStateChangeHandler(TerminalCarHandler);
                }
            }
        }
        public CarStateChangeHandler OnCarPermanentStateChanged = null;
        public CarStateChangeHandler OnCarTemporaryStateChanged = null;
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
            if (OnCarPermanentStateChanged != null)
            {
                CarStateChangeArgs args = new CarStateChangeArgs();
                args.CarArg = cr;
                args.ReasonArg = CarStateChangeArgs.Reason.Remove;
                OnCarPermanentStateChanged(this, args);
            }
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
                cmd.CommandText = String.Format("update cars set name = '{0}', pathid = {1} where cid = {2}",
                    c.Name, c.PathId, c.Id);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Update Failed");
            }
            dictCars[c.Id] = c;
            if (OnCarPermanentStateChanged != null)
            {
                CarStateChangeArgs args = new CarStateChangeArgs();
                args.CarArg = c;
                args.ReasonArg = CarStateChangeArgs.Reason.Update;
                OnCarPermanentStateChanged(this, args);
            }
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
                cmd.CommandText = String.Format(@"insert into cars (name, pathid) values ('{0}', -1)", c.Name);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Insert Failed");
            }
            UpdateFromDb();
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
                                    (cid INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, pathid INTEGER)";
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
                        int pid = rd.GetInt32(2);
                        Car newc = new Car(id, name, pid);
                        dictCars.Add(id, newc);
                    }
                }
            }
            if (OnCarPermanentStateChanged != null)
            {
                var cars = this.Cars;
                CarStateChangeArgs args = new CarStateChangeArgs();
                foreach(Car c in cars)
                {
                    args.CarArg = c;
                    args.ReasonArg = CarStateChangeArgs.Reason.Update;
                    OnCarPermanentStateChanged(this, args);
                }
            }
        }

        private bool MapPhoneToCarIdInner(string phone, out int id)
        {
            id = 1;
            return true;
        }
        private CarProcResult TerminalCarHandler(object sender, CarStateArg arg)
        {
            lock (this)
                return TerminalCarHandlerInner(sender, arg);
        }
        private CarProcResult TerminalCarHandlerInner(object sender, CarStateArg arg)
        {
            Car c;
            int cid;
            if (!MapPhoneToCarIdInner(arg.PhoneNum, out cid))
                return CarProcResult.Miss;
            if (!TryGetCarInner(cid, out c))
                return CarProcResult.Miss;
            c.X = arg.X;
            c.Y = arg.Y;
            c.RollDirection = arg.RollDirection;
            c.LastUpdateTime = arg.Time;
            CarStateChangeArgs newargs = new CarStateChangeArgs();
            newargs.CarArg = c;
            newargs.ReasonArg = CarStateChangeArgs.Reason.UpdateTemporary;
            DispatchStateChangeMsg(false, newargs);
            return CarProcResult.Ok;
            
        }
        private void DispatchStateChangeMsg(bool bPermanent, CarStateChangeArgs args)
        {
            CarStateChangeHandler handler;
            if (bPermanent)
                handler = OnCarPermanentStateChanged;
            else
                handler = OnCarTemporaryStateChanged;
            if (handler != null)
                handler(this, args);
        }
    }
}
