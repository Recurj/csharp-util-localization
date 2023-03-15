using Microsoft.Data.Sqlite;
using RJToolsApp;
using System;
using System.Data.Common;
namespace RJDBSqlite {
    public class DBTransaction {
        private readonly SqliteConnection connection;
        private SqliteTransaction trxn = null;
        public DBTransaction(SqliteConnection conn) {
            connection = conn;
            trxn = connection.BeginTransaction();
        }
        bool Commit() {
            trxn.Commit();
            trxn = null;
            return true;
        }
        T Commit<T>(T rc) {
            trxn.Commit();
            trxn = null;
            return rc;
        }
        bool Rollback() {
            trxn?.Rollback();
            trxn = null;
            return false;
        }
        T Rollback<T>(T rc) {
            trxn?.Rollback();
            trxn = null;
            return rc;
        }

        public bool Do(Func<SqliteTransaction, bool> body) =>
            RJApplication.TryReturn(Rollback, () => (body(trxn)) ? Commit() : Rollback());
        public T Do<T>(T def, Func<SqliteTransaction, T> body) =>
           RJApplication.TryReturn<T>(() => Rollback(def), () => Commit(body(trxn)));
        public T Do<T>(Func<T> def, Func<SqliteTransaction, T> body) =>
           RJApplication.TryReturn<T>(() => Rollback(def()), () => Commit(body(trxn)));
    }
}
