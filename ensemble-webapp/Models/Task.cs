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

        public override bool Equals(object obj)
        {
            return obj is Task task &&
                   IntTaskID == task.IntTaskID;
        }

        public Task() { }

    }

    class TaskEqualityComparer : IEqualityComparer<Task>
    {
        public bool Equals(Task x, Task y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Task obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.IntTaskID.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.IntTaskID.GetHashCode();
                return hashCode;
            }
        }
    }
}