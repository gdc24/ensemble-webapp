using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ScheduleHomeVM
    {
        public List<Rehearsal> LstUpcomingRehearsals { get; set; }

        public Users CurrentUser { get; set; }
    }
}