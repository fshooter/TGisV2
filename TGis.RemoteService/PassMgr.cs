using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace TGis.RemoteService
{
    class PassMgr
    {
        IDbConnection conn;
        public PassMgr(IDbConnection conn)
        {
            this.conn = conn;
            SHA1 hash = SHA1.Create();
            byte[] passadmin = hash.ComputeHash(Encoding.Default.GetBytes("TGisV2SuperPass"));
            byte[] passuser = hash.ComputeHash(Encoding.Default.GetBytes("123456"));
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "create table if not exists pass (id INTEGER PRIMARY KEY, pass BLOB)";
                cmd.ExecuteNonQuery();
            }
            try
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    SQLiteParameter paramData = new SQLiteParameter("@data");
                    paramData.Value = passadmin;
                    cmd.CommandText = "insert into pass (id, pass) values (2, @data)";
                    cmd.Parameters.Add(paramData);
                    cmd.ExecuteNonQuery();
                }
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    SQLiteParameter paramData = new SQLiteParameter("@data");
                    paramData.Value = passuser;
                    cmd.CommandText = "insert into pass (id, pass) values (1, @data)";
                    cmd.Parameters.Add(paramData);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception)
            {
            	
            }
            
        }

        public bool VerifyPass(byte[] pass)
        {
            bool br = false;
            
            using (IDbCommand cmd = conn.CreateCommand())
            {
                SQLiteParameter paramData = new SQLiteParameter("@data");
                paramData.Value = pass;
                cmd.CommandText = "select * from pass where pass = @data";
                cmd.Parameters.Add(paramData);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        br = true;
                        break;
                    }
                }
            }
            return br;
        }

        public void ModifyPass(byte[] pass)
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "create table if not exists pass (id INTEGER PRIMARY KEY, pass BLOB)";
                cmd.ExecuteNonQuery();
            }
            using (IDbCommand cmd = conn.CreateCommand())
            {
                SQLiteParameter paramData = new SQLiteParameter("@data");
                paramData.Value = pass;
                cmd.CommandText = "update pass set pass = @data where id = 1";
                cmd.Parameters.Add(paramData);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
