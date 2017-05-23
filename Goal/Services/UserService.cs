using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Goal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Goal.Services;
using Goal.Models;
using Goal.Exceptions;

namespace Goal.Services
{
    public class UserService : BaseService
    {

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static ApplicationUserManager GetUserManager()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static IdentityUser CreateUser(string email, string password)
        {
            ApplicationUserManager userManager = GetUserManager();

            ApplicationUser newUser = new ApplicationUser { UserName = email, Email = email, LockoutEnabled = false };
            IdentityResult result = null;
            try
            {
                result = userManager.Create(newUser, password);
            }
            catch
            {
                throw;
            }

            if (result.Succeeded)
            {
                return newUser;
            }
            else
            {
                throw new IdentityResultException(result);
            }
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static bool Signin(string emailaddress, string password)
        {
            bool result = false;

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.Find(emailaddress, password);
            if (user != null)
            {
                ClaimsIdentity signin = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, signin);
                result = true;

            }
            return result;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static bool IsUser(string emailaddress)
        {
            bool result = false;

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindByEmail(emailaddress);

            if (user != null)
            {
                result = true;
            }

            return result;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static ApplicationUser GetUser(string emailaddress)
        {

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindByEmail(emailaddress);

            return user;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static ApplicationUser GetUserById(string userId)
        {

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindById(userId);

            return user;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static bool ChangePassWord(string userId, string newPassword)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPassword))
            {
                throw new Exception("You must provide a userId and a password");
            }

            ApplicationUser user = GetUserById(userId);

            if (user != null)
            {

                ApplicationUserManager userManager = GetUserManager();

                userManager.RemovePassword(userId);
                IdentityResult res = userManager.AddPassword(userId, newPassword);

                result = res.Succeeded;
            }

            return result;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static bool Logout()
        {
            bool result = false;

            IdentityUser user = GetCurrentUser();

            if (user != null)
            {
                IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                result = true;
            }

            return result;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static IdentityUser GetCurrentUser()
        {
            if (!IsLoggedIn())
                return null;
            ApplicationUserManager userManager = GetUserManager();

            IdentityUser currentUserId = userManager.FindById(GetCurrentUserId());
            return currentUserId;
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Built-in method.
        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetCurrentUserId());

        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static bool IsEmailVerified()
        {
            bool result = false;

            IdentityUser user = GetCurrentUser();

            result = user.EmailConfirmed;

            return result;
        }
    }
}




