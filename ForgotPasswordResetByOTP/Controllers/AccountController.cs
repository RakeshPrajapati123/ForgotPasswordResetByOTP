using ForgotPasswordResetByOTP.BAL;
using ForgotPasswordResetByOTP.CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForgotPasswordResetByOTP.Controllers
{
    public class AccountController : Controller
    {
        UserBAL bal = new UserBAL();

        public ActionResult Register()
        {
            return View();
        }

        // POST: Register User
        [HttpPost]
        public ActionResult Register(string email, string password)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Message = "All fields are required";
                    return View();
                }

                // Call BAL
                string result = bal.RegisterUser(email, password);

                EmailService emailService = new EmailService();

                string path = Server.MapPath("~/Views/EmailTemplates/RegisterEmailTemplate.html");

                emailService.SendRegistrationEmail(email, path);

                if (result == "Success")
                {
                    TempData["Success"] = "Registration Successful!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = result;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong!";
                return View();
            }
        }

        // GET
        public ActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                string result = bal.LoginUser(email, password);

                if (result == "Success")
                {
                    Session["UserEmail"] = email; // ✅ session create
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.Message = result;
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "Something went wrong!";
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (!bal.IsEmailExists(email))
            {
                ViewBag.Message = "Email not found";
                return View();
            }

            string otp = bal.GenerateOTP();
            bal.SaveOTP(email, otp);

            EmailService emailService = new EmailService();

            string path = Server.MapPath("~/Views/EmailTemplates/OtpEmailTemplate.html");

            emailService.SendOtpEmail(email, otp, path);

            Session["Email"] = email;
            
            return RedirectToAction("VerifyOTP");
        }

        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(string otp)
        {
            string email = Session["Email"].ToString();

            if (!bal.VerifyOTP(email, otp))
            {
                ViewBag.Message = "Invalid OTP";
                return View();
            }

            Session["VerifiedEmail"] = email;
            return RedirectToAction("ResetPassword");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string newPassword)
        {
            string email = Session["VerifiedEmail"].ToString();
                        
            bal.ResetPassword(email, newPassword);

            return RedirectToAction("Login");
        }
    }
}