using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ForgotPasswordResetByOTP.CommonLayer
{
    public class EmailService
    {
        //public void SendEmailOTP(string toEmail, string otp)
        //{
        //    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
        //    {
        //        Credentials = new NetworkCredential("rakesheps@gmail.com", "jpix yhlr zgxl wqir"),
        //        EnableSsl = true
        //    };

        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress("rakesheps@gmail.com");
        //    message.To.Add(toEmail);
        //    message.Subject = "OTP Verification";
        //    message.Body = $"Your OTP is: {otp}";

        //    smtp.Send(message);
        //}

        public void SendRegistrationEmail(string toEmail, string filePath)
        {
            string subject = "Welcome to Password Reset Application";

            string body = System.IO.File.ReadAllText(filePath);
            body = body.Replace("{{EMAIL}}", toEmail);
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("your-email@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("rakesheps@gmail.com", "jpix yhlr zgxl wqir");

            smtp.Send(mail);
        }

        public void SendOtpEmail(string toEmail, string otp, string filePath)
        {
            string subject = "Your OTP for Password Reset";
            string body = System.IO.File.ReadAllText(filePath);
            body = body.Replace("{{OTP}}", otp);

            // send email logic
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress("rakesheps@gmail.com"); // your email
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("rakesheps@gmail.com", "jpix yhlr zgxl wqir");

            smtp.Send(mail);
        }
       
    }
}