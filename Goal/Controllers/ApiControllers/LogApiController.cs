using Goal.Models.Requests;
using Goal.Services;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;


namespace Goal.Controllers.ApiControllers
{
    [RoutePrefix("api/log")]
    public class LogApiController: ApiController
    {
        [Route("transaction"), HttpPost]
        public HttpResponseMessage InsertTransaction(TransactionRequestModel model)
        {
            string userId = UserService.GetCurrentUserId();

            if (!ModelState.IsValid || userId == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.UserId = userId;

            int transactionId = LogService.InsertTransaction(model);

            var response = new ItemResponse<int> { Item = transactionId };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}