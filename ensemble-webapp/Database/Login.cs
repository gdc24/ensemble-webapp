using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ensemble_webapp.Database
{

    public class Login
    {
        // returns true for successful login
        public static bool VerifyUser(String enteredUser, String enteredPassword)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            // find enteredUser in database
            Member usr = getDAL.GetMemberByUsername(enteredUser);

            // if username is found
            if (usr != null)
            {
                byte[] userSalt = usr.BytSalt;
                byte[] userKey = usr.BytKey;
                string actual = ComputeSHA256Hash(enteredPassword, userSalt);

                if (userKey.SequenceEqual(actual))
                {
                    return true;
                }
            }

            getDAL.CloseConnection();

            // did not find username
            return false;
        }

        // returns true if new user is created successfully
        public static bool CreateUser(String username, String password)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            Member usr = getDAL.GetMemberByUsername(username);

            // if no user found by username
            if (usr == null)
            {
                InsertDAL insertDAL = new InsertDAL();
                insertDAL.OpenConnection();

                // generate random number and convert it to a byte array for salt
                byte[] salt = BitConverter.GetBytes(new Random().Next(Int32));

                byte[] key = ComputeSHA256Hash(password, salt);
                insertDAL.InsertUser(username, salt, key);

                insertDAL.CloseConnection();

                return true;
            }

            getDAL.CloseConnection();

            return false;
        }

        // closes database connection for logging out
        public static bool Logout()
        {
            DatabaseConnnection.CloseConnection();
        }
        // Compute hash of a string using SHA 256
        public static byte[] ComputeSHA256Hash(String toHash, byte[] salt)
        {
            using (SHA256 hash = SHA256.Create())
            {
                // Computer hash in byte form
                byte[] b = hash.ComputeHash(Encoding.UTF8.GetBytes(toHash));

                // Concatenate toHash and salt byte arrays
                byte[] full = new int[b.Length + salt.Length];

                return full;
            }
        }
    }
}