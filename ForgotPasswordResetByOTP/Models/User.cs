using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForgotPasswordResetByOTP.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
    }
}