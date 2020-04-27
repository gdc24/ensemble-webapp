using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class CheckInMembersVM
    {

        public Event ChosenEvent { get; set; }

        public List<Event> LstAdminEvents { get; set; }

        public RehearsalPart CurrentRehearsalPart { get; set; }

        public List<Users> UsersFromForm { get; set; }

        public List<Users> UsersNotCurrentlyAtRehearasl { get; set; }

        public List<Users> UsersCurrentlyAtRehearasl { get; set; }
    }
}