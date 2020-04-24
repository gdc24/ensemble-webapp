﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Member
    {

        // this is basically a matrix table for users and events

        public Member(int intMemberID, Event paramEvent, Users user)
        {
            IntMemberID = intMemberID;
            Event = paramEvent;
            User = user;
        }

        public int IntMemberID { get; set; }

        public Event Event { get; set; }

        public Users User { get; set; }
    }
}