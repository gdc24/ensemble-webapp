using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ReportsHomeVM
    {
        public String EventName { get; set; }

        public String RehearsalDate { get; set; }
        
        public String GroupName { get; set; }
        
        public String Location { get; set; }
        
        public String StartToEnd { get; set; }

        public AttendancePlanned PlannedAttendance { get; set; }

        public AttendanceActual ActualAttendance { get; set; }
        
        public String Notes { get; set; }
    }
}
