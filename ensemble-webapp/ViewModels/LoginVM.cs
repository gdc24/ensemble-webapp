using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class LoginVM
    {
        public Users newUser { get; set; }

        public Users logInUser { get; set; }

        public List<Event> LstAllEvents { get; set; }

        public Event SelectEvent { get; set; }
    }
}