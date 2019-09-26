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
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace AppLockerLog.Web.Controllers
{
    [RoutePrefix("api/software")]
    public class SoftwareController : BaseApiController
    {
        private object res;
        private int totalCount;

        [Inject]
        public ISoftwareEntryRepository SoftwareEntryRepo { get; set; }

        public SoftwareController()
        {
        }

        // GET: /api/software
        [Route(Name = "getall")]
        [Route("{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup, Role.userReaderGroup)]
        [HttpGet]
        public IHttpActionResult Get(int pageSize, int page)
        {
            try
            {
                var urlHelper = new UrlHelper(Request);
                var result = SoftwareEntryRepo.All
                    .Select(s => s)
                    .OrderByDescending(x => x.Id)
                    .Paginate(pageSize, page);

                int totalCount = SoftwareEntryRepo.All.Count();
                var response =
                    Result.Return(Request, "getall", result, totalCount, page, pageSize);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET: /api/software/search?pageSize=int&page=int&state=string&query=string
        [Route(Name = "SoftwareSearch")]
        [Route("~/api/software/search/{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup, Role.userReaderGroup)]
        [HttpGet]
        public IHttpActionResult Search(int pageSize, int page, string query, string state = null)
        {
            try
            {
                var urlHelper = new UrlHelper(Request);

                if (state != null)
                {
                    res = SoftwareEntryRepo.Search(query)
                        .Select(x => x)
                        .OrderByDescending(x => x.Id)
                        .Where(x => x.Id == x.Id)
                        .Where(x => (x.State == null || x.State == state))
                        .Paginate(pageSize, page).ToList();

                    totalCount = SoftwareEntryRepo.Search(query)
                        .Select(x => x)
                        .OrderByDescending(x => x.Id)
                        .Where(x => x.Id == x.Id)
                        .Where(x => (x.State == null || x.State == state))
                        .Paginate(pageSize, page).Count();
                }
                else
                {
                    res = SoftwareEntryRepo.Search(query)
                     .Select(x => x)
                     .OrderByDescending(x => x.Id)
                     .Where(x => x.Id == x.Id)
                     .Paginate(pageSize, page).ToList();

                    totalCount = SoftwareEntryRepo.Search(query)
                        .Select(x => x)
                        .OrderByDescending(x => x.Id)
                        .Where(x => x.Id == x.Id)
                        .Paginate(pageSize, page).Count();
                }

                var response =
                    Result.Return(Request, "SESearch", res, totalCount, page, pageSize);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST: /api/entry
        [Route("")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [HttpPost]
        public IHttpActionResult Create([FromBody] SoftwareEntry softwareEntry)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                softwareEntry.TimeStamp = DateTime.Now;
                softwareEntry.CreatedBy = userName;
                SoftwareEntryRepo.InsertOrUpdate(softwareEntry);
                return Content(HttpStatusCode.Created, softwareEntry);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/software/{id:int}
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Update(int id, [FromBody] SoftwareEntry softwareEntry)
        {
            if (ModelState.IsValid)
            {
                SoftwareEntryRepo.InsertOrUpdate(softwareEntry);
                return Content(HttpStatusCode.OK, softwareEntry);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/software/{id:int}/reasoning
        [Route("{id:int}/reasoning")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Reasoning(int id, [FromBody] SoftwareEntry softwareEntry)
        {
            if (ModelState.IsValid)
            {
                SoftwareEntryRepo.UpdateReasoning(softwareEntry);
                return Content(HttpStatusCode.OK, SoftwareEntryRepo.Find(softwareEntry.Id));
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
                SoftwareEntryRepo.Delete(id);
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
                SoftwareEntryRepo.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
