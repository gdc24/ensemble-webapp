using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class AttendanceActual
    {

        public AttendanceActual(int intAttendanceActualID, DateTime dtmInTime, DateTime dtmOutTime, bool ysnDidShow, AttendancePlanned attendancePlanned)
        {
            IntAttendanceActualID = intAttendanceActualID;
            DtmInTime = dtmInTime;
            DtmOutTime = dtmOutTime;
            YsnDidShow = ysnDidShow;
            AttendancePlanned = attendancePlanned;
        }

        public AttendanceActual(DateTime dtmInTime, bool ysnDidShow, AttendancePlanned attendancePlanned)
        {
            DtmInTime = dtmInTime;
            YsnDidShow = ysnDidShow;
            AttendancePlanned = attendancePlanned;
        }

        public int IntAttendanceActualID { get; set; }

        public DateTime DtmInTime { get; set; }

        public DateTime DtmOutTime { get; set; }

        public bool YsnDidShow { get; set; }

        public AttendancePlanned AttendancePlanned { get; set; }
    }
}