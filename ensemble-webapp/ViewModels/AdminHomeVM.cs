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

    }
}