using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ScheduleViewVM
    {

        public FinalSchedule Schedule { get; set; }

        public List<Rehearsal> LstTmpRehearsals { get; set; }

        public List<RehearsalPart> LstConfirmScheduledRehearsalParts { get; set; }

        public Rehearsal SingleConfirmedRehearsal { get; set; }

        public ScheduleViewVM() {
            LstTmpRehearsals = new List<Rehearsal>();
        }

    }
}