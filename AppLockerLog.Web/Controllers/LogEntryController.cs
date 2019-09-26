using AppLockerLog.Data.Models;
using AppLockerLog.Data.Repository;
using AppLockerLog.Data.Utils;
using AppLockerLog.Web.Helpers;
using AppLockerLog.Web.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace AppLockerLog.Web.Controllers
{
    [RoutePrefix("api/entry")]
    public class LogEntryController : BaseApiController
    {
        [Inject]
        public ILogEntryRepository LogEntryRepo { get; set; }

        public LogEntryController()
        {
        }

        // GET: /api/entries
        [Route(Name = "Entries")]
        [Route("~/api/entries/{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup, Role.userReaderGroup)]
        [HttpGet]
        public IHttpActionResult Get(int pageSize, int page)
        {
            try
            {
                var result = LogEntryRepo.All.Select(s => s)
                    .OrderByDescending(x => x.Id)
                    .Paginate(pageSize, page);

                int totalCount = LogEntryRepo.All.Count();
                var response =
                    Result.Return(Request, "Entries", result, totalCount, page, pageSize);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET: /api/entry/{id} || /api/entry?id={int}
        [Route(Name = "entry")]
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup)]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            LogEntry logEntry = LogEntryRepo.Find(id);
            if (logEntry == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, logEntry);
        }
        [Route(Name = "Search")]
        [Route("~/api/entries/search/{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup)]
        [HttpGet]
        public IHttpActionResult Search(int pageSize, int page, string userName)
        {
            try
            {
                var result = LogEntryRepo.Search(userName)
                    .OrderByDescending(x => x.Id)
                    .Paginate(pageSize, page);

                int totalCount = LogEntryRepo.Search(userName).Count();
                var response =
                    Result.Return(Request, "Search", result, totalCount, page, pageSize);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST: /api/entry
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                logEntry.TimeStamp = DateTime.Now;
                LogEntryRepo.InsertOrUpdate(logEntry);
                return Content(HttpStatusCode.Created, logEntry);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/entry/{id:int}
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Update(int id, [FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                LogEntryRepo.InsertOrUpdate(logEntry);
                return Content(HttpStatusCode.OK, logEntry);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/entry/{id:int}/resolved
        [Route("{id:int}/resolved")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Resolved(int id, [FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                if (userName == logEntry.EditedBy || logEntry.EditedBy == null)
                {
                    LogEntryRepo.SetEditedBy(logEntry);
                    return Content(HttpStatusCode.OK, LogEntryRepo.Find(logEntry.Id));
                }

                return BadRequest("ModelState invalid.");
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/entry/{id:int}/denied
        [Route("{id:int}/denied")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Denied(int id, [FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                if (userName == logEntry.DeniedBy || logEntry.DeniedBy == null)
                {
                    LogEntryRepo.SetDeniedBy(logEntry);
                    return Content(HttpStatusCode.OK, LogEntryRepo.Find(logEntry.Id));
                }

                return BadRequest("ModelState invalid.");
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/entry/{id:int}/note
        [Route("{id:int}/note")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Note(int id, [FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                LogEntryRepo.UpdateNote(logEntry);
                return Content(HttpStatusCode.OK, LogEntryRepo.Find(logEntry.Id));
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/entry/{id:int}/software
        [Route("{id:int}/software")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult SoftwareName(int id, [FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                LogEntryRepo.UpdateSoftware(logEntry);
                return Content(HttpStatusCode.OK, LogEntryRepo.Find(logEntry.Id));
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: /api/entry
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                LogEntryRepo.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LogEntryRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}