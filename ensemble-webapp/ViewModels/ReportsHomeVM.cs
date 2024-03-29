﻿using ensemble_webapp.Models;
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
        
        public String StartTime { get; set; }

        public String EndTime { get; set; }

        public List<AttendancePlanned> PlannedAttendance { get; set; }

        public List<AttendanceActual> ActualAttendance { get; set; }

        public List<RehearsalPart> LstAllRehearsalParts { get; set; }

        public List<Rehearsal> LstAllRehearsals { get; set; }

        public List<Event> LstAllEvents { get; set; }

        public Rehearsal ChosenRehearsal { get; set; }
        
        public String Notes { get; set; }

        public ReportsHomeVM()
        {
            ActualAttendance = new List<AttendanceActual>();
            PlannedAttendance = new List<AttendancePlanned>();
        }
    }
}
