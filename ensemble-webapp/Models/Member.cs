using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Member
    {
        public Member(int intMemberID, string strName, byte[] bytSalt, byte[] bytKey, string strUsername, string strEmail, int intPhone, Event paramEvent)
        {
            IntMemberID = intMemberID;
            BytSalt = bytSalt;
            BytKey = bytKey;
            StrUsername = strUsername;
            StrName = strName;
            StrEmail = strEmail;
            IntPhone = intPhone;
            Event = paramEvent;
        }

        public int IntMemberID { get; set; }

        public byte[] BytSalt { get; set; }

        public byte[] BytKey { get; set; }

        public string StrUsername { get; set; }

        public string StrName { get; set; }

        public string StrEmail { get; set; }

        public int IntPhone { get; set; }

        public Event Event { get; set; }

        public bool 
    }
}