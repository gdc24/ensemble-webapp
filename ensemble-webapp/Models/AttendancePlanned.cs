using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class AttendancePlanned
    {

        public AttendancePlanned(int intAttendancePlannedID, RehearsalPart rehearsalPart, Member member)
        {
            IntAttendancePlannedID = intAttendancePlannedID;
            RehearsalPart = rehearsalPart;
            Member = member;
        }

        public int IntAttendancePlannedID { get; set; }

        public RehearsalPart RehearsalPart { get; set; }

        public Member Member { get; set; }
    }
}