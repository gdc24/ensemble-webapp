using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ensemble_webapp.Database
{

    public class Login
    {
        // returns true for successful login
        public static bool VerifyUser(string enteredUser, string enteredPassword)
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
                    Globals.LOGIN_STATUS = true;
                    return true;
                }
            }

            getDAL.CloseConnection();

            // did not find username
            return false;
        }

        // returns true if new user is created successfully
        public static bool CreateUser(string username, string password, int eventID)
        {
            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            Member usr = getDAL.GetMemberByUsername(username);

            // if no user found by username
            if (usr == null)
            {
                // prompt for name, email, phone, eventID

                // somehow get an event
                Event e = getDAL.GetEvent(eventID); 
                if (e == null)
                {
                    return false;
                }

                // get name
                string name = "";

                // get email
                string email = "";
                if (!IsValidEmail(email))
                {
                    return false;
                }

                // get phone
                string phone = "";
                if (phone.Length != 10)
                {
                    return false;
                }

                InsertDAL insertDAL = new InsertDAL();
                insertDAL.OpenConnection();

                // generate random number for salt and convert it to a byte array for key
                byte[] salt = BitConverter.GetBytes(new Random().Next(Int32));

                byte[] key = ComputeSHA256Hash(password, salt);
                insertDAL.InsertMember(new Member(name, salt, key, username, email, phone, e));

                insertDAL.CloseConnection();

                Globals.LOGIN_STATUS = true;
                return true;
            }

            getDAL.CloseConnection();

            return false;
        }

        // closes database connection for logging out
        public static bool Logout()
        {
            // DatabaseConnnection.CloseConnection();
            Globals.LOGIN_STATUS = false;
        }

        // Compute hash of a string using SHA 256
        public static byte[] ComputeSHA256Hash(string toHash, byte[] salt)
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

        // check for valid email
        private bool IsValidEmail(string email)
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