using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Part
    {
        public Part(int intPartID, string strRole, Users user, Event paramEvent)
        {
            IntPartID = intPartID;
            StrRole = strRole;
            User = user;
            Event = paramEvent;
        }

        public int IntPartID { get; set; }

        public string StrRole { get; set; }

        public Users User { get; set; }

        public Event Event { get; set; }
    }
}