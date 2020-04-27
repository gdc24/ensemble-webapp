using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ScheduleHomeVM
    {
        public List<RehearsalPart> LstUpcomingRehearsalParts { get; set; }

        public List<RehearsalPart> LstUserRehearsalParts { get; set; }

        public List<Rehearsal> LstUpcomingRehearsals { get; set; }

        public List<RehearsalPart> LstUnscheduledRehearsalParts { get; set; }

        public Users CurrentUser { get; set; }

        public List<Event> LstAdminEvents { get; set; }

        public Event SelectedEvent { get; set; }

        // have a drop down list
        public RehearsalPart CurrentRehearsalPart { get; set; }

        // from list of checkboxes on html form
        public List<Users> UsersToCheckInOut { get; set; }

        public ScheduleHomeVM()
        {
            LstUserRehearsalParts = new List<RehearsalPart>();
        }
    }
}