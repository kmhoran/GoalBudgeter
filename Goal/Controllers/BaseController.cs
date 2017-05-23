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

            if(UserService.IsEmailVerified())
            {
                return base.View();
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
                return base.View(viewString);
            }
            else
            {
                return base.View("../Home/InvalidEmail");
            }
        }

    }
}