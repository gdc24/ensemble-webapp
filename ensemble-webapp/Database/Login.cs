using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using ensemble_webapp.Models;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

namespace ensemble_webapp.Database
{

    public class Login
    {

        private static bool LoginUser(Users usr)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            usr.LstEvents = getDAL.GetEventsByUser(usr.IntUserID);
            usr.LstEventsIsAdmin = getDAL.GetAdminEventsByUser(usr.IntUserID);
            Globals.LOGIN_STATUS = true;
            Globals.LOGGED_IN_USER = usr;

            if (Globals.ADMINS.Contains(usr))
            {
                Globals.IS_ADMIN = true;
            }

            getDAL.CloseConnection();

            return true;
        }

        /* Takes the entered user and verifies if that is a
         * valid user in the database by checking its entered
         * username and password against the username and hashed
         * key store in the database. If the username and hashed
         * password match what is stored in the databse, then 
         * a boolean value of true is returned; otherwise, 
         * this function returns false.
         * 
         * @param users     user to be checked
         * 
         * @return          true if the user is verified to be
         *                  a valid user, false otherwise.
         */
        public static bool VerifyUser(Users users)
        {
            // open a database connection
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            // find entered username in the databased
            Users usr = getDAL.GetUserByName(users.StrName);

            // close the database connection
            getDAL.CloseConnection();

            // if username is found
            if (usr != null)
            {
                // get the stored key and salt for the user
                byte[] userSalt = usr.BytSalt;
                byte[] userKey = usr.BytKey;
                byte[] actual;
                try
                {
                    /* compute a hashed key with the user's stored 
                    salt and entered password */
                    actual = ComputeSHA256Hash(users.StrPassword, userSalt);
                } catch (ArgumentNullException)
                {
                    /* catch an ArgumentNullException triggered by 
                    blank password */
                    return false;
                }

                /* if the computed hashed key matches what is stored 
                   in the database then return login status */
                if (StructuralComparisons.StructuralEqualityComparer.Equals(userKey, actual))
                {
                    return LoginUser(usr);
                }
            }

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
                byte[] key;
                try
                {
                    key = ComputeSHA256Hash(newUser.StrPassword, salt);
                } catch (ArgumentNullException)
                {
                    return false;
                }

                int intNewUserID = insertDAL.InsertUser(new Users(newUser.StrName, salt, key, email, phone));

                insertDAL.CloseConnection();

                GetDAL get = new GetDAL();
                get.OpenConnection();

                Users completeUser = get.GetUserByID(intNewUserID);

                get.CloseConnection();

                return LoginUser(completeUser);
            }

            getDAL.CloseConnection();

            return false;
        }
        
        // returns true if password successfully changed
        public static bool ChangePass(Users user, string oldPassword, string newPassword)
        {
            user.BytKey = ComputeSHA256Hash(oldPassword, user.BytSalt);
            user.StrPassword = oldPassword;
            if (VerifyUser(user))
            {
                InsertDAL insertDAL = new InsertDAL();
                insertDAL.OpenConnection();

                byte[] newKey;
                try
                {
                    newKey = ComputeSHA256Hash(newPassword, user.BytSalt);
                } catch (ArgumentNullException)
                {
                    return false;
                }

                insertDAL.UpdateUserKey(user.IntUserID, newKey);

                insertDAL.CloseConnection();
                    
                return true;
                
            }

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
            if (string.IsNullOrEmpty(toHash))
                throw new ArgumentNullException(nameof(toHash));

            // Computer hash in byte form
            byte[] b = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(toHash));

            // Concatenate toHash and salt byte arrays
            byte[] key = new byte[b.Length + salt.Length];

            Buffer.BlockCopy(b, 0, key, 0, b.Length);
            Buffer.BlockCopy(salt, 0, key, b.Length, salt.Length);

            return key;
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