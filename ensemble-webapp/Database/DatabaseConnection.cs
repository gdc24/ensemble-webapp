using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Database
{
    public class DatabaseConnection
    {
        public static readonly string SERVER = "ec2-52-6-143-153.compute-1.amazonaws.com";
        public static readonly string USER = "gyevrymixnlzst";
        public static readonly string DATABASE = "dceh4hrbfiso1o";
        public static readonly string PASS = "e1930311b68ec1fdc7c8e1efca93161c3503ecd271bd07b47d85d3fbcc052a32";

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection("" +
                "Server=" + SERVER + ";" +
                "User Id=" + USER + ";" +
                "Password=" + PASS + ";" +
                "Database=" + DATABASE + ";");

            return conn;
        }
    }
}