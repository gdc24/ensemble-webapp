using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class AttendancePlanned
    {

        public AttendancePlanned(int intAttendancePlannedID, RehearsalPart rehearsalPart, Users user)
        {
            IntAttendancePlannedID = intAttendancePlannedID;
            RehearsalPart = rehearsalPart;
            User = user;
        }

        public int IntAttendancePlannedID { get; set; }

        public RehearsalPart RehearsalPart { get; set; }

        public Users User { get; set; }
    }
}