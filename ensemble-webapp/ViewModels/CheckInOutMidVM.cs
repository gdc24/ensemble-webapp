using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class CheckInOutMidVM
    {

        public CheckInOutMidVM()
        {
            CurrentEvent = new Event();
            Lst_RehearsalParts = new List<RehearsalPart>();
            Chosen_RehearsalPart = new RehearsalPart();
        }

        public Event CurrentEvent { get; set; }

        public List<RehearsalPart> Lst_RehearsalParts { get; set; }

        public RehearsalPart Chosen_RehearsalPart { get; set; }
    }
}