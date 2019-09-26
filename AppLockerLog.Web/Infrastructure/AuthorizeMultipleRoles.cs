using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AppLockerLog.Web.Infrastructure
{
    public class AuthorizeMultipleRoles : AuthorizeAttribute
    {
        public AuthorizeMultipleRoles(params string[] roles) : base ()
        {
            Roles = string.Join(",", roles);
        }
    }
}