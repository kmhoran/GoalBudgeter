using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Goal.Controllers
{
    [Authorize]
    public class GoalController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transaction()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
    }
}