using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class AdminHomeVM
    {
        public Group NewGroup { get; set; }

        public Event NewEvent { get; set; }

        public List<Group> LstAllGroups { get; set; }

        public List<Event> LstAllEvents { get; set; }

        public EventSchedule NewEventSchedule { get; set; }

        public Types NewType { get; set; }

        public List<Types> LstAllTypes { get; set; }

        public RehearsalPart NewRehearsalPart { get; set; }

        public List<Event> LstAdminEvents { get; set; }
        public List<Users> LstAllUsersForAdminEvents { get; internal set; }

        public AdminHomeVM() { }

    }
}