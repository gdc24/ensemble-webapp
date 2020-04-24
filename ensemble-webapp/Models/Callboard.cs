using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Callboard : IComparable<Callboard>
    {
        public Callboard(int intCallboardID, string strSubject, string strNote, DateTime dtmDateTime, Users postedByUser, Event paramEvent)
        {
            IntCallboardID = intCallboardID;
            StrSubject = strSubject;
            StrNote = strNote;
            DtmDateTime = dtmDateTime;
            PostedByUser = postedByUser;
            Event = paramEvent;
        }

        public int IntCallboardID { get; set; }

        public string StrSubject { get; set; }

        public string StrNote { get; set; }

        public DateTime DtmDateTime { get; set; }

        public Users PostedByUser { get; set; }

        public Event Event { get; set; }

        public int CompareTo(Callboard other)
        {
            return this.DtmDateTime.CompareTo(other.DtmDateTime);
        }
    }
}