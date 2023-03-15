using System.Collections.Generic;

namespace RJToolsApp.Handbooks {
    public class RJHandbook : Dictionary<int, string> {
        public string GetValue(int v) =>
            (TryGetValue(v, out var s)) ? s : "#" + v.ToString();
    }
    public class RJHandbookSet: Dictionary<int, RJHandbook> {
        public RJHandbookSet() {
        }
    }
    public class RJLangHandbookSet : Dictionary<int, RJHandbookSet> {
        public RJLangHandbookSet() {
        }
        public void AddLang(int lang) {
            this[lang] = new RJHandbookSet();
        }
    }
}
