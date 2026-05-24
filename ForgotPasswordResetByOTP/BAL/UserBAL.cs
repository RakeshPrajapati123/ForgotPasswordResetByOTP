using ForgotPasswordResetByOTP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace ForgotPasswordResetByOTP.BAL
{
    public class UserBAL
    {
        UserDAL dal = new UserDAL();

        public string RegisterUser(string email, string password)
        {
            // Check if email already exists
            var dt = dal.GetUserByEmail(email);

            if (dt != null && dt.Rows.Count > 0)
            {
                return "Email already exists";
            }

            // Hash password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            bool result = dal.RegisterUser(email, hashedPassword);

            if (!result)
                return "Registration Failed";

            return "Success";
        }

        public bool IsEmailExists(string email)
        {
            var dt = dal.GetUserByEmail(email);
            return dt.Rows.Count > 0;
        }

        public string LoginUser(string email, string password)
        {
            var dt = dal.LoginUser(email);

            if (dt == null)
                return "Error";

            if (dt.Rows.Count == 0)
                return "User not found";

            string storedHash = dt.Rows[0]["Password"].ToString();

            bool isValid = BCrypt.Net.BCrypt.Verify(password.Trim(), storedHash);

            if (!isValid)
                return "Invalid Password";

            return "Success";
        }

        public string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

        public void SaveOTP(string email, string otp)
        {
            dal.SaveOTP(email, otp, DateTime.Now.AddMinutes(10));
        }

        public bool VerifyOTP(string email, string otp)
        {
            var dt = dal.VerifyOTP(email, otp);
            return dt.Rows.Count > 0;
        }

        public void ResetPassword(string email, string password)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password.Trim());
            dal.ResetPassword(email, hashedPassword);
        }
    }
}