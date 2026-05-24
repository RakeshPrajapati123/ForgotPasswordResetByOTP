using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ForgotPasswordResetByOTP.DAL
{
    public class DBHelper
    {
        private readonly string connectionString;

        public DBHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}