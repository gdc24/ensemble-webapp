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
        

        List<RehearsalPart> LstScheduledRehearsalParts { get; set; }

        List<RehearsalPart> LstUnscheduledRehearsalParts { get; set; }
    }
}