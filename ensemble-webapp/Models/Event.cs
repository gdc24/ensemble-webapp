﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Event
    {

        public Event(int intEventID, string strName, DateTime dtmDate, string strLocation, Group group)
        {
            IntEventID = intEventID;
            StrName = strName;
            DtmDate = dtmDate;
            StrLocation = strLocation;
            Group = group;
        }

        public int IntEventID { get; set; }

        public string StrName { get; set; }

        //TODO make this a list for recurring events
        public DateTime DtmDate { get; set; }

        public string StrLocation { get; set; }

        public Group Group { get; set; }

        public Event() { }

        public bool Equals(Event other)
        {
            return IntEventID == other.IntEventID;
        }
    }


    class EventEqualityComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event x, Event y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Event obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.IntEventID.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.IntEventID.GetHashCode();
                return hashCode;
            }
        }
    }
}