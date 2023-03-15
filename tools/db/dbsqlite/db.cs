using Microsoft.Data.Sqlite;
using RJToolsDb;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RJDBSqlite {
    public class DBSqlite {
        public SqliteConnection Connection { get; set; }

        public DBSqlite() { }

        public DbCommand Command(String sql, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction= trxn;
            return command;
        }
        public virtual bool Open(string fn) {
            try {
                SqliteConnectionStringBuilder builder = new() {
                    DataSource = fn,
                    Mode = SqliteOpenMode.ReadWriteCreate
                };
                Connection = new SqliteConnection(builder.ToString());
                Connection.Open();
                Exec("pragma synchronous=off");
                Exec("pragma journal_mode=off");
                return true;
            }
            catch { }
            return false;

        }
        public void Close() {
            Connection.Close();
        }
        protected void Exec(string sql) {
            using (var cmd = Connection.CreateCommand()) {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
        public int Run(string sql, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.Transaction = trxn;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            command.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            return command.ExecuteNonQuery();
        }
        public T RunSerial<T>(string sql, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.Transaction = trxn;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            command.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            return (T)command.ExecuteScalar();
        }
        public bool Find(string sql, Func<DbDataReader, bool> result, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.Transaction = trxn;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            command.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            var reader = command.ExecuteReader();
            var rc = reader.Read() && result(reader);
            reader.Close();
            return rc;
        }
        public T Find<T>(string sql, T def, Func<DbDataReader, T> result, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.Transaction = trxn;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            command.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            var reader = command.ExecuteReader();
            T rc = reader.Read() ? result(reader) : def;
            reader.Close();
            return rc;
        }
        public bool Select(string sql, Func<DbDataReader, bool> fetch, SqliteTransaction trxn = null) {
            var command = Connection.CreateCommand();
            command.Transaction = trxn;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            command.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            var reader = command.ExecuteReader();
            while (reader.Read()) {
                if (!fetch(reader)) {
                    reader.Close();
                    return false;
                }
            }
            reader.Close();
            return true;
        }

        public long FindLong(string sql, long def = -1, SqliteTransaction trxn = null) {
            return Find(sql, def, (reader) => reader.TypedInt64(0, def), trxn);
        }
        public int FindInt(string sql, int def = -1, SqliteTransaction trxn = null) {
            return Find(sql, def, (reader) => reader.TypedInt32(0, def), trxn);
        }

        public string FindString(string sql, string def) {
            return Find(sql, def, (reader) =>
                reader.IsDBNull(0) ? def : reader.GetString(0));
        }

        public bool Transaction(Func<SqliteTransaction, bool> body) {
            return new DBTransaction(Connection).Do(body);
        }
        public T Transaction<T>(T def, Func<SqliteTransaction, T> body) {
            return new DBTransaction(Connection).Do<T>(def, body);
        }
        public T Transaction<T>(Func<T> def, Func<SqliteTransaction, T> body) {
            return new DBTransaction(Connection).Do<T>(def, body);
        }
        static public string GetString(string s) {
            return s.Replace('\'', '\u2019');
        }
        public int GetLang(string label, SqliteTransaction trxn = null) =>
            FindInt($"select id from langs where szlabel='{label}'", -1, trxn);
        public long NewPhrase(int context, SqliteTransaction trxn = null) =>
            RunSerial<long>($"insert into phrases (lcontext) values({context}) returning id", trxn);
    }
}
