using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using ensemble_webapp.Models;
using System.Collections.Generic;

namespace ensemble_webapp.Database
{

    public class Login
    { 
        // returns true for successful login
        public static bool VerifyUser(Users users)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            // find enteredUser in database
            Users usr = getDAL.GetUserByName(users.StrName);

            // if username is found
            if (usr != null)
            {
                byte[] userSalt = usr.BytSalt;
                byte[] userKey = usr.BytKey;
                byte[] actual = ComputeSHA256Hash(users.StrPassword, userSalt);

                if (StructuralComparisons.StructuralEqualityComparer.Equals(userKey, actual))
                {
                    Globals.LOGIN_STATUS = true;
                    Globals.LOGGED_IN_USER = usr;
                    return true;
                }
            }

            getDAL.CloseConnection();

            // did not find username
            return false;
        }

        // returns true if new user is created successfully
        public static bool CreateUser(Users newUser)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            Users usr = getDAL.GetUserByName(newUser.StrName);

            // if no user found by username
            if (usr == null)
            {
                // prompt for name, email, phone, eventID

                // get email
                string email = newUser.StrEmail;
                if (!IsValidEmail(email))
                {
                    return false;
                }

                // get phone
                string phone = newUser.StrPhone;
                if (phone.Length != 10)
                {
                    return false;
                }

                InsertDAL insertDAL = new InsertDAL();
                insertDAL.OpenConnection();

                // generate random number for salt and convert it to a byte array for key
                byte[] salt = BitConverter.GetBytes(new Random().Next());
                byte[] key = ComputeSHA256Hash(newUser.StrPassword, salt);

                int intNewUserID = insertDAL.InsertUser(new Users(newUser.StrName, salt, key, email, phone));

                insertDAL.CloseConnection();

                GetDAL get = new GetDAL();
                get.OpenConnection();

                Users completeUser = get.GetUserByID(intNewUserID);

                get.CloseConnection();

                Globals.LOGGED_IN_USER = completeUser;
                Globals.LOGIN_STATUS = true;

                return true;
            }

            getDAL.CloseConnection();

            return false;
        }
        
        // returns true if password successfully changed
        public static bool ChangePass(Users user, string oldPassword, string newPassword)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            Users usr = getDAL.GetUserByName(user.StrName);

            if (VerifyUser(new Users(user.StrName, user.BytSalt, ComputeSHA256Hash(oldPassword, getDAL.GetUserByID(user.IntUserID).BytSalt), user.StrEmail, user.StrPhone)))
            {
                InsertDAL insertDAL = new InsertDAL();
                insertDAL.OpenConnection();

                // need to implement GetSaltByUser
                byte[] newKey = ComputeSHA256Hash(newPassword, getDAL.GetUserByID(user.IntUserID).BytSalt);

                insertDAL.InsertNewHash(newKey);

                insertDAL.CloseConnection();
            }

            getDAL.CloseConnection();

            return false;

        }

        // closes database connection for logging out
        public static bool Logout()
        {
            // DatabaseConnnection.CloseConnection();
            Globals.LOGIN_STATUS = false;
            Globals.LOGGED_IN_USER = null;
            return true;
        }

        // Compute hash of a string using SHA 256
        private static byte[] ComputeSHA256Hash(string toHash, byte[] salt)
        {
            using (SHA256 hash = SHA256.Create())
            {
                // Computer hash in byte form
                byte[] b = hash.ComputeHash(Encoding.UTF8.GetBytes(toHash));

                // Concatenate toHash and salt byte arrays
                byte[] key = new byte[b.Length + salt.Length];

                Buffer.BlockCopy(b, 0, key, 0, b.Length);
                Buffer.BlockCopy(salt, 0, key, b.Length, salt.Length);

                return key;
            }
        }

        // check for valid email
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}