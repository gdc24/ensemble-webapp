using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class FinalSchedule
    {
        public FinalSchedule(List<RehearsalPart> scheduledRehearsalParts, List<RehearsalPart> unscheduledRehearsalParts)
        {
            LstScheduledRehearsalParts = scheduledRehearsalParts;
            LstUnscheduledRehearsalParts = unscheduledRehearsalParts;
        }
        

        public List<RehearsalPart> LstScheduledRehearsalParts { get; set; }

        public List<RehearsalPart> LstUnscheduledRehearsalParts { get; set; }

        public FinalSchedule() { }
    }
}