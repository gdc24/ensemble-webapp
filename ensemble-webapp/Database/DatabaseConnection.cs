using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// below for ComputerSHA256Hash function
using System.Text;
using System.Security.Cryptography;

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
        
        // Compute hash of a string using SHA 256
        public static ComputeSHA256Hash(string toHash, byte[] salt)
        {
            using (SHA256 hash = SHA256.Create())
            {
                // Computer hash in byte form
                byte[] b = hash.ComputeHash(Encoding.UTF8.GetBytes(toHash));

                // Concatenate toHash and salt byte arrays
                byte[] full = new int[b.Length + salt.Length];
                Array.Copy(b, full, b.Length);
                Array.Copy(salt, full, b.Length, salt.Length);

                // Convert byte array hash back to string
                StringBuilder builder2 = new StringBuilder();
                for (int i = 0; i < full.Length; i++)
                {
                    builder2.Append(full[i].ToString("x2"));
                }

                return builder2.ToString();
            }
        }
    }
}