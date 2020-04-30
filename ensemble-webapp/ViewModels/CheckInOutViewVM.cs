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
            Users UserToCheckInOut = new Users();
            RehearsalPart CurrentRehearsalPart = new RehearsalPart();
        }

        public Users UserToCheckInOut { get; set; }

        public List<Users> UsersNotCurrentlyAtRehearsal { get; set; }

        public RehearsalPart CurrentRehearsalPart { get; set; }
    }
}