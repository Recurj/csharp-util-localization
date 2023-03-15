using RJToolsApp;
using RJToolsApp.formats.xml;
using RJDBSqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoaderDbApp {
    public class App : RJApplication {
        private readonly DbApp _db;
        public App() {
            _db = new DbApp();
        }
        public void SetDatabase(string fn) {
            if (String.IsNullOrEmpty(fn)) {
                throw new ArgumentNullException("Database was not defined");
            }
            _db.Prepare(fn);
        }
        public void Done() {
            _db.Close();
        }
        protected static void Process<T>(List<T> elems, Action<T> func) {
            if (elems != null) foreach (var elem in elems) func(elem);
        }
        public void LoadFile(string fn) {
            Logger.InfoMsg($"Process file {fn}...");
            XMLDataLoader<XMLDataFile> xmlLoader = new XMLDataLoader<XMLDataFile>(OnXmlError);
            XMLDataFile _dataFile = xmlLoader.LoadFile(fn);
            if (_dataFile == null) throw new InvalidOperationException("Error::Could load data file " + fn);
            else {
                Process(_dataFile.Langs, OnLang);
                Process(_dataFile.Handbooks, OnHandbook);
            }
        }
        protected void OnXmlError(string err, int tag) {
            AppError(err);
        }
        protected void OnLang(XMLDataLang lang) {
            int id = _db.GetLang(lang.Label, lang.Default, lang.Iso);
            if (id <= 0) {
                AppError($"Could not add lang {lang.Label}");
            }
            else OnPhrases(_db.GetLangPhrase(id), lang.Phrases, $"lang {lang.Label}");
        }
        protected void OnHandbook(XMLDataHandbook handbook) {
            foreach (var value in handbook.Values) OnHandbookValue(handbook.Id, value);
        }
        protected void OnHandbookValue(int h, XMLDataHandbookValue val) {
            OnPhrases(_db.GetHandbookCodePhrase(h, val.Code), val.Phrases, $"handbook value {h}:{val.Code}");
        }
         protected void OnPhrases(long id, XMLDataPhrases phrases, string desc) {
            if (id < 0) AppError($"Could not find phrases for the {desc}");
            else if (phrases != null) {
                foreach (var phrase in phrases.Phrases) {
                    if (!_db.UpdateTranslate(id, phrase.Lang, DBSqlite.GetString(phrase.Text))) {
                        AppError($"Could not update phrase {id} lang {phrase.Lang} text {phrase.Text}");
                    }
                }
            }
        }
    }
}