﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Users
    {
        public Users(int intUserID, string strName, byte[] bytSalt, byte[] bytKey, string strEmail, string strPhone)
        {
            IntUserID = intUserID;
            StrName = strName;
            BytSalt = bytSalt;
            BytKey = bytKey;
            StrEmail = strEmail;
            StrPhone = strPhone;
        }

        public Users(string strName, byte[] bytSalt, byte[] bytKey, string strEmail, string strPhone)
        {
            StrName = strName;
            BytSalt = bytSalt;
            BytKey = bytKey;
            StrEmail = strEmail;
            StrPhone = strPhone;
        }

        public int IntUserID { get; set; }

        public byte[] BytSalt { get; set; }

        public byte[] BytKey { get; set; }

        public string StrUsername { get; set; }

        public string StrPassword { get; set; }

        public string StrName { get; set; }

        public string StrEmail { get; set; }

        public string StrPhone { get; set; }

        public List<Event> LstEvents { get; set; }
    }
}