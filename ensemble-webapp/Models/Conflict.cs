using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Conflict
    {
        public Conflict(int intConflictID, DateTime dtmStartDateTime, DateTime dtmEndDateTime, Member member)
        {
            IntConflictID = intConflictID;
            DtmStartDateTime = dtmStartDateTime;
            DtmEndDateTime = dtmEndDateTime;
            Member = member;
        }

        public int IntConflictID { get; set; }

        public DateTime DtmStartDateTime { get; set; }

        public DateTime DtmEndDateTime { get; set; }

        public Member Member { get; set; }
    }
}