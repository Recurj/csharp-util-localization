using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RJToolsDb {
    public class RJDBStatement {
        public DbCommand Command { get; set; }
        public RJDBStatement(DbConnection db,string sql) {
            Command = db.CreateCommand();
            Command.CommandText = sql;  
        }
        protected void ParameterBool(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.Boolean;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
        protected void ParameterDatetime(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.DateTime;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
        protected void ParameterDouble(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.Double;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
        protected void ParameterInt(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
        protected void ParameterInt64(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.Int64;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
        protected void ParameterUInt64(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.UInt64;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }

        protected void ParameterStr(string name) {
            var param = Command.CreateParameter();
            param.ParameterName = name;
            param.DbType = DbType.String;
            param.Direction = ParameterDirection.Input;
            Command.Parameters.Add(param);
        }
    }
}
