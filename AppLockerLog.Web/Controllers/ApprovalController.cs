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
    [RoutePrefix("api/approval")]
    public class ApprovalController : BaseApiController
    {
        [Inject]
        public IApprovalRepository ApprovalRepo { get; set; }

        public ApprovalController()
        {
        }

        // GET: /api/approval
        [Route(Name =  "Approvals")]
        [Route("~/api/approvals/{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup)]
        [HttpGet]
        public IHttpActionResult Get(int pageSize, int page)
        {
            var urlHelper = new UrlHelper(Request);

            var result =ApprovalRepo.All
                .Select(x => x)
                .OrderByDescending(x => x.ID)
                .Paginate(pageSize, page);

            int totalCount = ApprovalRepo.All.Count();

            var response =
                Result.Return(Request, "Approvals", result, totalCount, page, pageSize);

            return Content(HttpStatusCode.OK, response);
        }

        // GET: /api/approval/{id} || /api/approval?id={int}
        [Route(Name = "GetApproval")]
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup)]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Approval approval = ApprovalRepo.Find(id);
            if (approval == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, approval);
        }

        // GET: /api/approvals/search?pageSize=int&page=int&query=string
        [Route(Name = "ASearch")]
        [Route("~/api/approvals/search/{pageSize:int=30}/{page:int=0}")]
        [AuthorizeMultipleRoles(Role.incidentReaderGroup, Role.incidentResolverGroup)]
        [HttpGet]
        public IHttpActionResult Search(int pageSize, int page, string query)
        {
            try
            {
                var urlHelper = new UrlHelper(Request);

                var result = ApprovalRepo.Search(query)
                    .OrderByDescending(x => x.ID)
                    .Paginate(pageSize, page);

                int totalCount = ApprovalRepo.Search(query).Count();

                var response =
                    Result.Return(Request, "ASearch", result, totalCount, page, pageSize);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST: /api/approval
        [Route("")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [HttpPost]
        public IHttpActionResult Create([FromBody] Approval approval)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.Name;

                    approval.TimeStamp = DateTime.Now;
                    approval.Approver = userName;
                    ApprovalRepo.InsertOrUpdate(approval);
                    return Content(HttpStatusCode.Created, approval);
                } else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
        }

        // PUT|PATCH: /api/approval/{id:int}
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Update(int id, [FromBody] Approval approval)
        {
            if (ModelState.IsValid)
            {
                ApprovalRepo.InsertOrUpdate(approval);
                return Content(HttpStatusCode.OK, approval);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/approval/{id:int}/note
        [Route("{id:int}/note")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult Note(int id, [FromBody] Approval approval)
        {
            if (ModelState.IsValid)
            {
                ApprovalRepo.UpdateReasoning(approval);
                return Content(HttpStatusCode.OK, ApprovalRepo.Find(approval.ID));
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/approval/{id:int}/group
        [Route("{id:int}/group")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult AddGroup(int id, [FromBody] Approval approval)
        {
            if (ModelState.IsValid)
            {
                ApprovalRepo.AddGroup(approval);
                return Content(HttpStatusCode.OK, ApprovalRepo.Find(approval.ID));
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT|PATCH: /api/approval/{id:int}/computername
        [Route("{id:int}/computername")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [AcceptVerbs("PUT", "PATCH")]
        public IHttpActionResult ComputerName(int id, [FromBody] Approval approval)
        {
            if (ModelState.IsValid)
            {
                ApprovalRepo.UpdateComputerName(approval);
                return Content(HttpStatusCode.OK, ApprovalRepo.Find(approval.ID));
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: /api/approval/:id
        [Route("{id:int}")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                ApprovalRepo.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE: /api/approval/:id/group
        [Route("{id:int}/group")]
        [AuthorizeMultipleRoles(Role.incidentResolverGroup)]
        [HttpDelete]
        public IHttpActionResult DeleteGroup(int id)
        {
            try
            {
                ApprovalRepo.DeleteGroup(id);
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
                ApprovalRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
