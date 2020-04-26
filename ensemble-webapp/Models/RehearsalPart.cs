using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class RehearsalPart
    {
        public RehearsalPart(int intRehearsalPartID, DateTime dtmStartDateTime, DateTime dtmEndDateTime, string strDescription, Rehearsal rehearsal, Types type, Event @event)
        {
            IntRehearsalPartID = intRehearsalPartID;
            DtmStartDateTime = dtmStartDateTime;
            DtmEndDateTime = dtmEndDateTime;
            StrDescription = strDescription;
            Rehearsal = rehearsal;
            Type = type;
            Event = @event;
        }

        public int IntRehearsalPartID { get; set; }

        public DateTime DtmStartDateTime { get; set; }

        public DateTime DtmEndDateTime { get; set; }

        public string StrDescription { get; set; }

        public Rehearsal Rehearsal { get; set; }

        public Types Type { get; set; }

        public Event Event { get; set; }
    }
}