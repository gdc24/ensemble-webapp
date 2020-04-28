using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class CheckInOutVM
    {

        public Event ChosenEvent { get; set; }

        public List<Event> LstAdminEvents { get; set; }

    }
}