using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TGis.RemoteService
{
    class PathStateChangeArgs
    {
        public Path PathArg;
        public enum Reason
        {
            Add,
            Remove,
            Update,
        };
        public Reason ReasonArg;
    }
    delegate void PathStateChangeHandler(object sender, PathStateChangeArgs args);
    class Path
    {
        public int Id;
        public string Name;
        public Polygon PathPolygon;

        public Path()
        {
            Id = -1;
            Name = "未命名路径";
            PathPolygon = new Polygon(new double[] { 0, 0, 1, 1, 2, 2 });
        }
        public Path(int id, string name, double[] points)
        {
            Id = id;
            Name = name;
            PathPolygon = new Polygon(points);
        }
        public Path(int id, string name, string strPoints)
        {
            Id = id;
            Name = name;
            string[] singlePoints = strPoints.Split(',');
            if (singlePoints.Length < 6)
                throw new ApplicationException("Path Init Error");
            double[] MapPoints = new double[singlePoints.Length];
            int i = 0;
            foreach (string sp in singlePoints)
            {
                MapPoints[i++] = Convert.ToDouble(sp);
            }
            PathPolygon = new Polygon(MapPoints);
        }
        public string ConvertPointsToStr()
        {
            string r = null;
            foreach (double n in PathPolygon.Points)
            {
                if (r == null)
                    r = n.ToString();
                else
                {
                    r += ",";
                    r += n.ToString();
                }
            }
            return r;
        }
    }
    class PathMgr
    {
        private IDictionary<int, Path> dictPaths = new Dictionary<int, Path>();
        private IDbConnection connection;

        public PathMgr(IDbConnection conn)
        {
            connection = conn;
            ReloadFromDb();
        }
        public Path[] Paths
        {
            get 
            {
                Path[] r = new Path[dictPaths.Count];
                int i = 0;
                lock (this)
                {
                    foreach (Path c in dictPaths.Values)
                        r[i++] = c;
                }
                return r;
            }
        }
        public PathStateChangeHandler OnPathStateChanged = null;
        public bool TryGetPath(int id, out Path c)
        {
            lock (this)
                return TryGetPathInner(id, out c);
        }
        public bool TryGetPathInner(int id, out Path c)
        {
            return dictPaths.TryGetValue(id, out c);
        }
        public void RemovePath(Path c)
        {
            lock (this)
                RemovePathInner(c);
        }
        public void RemovePathInner(Path c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format("delete from paths where pid = {0}", c.Id);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Remove Failed");
            }
            Path cr;
            dictPaths.TryGetValue(c.Id, out cr);
            if (!dictPaths.Remove(c.Id))
                throw new ApplicationException("Remove Failed");
            DispatchStateChangeMsg(c, PathStateChangeArgs.Reason.Remove);
        }
        public void UpdatePath(Path c)
        {
            lock (this)
                UpdatePathInner(c);
        }
        public void UpdatePathInner(Path c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format("update paths set name = '{0}', data = '{1}' where pid = {2}",
                    c.Name, c.ConvertPointsToStr(), c.Id);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Update Failed");
            }
            dictPaths[c.Id] = c;
            DispatchStateChangeMsg(c, PathStateChangeArgs.Reason.Update);
        }
        public void InsertPath(Path c)
        {
            lock (this)
                InsertPathInner(c);
        }
        public void InsertPathInner(Path c)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format(@"insert into paths (name, data) values ('{0}', '{1}')",
                    c.Name, c.ConvertPointsToStr());
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException("Insert Failed");
            }
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format(@"select * from paths where name = '{0}'", c.Name);
                using (IDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        int id = rd.GetInt32(0);
                        string name = rd.GetString(1);
                        string pathData = rd.GetString(2);
                        Path newc = new Path(id, name, pathData);
                        dictPaths.Add(id, newc);
                        DispatchStateChangeMsg(newc, PathStateChangeArgs.Reason.Add);
                    }
                }
            }
        }
        public void ReloadFromDb()
        {
            lock (this)
                UpdateFromDbInner();
        }
        public void UpdateFromDbInner()
        {
            dictPaths.Clear();
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists paths 
                                    (pid INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, data TEXT)";
                cmd.ExecuteNonQuery();
            }
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from paths";
                using (IDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        int id = rd.GetInt32(0);
                        string name = rd.GetString(1);
                        string pathData = rd.GetString(2);
                        Path newc = new Path(id, name, pathData);
                        dictPaths.Add(id, newc);
                    }
                }
            }
            foreach (Path c in Paths)
                DispatchStateChangeMsg(c, PathStateChangeArgs.Reason.Add);
        }
        private void DispatchStateChangeMsg(Path c, PathStateChangeArgs.Reason reason)
        {
            if (OnPathStateChanged == null)
                return;
            PathStateChangeArgs args = new PathStateChangeArgs();
            args.PathArg = c;
            args.ReasonArg = reason;
            OnPathStateChanged(this, args);
        }
    }
}
