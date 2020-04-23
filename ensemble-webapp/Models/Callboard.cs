using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Callboard
    {
        public Callboard(int intCallboardID, string strSubject, string strNote, DateTime dtmDateTime, Member postedByMember, Event paramEvent)
        {
            IntCallboardID = intCallboardID;
            StrSubject = strSubject;
            StrNote = strNote;
            DtmDateTime = dtmDateTime;
            PostedByMember = postedByMember;
            Event = paramEvent;
        }

        public int IntCallboardID { get; set; }

        public string StrSubject { get; set; }

        public string StrNote { get; set; }

        public DateTime DtmDateTime { get; set; }

        public Member PostedByMember { get; set; }

        public Event Event { get; set; }
    }
}