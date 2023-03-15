using RJToolsApp.Handbooks;
using System;
using System.Data.Common;
using System.Globalization;

namespace RJToolsDb {

    public static class DBDataReaderTools {
        static public string TypedString(this DbDataReader reader, int f) =>
            reader.IsDBNull(f) ? String.Empty : reader.GetString(f);
        static public string TypedDateTime(this DbDataReader reader, int f) =>
            reader.IsDBNull(f) ? String.Empty : reader.GetDateTime(f).ToString(CultureInfo.CurrentUICulture);
        static public long TypedInt64(this DbDataReader reader, int f, long n=0) =>
            reader.IsDBNull(f) ? n : reader.GetInt64(f);
        static public int TypedInt32(this DbDataReader reader, int f, int n = 0) =>
            reader.IsDBNull(f) ? n : reader.GetInt32(f);
        static public double TypedDouble(this DbDataReader reader, int f, double d = 0) =>
            reader.IsDBNull(f) ? d : reader.GetDouble(f);
        static public bool TypedBool(this DbDataReader reader, int f,bool b=false) =>
            reader.IsDBNull(f) ? b : reader.GetBoolean(f);
        static public string Handbook(this DbDataReader reader, RJHandbook h, int f) {
            return h.GetValue(TypedInt32(reader, f));
        }
        static public long Int64NotNullable(this DbDataReader reader, int f) => reader.GetInt64(f);
        static public int Int32NotNullable(this DbDataReader reader, int f) => reader.GetInt32(f);
        static public string Number(this DbDataReader reader, int f) {
            var r = reader.TypedInt64(f);
            return r.ToString();
        }
    }
}
