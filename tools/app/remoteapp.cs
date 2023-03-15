namespace RJToolsApp {
    public static class RemoteAppRegion {
        public static readonly int Undefined = 1;
        public static readonly int System = 2;
        public static readonly int Security = 3;
        public static readonly int Storage = 4;
        public static readonly int Format = 5;
        public static readonly int Session = 6;
        public static readonly int Property = 7;
        public static readonly int Table = 8;
        public static readonly int Access = 9;

        public static readonly int Server = 101;
        public static readonly int Client = 102;
        public static readonly int User = 103;
        public static readonly int Network = 104;
    }
    public static class RemoteAppRegionSystem {
        public static readonly int Exception = 1;
        public static readonly int AsyncError = 2;
        public static readonly int Fatal = 3;
        public static readonly int BadServer = 4;
    }
    public static class RemoteAppRegionSecurity {
        public static readonly int InstallPublic = 1;
        public static readonly int Generate = 2;
        public static readonly int ServerPublic = 3;
        public static readonly int WorkplacePrivate = 4;
        public static readonly int WorkplacePublic = 5;
    }
    public static class RemoteAppRegionStorage {
        public static readonly int NoId = 1;
        public static readonly int ReadFile = 2;
        public static readonly int SaveFile = 3;
        public static readonly int NoData = 4;
    }
    public static class RemoteAppRegionFormat {
        public static readonly int ParserHeader = 1;
        public static readonly int ParserLineBadSeparator = 2;
        public static readonly int ParserLineBadValue = 3;
        public static readonly int ParserTotal = 4;
        public static readonly int ParserStart = 5;
    }
    public static class RemoteAppRegionSession {
        public static readonly int NoController = 1;
        public static readonly int NoMethod = 2;
        public static readonly int NoData = 3;
    }
    public static class RemoteAppRegionProperty {
        public static readonly int NoId = 1;
        public static readonly int NoData = 2;
        public static readonly int UnknownId = 3;
    }
    public static class RemoteAppRegionTable {
        public static readonly int BadId = 1;
        public static readonly int BadFields = 2;
        public static readonly int ErrLoadProps = 3;
        public static readonly int BadSQL = 4;

        //public static readonly int BadView = 3;
        //public static readonly int BadSection = 5;
        //public static readonly int ConfigNameEmpty = 6;
        //public static readonly int ConfigNameExist = 7;
        //public static readonly int ConfigNameNotExist = 8;
        //public static readonly int ConfigCreate = 9;
        //public static readonly int ConfigPropertyEmpty = 10;
    }
    public static class RemoteAppRegionServer {
        public static readonly int Fatal = 1;
        public static readonly int HandbookNotFound = 2;
    }
    public static class RemoteAppRegionAccess {
        public static readonly int DeniedCatalog = 1;
        public static readonly int DeniedObject = 1;
    }
    public static class RemoteAppRegionClient {
        public static readonly int Fatal = 1;
        public static readonly int NoApp = 2;
        public static readonly int NoRequest = 3;
    };
    public static class RemoteAppRegionUser {
        public static readonly int Denied = 1;
        public static readonly int PasswordReset = 2;
        public static readonly int NoId = 3; // нет идентификатора
        public static readonly int NoPassword = 4; // нет пароля
        public static readonly int Duplicated = 5;  // уже есть пользователь с таким именем
        public static readonly int NoSentEMail = 6;
        public static readonly int CodeExpired = 7;
        public static readonly int CodeInvalid = 8;
        public static readonly int NoSaveNick = 9;
        public static readonly int Expired = 10;
        public static readonly int ApplicationNo = 21;
        public static readonly int ApplicationBlocked = 22;
    }
    public static class RemoteAppRegionNetwork {
        public static readonly int Http = 1;
        public static readonly int Connect = 2;
    }
    public class RemoteAppError {
        public static readonly int No = 0;
        public static int asyncErr() =>
            build(RemoteAppRegion.System, RemoteAppRegionSystem.AsyncError);

        public static int exeption() =>
            build(RemoteAppRegion.System, RemoteAppRegionSystem.Exception);

        public static int build(int reg, int err) => (reg << 8) + err;
    }
}
