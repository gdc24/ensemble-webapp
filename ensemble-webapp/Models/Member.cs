using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Member
    {
        public Member(int intMemberID, string strName, string strEmail, int intPhone, Event paramEvent)
        {
            IntMemberID = intMemberID;
            StrName = strName;
            StrEmail = strEmail;
            IntPhone = intPhone;
            Event = paramEvent;
        }

        public int IntMemberID { get; set; }

        public string StrName { get; set; }

        public string StrEmail { get; set; }

        public int IntPhone { get; set; }

        public Event Event { get; set; }
    }
}