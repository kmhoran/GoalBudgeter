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

        //[Route("transaction"), HttpPost]
        //public HttpResponseMessage InsertTransaction(TransactionInsertRequest model)
        //{
        //    string userId = UserService.GetCurrentUserId();

        //    if (!ModelState.IsValid || userId == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    model.UserId = userId;

        //    int transactionId = LogService.Data.InsertTransaction(model);

        //    var response = new ItemResponse<int> { Item = transactionId };

        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}


        // --| Categories |------------------------------------------------------------------------> 

        //[Route("categories"), HttpGet]
        //public HttpResponseMessage GetUserTransactionCategories()
        //{
        //    if (!UserService.IsLoggedIn())
        //    {
        //        HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
        //    }
        //    string userId = UserService.GetCurrentUserId();

        //    CategoryCollectionDomain collection = LogService.Data.GetUserCategories(userId);

        //    var response = new ItemResponse<CategoryCollectionDomain> { Item = collection };

        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}


        // .........................................................................................

        //[Route("categories"), HttpPost]
        //public HttpResponseMessage InsertCategory(CategoryInsertRequest model)
        //{
        //    if (!UserService.IsLoggedIn())
        //    {
        //        HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    model.UserId = UserService.GetCurrentUserId();
        //    model.ForecastType = "Fixed";
        //    model.FixedPrediction = 20;

        //    int categoryId = LogService.Data.InsertCategory(model);

        //    var response = new ItemResponse<int> { Item = categoryId };

        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}


        // .........................................................................................

        //[Route("categories"), HttpPut]
        //public HttpResponseMessage UpdateCategories(CategoriesUpdateRequest model)
        //{
        //    if (!UserService.IsLoggedIn())
        //    {
        //        HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    string userId = UserService.GetCurrentUserId(); 

        //    bool isSuccess = LogService.Data.UpdateCategoryCollection(model, userId);

        //    var response = new ItemResponse<bool> { Item = isSuccess };

        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}


        // .........................................................................................

        //[Route("category"), HttpDelete]
        //public HttpResponseMessage DeleteCategories(int categoryId)
        //{
        //    if (!UserService.IsLoggedIn())
        //    {
        //        HttpError error = new HttpError { Message = "Cannot process request for unauthenticated user." };
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    bool isSuccess = LogService.Data.DeleteCategory(categoryId);

        //    var response = new ItemResponse<bool> { Item = isSuccess };

        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}

        // --| Months |---------------------------------------------------------------------------->
        [Authorize]
        [Route("CurrentMonth"), HttpGet]
        public HttpResponseMessage GetCuttentMonth()
        {
            try
            {
                Month currentMonth = LogService.GetCurrentMonth();

                var response = new ItemResponse<Month> { Item = currentMonth };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            
        }

        // --| User Log |-------------------------------------------------------------------------->

        [Route("initializeUserLog"), HttpPost]
        public HttpResponseMessage InsertNewUserLog(NewUserLogInsertRequest model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                bool isSuccess = LogService.CreateNewLog(model);

                if (isSuccess)
                {
                    var response = new ItemResponse<bool> { Item = isSuccess };

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to Process Request.");
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }


        // --| Preferences |----------------------------------------------------------------------->

        //[Route("preferences"), HttpPut]
        //public HttpResponseMessage UpdateUserPreferences(PreferencceUpdateRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    bool isSuccess = LogService.Data.UpdateUserPreferences(model);

        //    var response = new SuccessResponse();

        //    if(!isSuccess)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Unable to make update");
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, response);
        //    }
        //}
    }
}