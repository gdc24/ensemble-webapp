using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ProfileHomeVM
    {
        public Users EditedUserProfile { get; set; }

        public Users CurrentUser { get; set; }

        public List<Event> LstAllEvents { get; set; }

    }
}