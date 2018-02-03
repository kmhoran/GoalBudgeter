using Goal.Models.ViewModels;
using Goal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Goal.Controllers
{
    public class BaseController : Controller
    {

        public new ViewResult View()
        {

            if (UserService.IsEmailVerified())
            {
                BaseViewModel BaseModel = GetViewModel<BaseViewModel>(); // create a new instance

                BaseModel = DecorateViewModel<BaseViewModel>(BaseModel); // add user id to new instance

                return base.View(BaseModel);
            }
            else
            {
                return base.View("../Home/InvalidEmail");
            }
        }

        public new ViewResult View(string viewString)
        {

            if (UserService.IsEmailVerified())
            {
                BaseViewModel BaseModel = GetViewModel<BaseViewModel>(); // create a new instance

                BaseModel = DecorateViewModel<BaseViewModel>(BaseModel); // add user id to new instance

                return base.View(viewString, BaseModel);
            }
            else
            {
                return base.View("../Home/InvalidEmail");
            }
        }

        public ViewResult View(string ViewStrings, BaseViewModel BaseVMS)
        {
            if (UserService.IsEmailVerified())
            {
                BaseVMS = DecorateViewModel<BaseViewModel>(BaseVMS); // add user ID to BaseVMS

                return base.View(ViewStrings, BaseVMS); // call the base class controller and return the ID to it
            }
            else
            {
                return base.View("../Home/InvalidEmail");
            }
        }

        public ViewResult View(BaseViewModel baseVM)
        {
            if (UserService.IsEmailVerified())
            {
                baseVM = DecorateViewModel<BaseViewModel>(baseVM); // decorate baseVM with the string id

                return base.View(baseVM); // call the base class controller and return the ID to it
            }
            else
            {
                return base.View("../Home/InvalidEmail");
            }
        }

        protected T GetViewModel<T>() where T : BaseViewModel, new() //creates a new instance of the object in the base view model
        {
            //don't add anything to this function "ever"

            T viewModel = new T(); // create a new variable using T 
            return viewModel; //return the id to the variable
        }

        protected T DecorateViewModel<T>(T model) where T : BaseViewModel // T is extended with the baseviewmodel
        {
            if (UserService.IsLoggedIn())
            {
                string userId = UserService.GetCurrentUserId();

                model.IsLoggedIn = true;
                model.LogStatus = LogService.Data.GetLogStatus(userId);
            }
            else
            {
                model.IsLoggedIn = false;
            }

            return model;
        }
    }
}