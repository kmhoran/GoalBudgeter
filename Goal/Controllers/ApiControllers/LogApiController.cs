using Goal.Models.Domain;
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
    public class LogApiController : ApiController
    {
        //--| Transactions |----------------------------------------------------------------------->

        [Route("transaction"), HttpPost]
        public HttpResponseMessage InsertTransaction(TransactionInsertRequest model)
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


        // --| Categories |------------------------------------------------------------------------> 

        [Route("categories"), HttpGet]
        public HttpResponseMessage GetUserTransactionCategories()
        {
            if (!UserService.IsLoggedIn())
            {
                HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            string userId = UserService.GetCurrentUserId();

            CategoryCollectionDomain collection = LogService.GetUserCategories(userId);

            var response = new ItemResponse<CategoryCollectionDomain> { Item = collection };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        // .........................................................................................

        [Route("categories"), HttpPost]
        public HttpResponseMessage InsertCategory(CategoryInsertRequest model)
        {
            if (!UserService.IsLoggedIn())
            {
                HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.UserId = UserService.GetCurrentUserId();
            model.ForecastType = "Fixed";
            model.FixedPrediction = 20;

            int categoryId = LogService.InsertCategory(model);

            var response = new ItemResponse<int> { Item = categoryId };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        // .........................................................................................

        [Route("categories"), HttpPut]
        public HttpResponseMessage UpdateCategories(CategoriesUpdateRequest model)
        {
            if (!UserService.IsLoggedIn())
            {
                HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = UserService.GetCurrentUserId(); 

            bool isSuccess = LogService.UpdateCategoryCollection(model, userId);

            var response = new ItemResponse<bool> { Item = isSuccess };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        // .........................................................................................

        [Route("category"), HttpDelete]
        public HttpResponseMessage DeleteCategories(int categoryId)
        {
            if (!UserService.IsLoggedIn())
            {
                HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccess = LogService.DeleteCategory(categoryId);

            var response = new ItemResponse<bool> { Item = isSuccess };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // --| Months |---------------------------------------------------------------------------->
        [Authorize]
        [Route("CurrentMonth"), HttpGet]
        public HttpResponseMessage GetCuttentMonth()
        {
            string userId = UserService.GetCurrentUserId();

            CurrentMonthDomain currentMonth = LogService.GetCurrentMonth(userId);

            var response = new ItemResponse<CurrentMonthDomain> { Item = currentMonth };

            return Request.CreateResponse(HttpStatusCode.OK, response); 
        }

        // --| User Log |-------------------------------------------------------------------------->

        [Route("initializeUserLog"), HttpPost]
        public HttpResponseMessage InsertNewUserLog(NewUserLogInsertRequest model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.UserId = UserService.GetCurrentUserId();
            model.StartDate = DateTime.Today;

            bool isSuccess = LogService.InitializeUserLog(model);

            if (isSuccess)
            {
                isSuccess = LogService.InsertDefaultCategories(model.UserId);
            }

            var response = new ItemResponse<bool> { Item = isSuccess };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        // --| Preferences |----------------------------------------------------------------------->

        [Route("preferences"), HttpPut]
        public HttpResponseMessage UpdateUserPreferences(PreferencceUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccess = LogService.UpdateUserPreferences(model);

            var response = new SuccessResponse();

            if(!isSuccess)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Unable to make update");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }
}