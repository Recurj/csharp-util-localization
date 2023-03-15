using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJToolsDb {
    public static class DBCommandTools {
        public static DbParameter ParameterDatetime(this DbCommand cmd, string name, DateTime dt) {
            DbParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = DbType.DateTime;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = dt;
            cmd.Parameters.Add(parameter);
            return parameter;
        }
        public static DbParameter ParameterDouble(this DbCommand cmd, string name, double d = 0) {
            DbParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = DbType.Double;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = d;
            cmd.Parameters.Add(parameter);
            return parameter;
        }
        public static DbParameter ParameterInt32(this DbCommand cmd, string name, Int32 d = 0) {
            DbParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = DbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = d;
            cmd.Parameters.Add(parameter);
            return parameter;
        }
        public static DbParameter ParameterInt64(this DbCommand cmd, string name, Int64 d = 0) {
            DbParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = DbType.Int64;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = d;
            cmd.Parameters.Add(parameter);
            return parameter;
        }
    }
}
