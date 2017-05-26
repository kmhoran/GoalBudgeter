using Goal.Models;
using Goal.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Goal.Services
{
    public class EmailService
    {
        public static async Task<bool> SendEmailConfirmationLetter(ApplicationUser user, Guid token)
        {
            if (user.EmailConfirmed == true)
            {
                Exception e = new Exception();
                throw e;
            }

            bool isSuccess = false;

            MailAddress userEmail = new MailAddress(user.Email);
            // This address to be updated for production
            string verificationAddress = String.Concat("http://localhost:55960/Home/VerifyAccount/?identity=",user.Id,"&key=", token);
            string body = "<p>Hi {0},</p><p>Welcome to Goal Budget! To access your account, please follow the link below:</p><a href='{1}'>{1}</a>";

            var email = new MailMessage();
            email.To.Add(userEmail);
            email.Subject = "Email Verification";
            email.Body = String.Format(body, userEmail.User, verificationAddress);
            email.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(email);
            }

            isSuccess = true;

            return isSuccess;

        }
    }
}