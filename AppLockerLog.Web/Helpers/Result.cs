using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace AppLockerLog.Web.Helpers
{
    public class Result
    {
        public static object Return(HttpRequestMessage request, string routeName, object content, int totalCount, int page, int pageSize)
        {
            var urlHelper = new UrlHelper(request);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagination = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevLink = page > 0 ? urlHelper.Link("Search", new { page = page - 1, pageSize = pageSize }) : "",
                NextLink = page < totalPages - 1 ? urlHelper.Link("Search", new { page = page + 1, pageSize = pageSize }) : ""
            };

            var response = new
            {
                result = content,
                pagination = pagination
            };

            return response;
        }
    }
}