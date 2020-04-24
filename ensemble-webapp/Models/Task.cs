using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Task
    {
        public Task(int intTaskID, DateTime dtmDue, string strName, string strAttachment, Users userAssignedTo, Users userAssignedBy, Event paramEvent)
        {
            IntTaskID = intTaskID;
            DtmDue = dtmDue;
            StrName = strName;
            StrAttachment = strAttachment;
            UserAssignedTo = userAssignedTo;
            UserAssignedBy = userAssignedBy;
            Event = paramEvent;
        }

        public int IntTaskID { get; set; }

        public DateTime DtmDue { get; set; }

        public string StrName { get; set; }

        public string StrAttachment { get; set; }

        public Users UserAssignedTo { get; set; }

        public Users UserAssignedBy { get; set; }

        public Event Event { get; set; }
    }
}