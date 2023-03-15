using System;

namespace RJToolsApp.Formats.Simple {
    public class RJFormatParser {
        public static int Reply(String s, IRJFormatReader receiver) {
            switch (s[0]) {
                case '+':
                    return RJFormatParser.Fields(s, 0, receiver, 3);
                case '-':
                    return onError(s);
            }
            return RemoteAppError.build(
                RemoteAppRegion.Format, RemoteAppRegionFormat.ParserHeader);
        }

        public static bool Lines(String s, int level, IRJFormatReader receiver) {
            return receiver.total(RJFormatParser.Fields(s, level, receiver));
        }
        public static int Fields(
            String s, int level, IRJFormatReader receiver, int start = 0) {
            if (receiver.start(level)) {
                int j, i = start, fields = 0;
                int l = s.Length;
                string sep = RJFormat.Separators[level];
                while (i < l) {
                    j = s.IndexOf(sep, i);
                    if (j < 0)
                        return RemoteAppError.build(RemoteAppRegion.Format,
                            RemoteAppRegionFormat.ParserLineBadSeparator);
                    fields++;
                    if (!receiver.onRead(fields, s[i..j]))
                        return RemoteAppError.build(RemoteAppRegion.Format,
                            RemoteAppRegionFormat.ParserLineBadValue);
                    i = j + 2;
                }
                return receiver.total(fields)
                    ? RemoteAppError.No
                    : RemoteAppError.build(
                    RemoteAppRegion.Format, RemoteAppRegionFormat.ParserTotal);
            }
            return RemoteAppError.build(
                RemoteAppRegion.Format, RemoteAppRegionFormat.ParserStart);
        }
        static int onError(String s) {
            var rc = -RemoteAppRegion.Undefined;
            if (s.Length>3) _ = int.TryParse(s[3..], out rc);
            return rc;
        }
    }
}
