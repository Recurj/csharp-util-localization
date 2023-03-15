using System.Globalization;
using System.Text;

namespace RJToolsApp.Formats.Simple {
    public class RJFormatBuilder {
        public StringBuilder Builder { get; init; }
        public bool Error { get; set; }
        int _level;
        public RJFormatBuilder(int s, int l = 0) {
            Builder = new(s);
            _level = l;
            Error = false;
        }
        public byte[] ResultBytes() => Encoding.UTF8.GetBytes(Builder.ToString());
        public string Result() => Builder.ToString();
        public RJFormatBuilder OK() {
            Builder.Append('+').Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Failure(int err) {
            Builder.Append('-').Append(RJFormat.Separators[_level]).Append(err);
            return this;
        }
        public RJFormatBuilder Enter() {
            _level++;
            return this;
        }
        public RJFormatBuilder Leave() {
            _level--;
            Builder.Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Elem(string v) {
            Builder.Append(v).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Elem(bool b) {
            Builder.Append((b) ? '1' : '0').Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Elem(double v,int digits=2) {
            string s = $"F{digits}";
            Builder.Append(v.ToString(s,
                  CultureInfo.InvariantCulture)).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Elem(long v) {
            Builder.Append(v).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder Reset() {
            Builder.Length = 0;
            Error = false;
            return this;
        }
        public RJFormatBuilder Elem(int v) {
            Builder.Append(v).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder ElemKey(string val) {
            Builder.Append('#').Append(val).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder ElemKey(long val) {
            Builder.Append('#').Append(val).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder ElemField(int r,long val) {
            Builder.Append(r).Append(':').Append(val).Append(RJFormat.Separators[_level]);
            return this;
        }
        public RJFormatBuilder ElemField(int r, string val) {
            Builder.Append(r).Append(':').Append(val).Append(RJFormat.Separators[_level]);
            return this;
        }
        //public RJFormatBuilder UrlParam(string key, string v) {
        //    UrlKey(key);
        //    UrlField(v);
        //    Builder.Append('&');
        //    return this;
        //}
        //public RJFormatBuilder UrlParam(string key, int v) {
        //    UrlKey(key);
        //    Builder.Append(v).Append('&');
        //    return this;
        //}
        //public RJFormatBuilder UrlParamLast(string key, string v) {
        //    UrlKey(key);
        //    UrlField(v);
        //    return this;
        //}
        //public RJFormatBuilder UrlParamLast(string key, int v) {
        //    UrlKey(key);
        //    Builder.Append(v);
        //    return this;
        //}
        //void UrlKey(string key) {
        //    Builder.Append(key).Append('=');
        //}
        //void UrlField(string v) {
        //    try {
        //        Builder.Append(WebUtility.UrlEncode(v));
        //        return;
        //    }
        //    catch (Exception ex) {
        //        RJApplication.Logger.Exception(ex);
        //    }
        //    Error = true;
        //}
    }
}
