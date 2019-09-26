using AppLockerLog.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AppLockerLog.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        [HttpGet]
        [Route("getuser")]
        public IHttpActionResult GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool readerRole = false;
                bool writerRole = false;
                bool consumerRole = false;

                if (HttpContext.Current.User.IsInRole(Role.incidentReaderGroup))
                {
                    readerRole = true;
                }

                if (HttpContext.Current.User.IsInRole(Role.incidentResolverGroup))
                {
                    writerRole = true;
                }
                
                if (HttpContext.Current.User.IsInRole(Role.userReaderGroup))
                {
                    consumerRole = true;
                }

                var userInfo = new
                {
                    UserName = User.Identity.Name,
                    IncidentReaderGroup = readerRole,
                    IncidentResolverGroup = writerRole,
                    UserReaderGroup = consumerRole
                };

                return Ok(userInfo);
            }

            else
            {
                return BadRequest();
            }
        }
    }
}
