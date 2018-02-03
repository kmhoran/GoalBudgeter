using Goal.Enums;
using Goal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Goal.Controllers
{
    [Authorize]
    public class GoalController : BaseController
    {
        public ActionResult Index()
        {
            LogStatusType currentStatus = LogService.GetStatusForCurrentMonth();

            switch (currentStatus)
            {
                case LogStatusType.OK:
                    return View();
                case LogStatusType.NewUser:
                    return RedirectToAction("NewUser");
                case LogStatusType.NewYear:
                    return RedirectToAction("NewYear");
                default:
                    return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Transaction()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult NewUser()
        {
            LogStatusType currentStatus = LogService.GetStatusForCurrentMonth();

            switch (currentStatus)
            {
                case LogStatusType.NewUser:
                    return View();
                case LogStatusType.OK:
                    return RedirectToAction("Index");
                case LogStatusType.NewYear:
                    return RedirectToAction("NewYear");
                default:
                    return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult NewYear()
        {
            LogStatusType currentStatus = LogService.GetStatusForCurrentMonth();

            switch (currentStatus)
            {
                case LogStatusType.NewYear:
                    return View();
                case LogStatusType.OK:
                    return RedirectToAction("Index");
                case LogStatusType.NewUser:
                    return RedirectToAction("NewUser");
                default:
                    return RedirectToAction("Error", "Home");
            }
        }
    }
}