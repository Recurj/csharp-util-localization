using Microsoft.Data.Sqlite;
using RJDBSqlite;
using RJToolsDb;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJToolsDbSqlite.Tables
{
    public class RJTableTranslate
    {
        private readonly DbCommand _cmd;
        private readonly DbParameter _phrase;
        private readonly DbParameter _lang;
        public RJTableTranslate(DBSqlite db)
        {
            _cmd = db.Command("select szvalue from translates where rphrase=@phrase and rlang=@lang");
            _phrase = _cmd.ParameterInt64("@phrase");
            _lang = _cmd.ParameterInt32("@lang");
            _cmd.Prepare();
        }
        public string Read(int lang, long phrase, SqliteTransaction trxn = null)
        {
            _lang.Value = lang;
            _phrase.Value = phrase;
            _cmd.Transaction = trxn;
            using (var reader = _cmd.ExecuteReader())
            {
                if (reader.Read()) return reader.TypedString(0);
            }
            return null;
        }
        public bool Update(DBSqlite db, long phrase, string label, string val) => db.Transaction((trxn) =>
        {
            int lang = db.GetLang(label, trxn);
            if (lang < 0) return false;
            string s = Read(lang, phrase, trxn);
            if (s == null) return db.Run($"insert into translates values({phrase},{lang},'{val}')", trxn) > 0;
            if (s == val) return true;
            return db.Run($"update translates set szvalue='{val}' where rphrase= {phrase} and rlang={lang}", trxn) > 0;
        });
    }
}
