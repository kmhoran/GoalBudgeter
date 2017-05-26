using Goal.Models;
using Goal.Models.Domain;
using Goal.Models.ViewModels;
using Goal.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Goal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendEmail()
        {
            // Send Email-verification Email
            IdentityUser user = UserService.GetCurrentUser();
            ApplicationUser userContext = UserService.GetUserById(user.Id);
            Guid token = TokenService.CreateToken(user.Id);
            await Services.EmailService.SendEmailConfirmationLetter(userContext, token);

            return RedirectToAction("EmailSent");
        }

        public ActionResult EmailSent()
        {
            return View();
        }


        public ActionResult VerifyAccount([FromUri] string identity, Guid key)
        {
            if(TokenService.IsValidToken(identity,key))
            {
                UserService.ConfirmUserEmail(identity);

                return RedirectToAction("Index", "Goal");
            }
            else
            {
                var error = new ErrorMessageVM { Code = 498, Message = "Invalid Token" };

                return RedirectToAction("Error", "Home", error);
            }
        }

        public ActionResult Error(ErrorMessageVM error)
        {
            if (error == null)
            {
                error = new ErrorMessageVM { Code = 0, Message = "Woops! Something went wrong." };
            }

            return View(error);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}