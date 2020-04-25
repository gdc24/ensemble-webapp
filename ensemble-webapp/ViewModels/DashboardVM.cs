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
    }
}