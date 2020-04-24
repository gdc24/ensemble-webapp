using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Users
    {
        public Users(int intUserID, string strName, byte[] bytSalt, byte[] bytKey, string strUsername, string strEmail, int intPhone)
        {
            IntUserID = intUserID;
            StrName = strName;
            BytSalt = bytSalt;
            BytKey = bytKey;
            StrUsername = strUsername;
            StrEmail = strEmail;
            IntPhone = intPhone;
        }

        public Users(string strName, byte[] bytSalt, byte[] bytKey, string strUsername, string strEmail, int intPhone)
        {
            StrName = strName;
            BytSalt = bytSalt;
            BytKey = bytKey;
            StrUsername = strUsername;
            StrEmail = strEmail;
            IntPhone = intPhone;
        }

        public int IntUserID { get; set; }

        public byte[] BytSalt { get; set; }

        public byte[] BytKey { get; set; }

        public string StrUsername { get; set; }

        public string StrName { get; set; }

        public string StrEmail { get; set; }

        public int IntPhone { get; set; }

        public List<Event> LstEvents { get; set; }
    }
}