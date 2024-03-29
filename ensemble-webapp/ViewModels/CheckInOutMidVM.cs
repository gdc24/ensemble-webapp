﻿using ensemble_webapp.Models;
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
            ChosenEvent = new Event();
            LstRehearsalParts = new List<RehearsalPart>();
            ChosenRehearsalPart = new RehearsalPart();
        }

        public Event ChosenEvent { get; set; }

        public List<RehearsalPart> LstRehearsalParts { get; set; }

        public RehearsalPart ChosenRehearsalPart { get; set; }
    }
}