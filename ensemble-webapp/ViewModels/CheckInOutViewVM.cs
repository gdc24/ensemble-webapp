using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class CheckInOutViewVM
    {

        public CheckInOutViewVM() 
        {
            List<Users> UsersNotCurrentlyAtRehearsal = new List<Users>();
            List<Users> UsersCurrentlyAtRehearsal = new List<Users>();
            RehearsalPart CurrentRehearsalPart = new RehearsalPart();
        }

        public List<Users> UsersNotCurrentlyAtRehearsal { get; set; }

        public List<Users> UsersCurrentlyAtRehearsal { get; set; }

        public RehearsalPart CurrentRehearsalPart { get; set; }
    }
}