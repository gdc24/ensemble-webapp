using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class EventSchedule
    {
        public EventSchedule(int intEventScheduleID, LocalTime tmeMondayStart, LocalTime tmeTuesdayStart, LocalTime tmeWednesdayStart, LocalTime tmeThursdayStart, LocalTime tmeFridayStart, LocalTime tmeSaturdayStart, LocalTime tmeSundayStart, Duration durWeekdayDuration, Duration durWeekendDuration, Event paramEvent)
        {
            IntEventScheduleID = intEventScheduleID;
            TmeMondayStart = tmeMondayStart;
            TmeTuesdayStart = tmeTuesdayStart;
            TmeWednesdayStart = tmeWednesdayStart;
            TmeThursdayStart = tmeThursdayStart;
            TmeFridayStart = tmeFridayStart;
            TmeSaturdayStart = tmeSaturdayStart;
            TmeSundayStart = tmeSundayStart;
            DurWeekdayDuration = durWeekdayDuration;
            DurWeekendDuration = durWeekendDuration;
            Event = paramEvent;
        }

        public int IntEventScheduleID { get; set; }

        public LocalTime TmeMondayStart { get; set; }

        public LocalTime TmeTuesdayStart { get; set; }

        public LocalTime TmeWednesdayStart { get; set; }

        public LocalTime TmeThursdayStart { get; set; }

        public LocalTime TmeFridayStart { get; set; }

        public LocalTime TmeSaturdayStart { get; set; }

        public LocalTime TmeSundayStart { get; set; }

        public Duration DurWeekdayDuration { get; set; }

        public Duration DurWeekendDuration { get; set; }

        public Event Event { get; set; }
    }
}