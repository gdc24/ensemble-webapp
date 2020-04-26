using NodaTime;
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

        public List<Users> LstMembers { get; set; } 

        public int[] ArrMemberNeededIDs { get; set; }

        public int IntRehearsalPartID { get; set; }

        public DateTime? DtmStartDateTime { get; set; }

        public DateTime? DtmEndDateTime { get; set; }

        public string StrDescription { get; set; }

        public Period DurLength { get; set; }

        public int IntLengthMinutes { get; set; }

        public Rehearsal Rehearsal { get; set; }

        public Types Type { get; set; }

        public Event Event { get; set; }

        public RehearsalPart() {
            LstMembers = new List<Users>();
        }

        public RehearsalPart(int intRehearsalPartID, string strDescription, Types type, Event @event, Period durLength)
        {
            Event = @event;
            DurLength = durLength;
            IntRehearsalPartID = intRehearsalPartID;
            StrDescription = strDescription;
            Type = type;
            Event = @event;
        }
    }
}