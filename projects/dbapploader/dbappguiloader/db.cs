using RJDBSqlite;
using RJToolsDbSqlite.Tables;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace LoaderDbApp
{
    public class DbApp : DBSqlite {
        private RJTableTranslate _translates;
        public DbApp() {
        }
        public bool Prepare(string fn) {
            bool bExist = File.Exists(fn);
            if (Open(fn)) {
                if (!bExist) CreateTables();
                _translates = new(this);
                return true;
            }
            return false;
        }
        public int GetLang(string label, int def, string iso) => Transaction<int>(-1, (trxn) => {
            int id = GetLang(label, trxn);
            if (id < 0) {
                long phrase = NewPhrase(0, trxn);
                if (phrase < 0) throw new InvalidOperationException($"Could not get langs's phrase {label}");
                id = (int)RunSerial<long>($"insert into langs (ldefault,rphrase,szlabel,sziso) values({def},{phrase},'{label}','{iso}') returning id", trxn);
            }
            else Run($"update langs set ldefault={def},szlabel='{label}',sziso='{iso}' where id={id}", trxn);
            return id;
        });
        public long GetLangPhrase(int id) => Transaction<long>(-1, (trxn) =>
            FindLong($"select rphrase from langs where id={id}", -1, trxn));
        public long GetHandbookCodePhrase(int h, int id) => Transaction<long>(-1, (trxn) => {
            long phrase = FindLong($"select rphrase from handbooks where id={h} and lcode={id}", -1, trxn);
            if (phrase > 0) return phrase;
            phrase = NewPhrase(1, trxn);
            if (phrase > 0 && Run($"insert into handbooks values({h},{id},{phrase})", trxn) > 0) return phrase;
            throw new InvalidOperationException($"Could not get handbook's phrase {h}:{id}");
        });
        public bool UpdateTranslate(long phrase, string label, string val) =>
            _translates.Update(this, phrase, label, val);
        public void CreateTables() {
            Exec("create table langs(id integer primary key, ldefault integer, rphrase int, szlabel text, sziso text)");
            Exec("create table phrases(id integer primary key, lcontext int)");
            Exec("create table translates(rphrase int, rlang int, szvalue text,primary key(rphrase,rlang)) without rowid");
            Exec("create table handbooks(id int,lcode int,rphrase int,primary key(id,lcode)) without rowid");
        }
    }
}

