using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppLockerLog.Web.Helpers
{
    public static class Role
    {
        public const string incidentResolverGroup = "TORG-ODK-W10-USER-APPLOCKERLOG-Write";
        public const string incidentReaderGroup = "TORG-ODK-W10-USER-APPLOCKERLOG-Read";
        public const string userReaderGroup = "TORG-ODK-W10-USER-APPLOCKERLOG-CONSUMER-Read";
    }
}