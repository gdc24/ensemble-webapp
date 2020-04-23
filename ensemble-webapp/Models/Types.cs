using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Types
    {
        public Types(int intTypeID, string strName, Event paramEvent)
        {
            IntTypeID = intTypeID;
            StrName = strName;
            Event = paramEvent;
        }

        public int IntTypeID { get; set; }

        public string StrName { get; set; }

        public Event Event { get; set; }
    }
}