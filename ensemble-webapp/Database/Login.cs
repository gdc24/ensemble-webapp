using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Dynamic;
using System.Linq;

namespace ensemble_webapp.Database
{

    public class Login
    {
        // returns true for successful login
        public static bool VerifyUser(string enteredUser, string enteredPassword, byte[] salt, byte[] key)
        {
            // GetDAL.GetDAL(); <- is this needed here?
            // find enteredUser in database
            // if cannot find entered User, return false;
            // if can find enteredUser then
            String userPass = "";
            byte[] userSalt;
            byte[] userKey;
            String actual = ComputeSHA256Hash(userPass, userSalt);
            String toCheck = ComputeSHA256Hash(enteredPassword, salt);

            if (userKey.SequenceEqual(key) && actual.Equals(toCheck))
            {
                return true;
            }

            // GetDAL.CloseConnection(); <- only needed if above is needed?
        }
        
        // Compute hash of a string using SHA 256
        public static string ComputeSHA256Hash(string toHash, byte[] salt)
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