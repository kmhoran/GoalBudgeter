using Goal.Models.Domain.Reports;
using Goal.Services;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Goal.Controllers.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/reports")]
    public class ReportApiController: ApiController
    {
        [Route("categorymonthlyspending"), HttpGet]
        public HttpResponseMessage GetMonthlySpendingByCategory()
        {
            string userId = UserService.GetCurrentUserId();

            List<MonthlySpendingByCategoryDomain> reportList = ReportService.GetMonthlySpendingByCategory(userId);

            var response = new ItemsResponse<MonthlySpendingByCategoryDomain> { Items = reportList };

            return Request.CreateResponse(HttpStatusCode.OK, response); 
        }
    }
}