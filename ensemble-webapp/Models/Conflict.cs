using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Conflict
    {
        public Conflict(int intConflictID, DateTime dtmStartDateTime, DateTime dtmEndDateTime, Users user)
        {
            IntConflictID = intConflictID;
            DtmStartDateTime = dtmStartDateTime;
            DtmEndDateTime = dtmEndDateTime;
            User = user;
        }

        public int IntConflictID { get; set; }

        public DateTime DtmStartDateTime { get; set; }

        public DateTime DtmEndDateTime { get; set; }

        public Users User { get; set; }

        public Conflict() { }
    }
}