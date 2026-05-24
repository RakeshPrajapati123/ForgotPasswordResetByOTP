using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ForgotPasswordResetByOTP.DAL
{
    public class UserDAL
    {
        DBHelper db = new DBHelper();

        public bool RegisterUser(string email, string password)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisterUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable LoginUser(string email)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GetUserByEmail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }

            catch (Exception ex)
            {
                return new DataTable();

            }
        }

        public bool SaveOTP(string email, string otp, DateTime expiry)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_SaveOTP", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@OTP", SqlDbType.NVarChar).Value = otp;
                    cmd.Parameters.Add("@Expiry", SqlDbType.DateTime).Value = expiry;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0; // ✅ success check
                }
            }
            catch (Exception ex)
            {
                // log error
                return false; // ✅ valid now
            }
        }

        public DataTable VerifyOTP(string email, string otp)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_VerifyOTP", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OTP", otp);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }

            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public bool ResetPassword(string email, string password)
        {
            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_ResetPassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0; // ✅ success check
                }
            }
            catch (SqlException sqlEx)
            {
                // TODO: log sqlEx.Message
                return false;
            }
            catch (Exception ex)
            {
                // TODO: log ex.Message
                return false;
            }
        }
    }
}