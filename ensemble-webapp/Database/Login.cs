// below for ComputerSHA256Hash function
using System.Text;
using System.Security.Cryptography;
using System;

namespace ensemble_webapp.Database
{

    public class Login
    {

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