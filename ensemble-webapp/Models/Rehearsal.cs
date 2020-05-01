using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Rehearsal
    {
        public Rehearsal()
        {
        }

        public Rehearsal(int intRehearsalID, DateTime dtmStartDateTime, DateTime dtmEndDateTime, string strLocation, string strNotes, Event paramEvent)
        {
            IntRehearsalID = intRehearsalID;
            DtmStartDateTime = dtmStartDateTime;
            DtmEndDateTime = dtmEndDateTime;
            StrLocation = strLocation;
            StrNotes = strNotes;
            Event = paramEvent;
        }

        public string DateWithEvent { get; set; }

        public int IntRehearsalID { get; set; }

        public DateTime DtmStartDateTime { get; set; }

        public DateTime DtmEndDateTime { get; set; }

        public string StrLocation { get; set; }

        public string StrNotes { get; set; }

        public Event Event { get; set; }

        public List<RehearsalPart> LstRehearsalParts { get; set; }
    }
}