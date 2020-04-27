using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class DashboardVM
    {
        public Users CurrentUser { get; set; }

        public List<Task> LstUpcomingTasks { get; set; }

        public List<Event> LstEvents { get; set; }

        public List<Rehearsal> LstUpcomingRehearsals { get; set; }

        public List<Task> LstOverdueTasks { get; set; }
        public List<RehearsalPart> LstUserRehearsalParts { get; set; }
        public List<RehearsalPart> LstUpcomingRehearsalParts { get; internal set; }
        public List<RehearsalPart> LstUnscheduledRehearsalParts { get; internal set; }

        public DashboardVM()
        {
            LstUpcomingRehearsals = new List<Rehearsal>();
            LstUserRehearsalParts = new List<RehearsalPart>();
            LstUpcomingRehearsalParts = new List<RehearsalPart>();
            LstUnscheduledRehearsalParts = new List<RehearsalPart>();
        }
    }
}