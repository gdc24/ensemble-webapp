using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Dynamic;
using System.Linq;
using ensemble_webapp.Models;

namespace ensemble_webapp.Database
{

    public class Login
    {
        // returns true for successful login
        public static bool VerifyUser(String enteredUser, String enteredPassword, string key)
        {
            // GetDAL.GetDAL(); <- is this needed here?
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();
            // find enteredUser in database
            Member usr = getDAL.GetMemberByUsername(enteredUser);
            // if cannot find entered User, return false;
            // if can find enteredUser then
            String userPass = "";
            byte[] userSalt = usr.BytSalt;
            byte[] userKey = usr.BytKey;
            String actual = ComputeSHA256Hash(userPass, userSalt);
            String toCheck = ComputeSHA256Hash(enteredPassword, salt);
            // if found enteredUser then
            {
                String userPass = "";
                byte[] userSalt;
                string userKey = "";

                if (userKey.SequenceEqual(key))
                {
                    return true;
                }
            }

            // GetDAL.CloseConnection(); <- only needed if above is needed?

            return false;
        }

        public static bool CreateUser(String username, String password)
        {
            // GetDAL.GetDAL(); <- is this needed here?

            // check if user already exists in database
            // if username not already in database
                {
                    InsertDAL.InsertDAL();

                    // generate random number and convert it to a byte array for salt
                    byte[] salt = BitConverter.GetBytes(new Random().Next(Int32));

                    string key = ComputeSHA256Hash(password, salt);
                    InsertDAL.InsertUser(username, password, salt);

                    InsertDAL.CloseConnection();

                    return true;
                }

            // GetDAL.CloseConnection(); <- only needed if above is needed?
            getDAL.CloseConnection();

            return false;
        }

        // Compute hash of a string using SHA 256
        public static string ComputeSHA256Hash(String toHash, byte[] salt)
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
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < full.Length; i++)
                {
                    builder.Append(full[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}