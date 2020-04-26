using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class EventSchedule
    {
        public EventSchedule(int intEventScheduleID, LocalTime tmeMondayStart, LocalTime tmeTuesdayStart, LocalTime tmeWednesdayStart, LocalTime tmeThursdayStart, LocalTime tmeFridayStart, LocalTime tmeSaturdayStart, LocalTime tmeSundayStart, Period durWeekdayDuration, Period durWeekendDuration, Event paramEvent)
        {
            IntEventScheduleID = intEventScheduleID;
            TmeMondayStart = tmeMondayStart;
            TmeTuesdayStart = tmeTuesdayStart;
            TmeWednesdayStart = tmeWednesdayStart;
            TmeThursdayStart = tmeThursdayStart;
            TmeFridayStart = tmeFridayStart;
            TmeSaturdayStart = tmeSaturdayStart;
            TmeSundayStart = tmeSundayStart;
            PerWeekdayDuration = durWeekdayDuration;
            PerWeekendDuration = durWeekendDuration;
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

        public Period PerWeekdayDuration { get; set; }

        public Period PerWeekendDuration { get; set; }

        public int IntWeekdayDuration { get; set; }

        public int IntWeekendDuration { get; set; }

        public Event Event { get; set; }


        public string StrMondayStart { get; set; }

        public string StrTuesdayStart { get; set; }

        public string StrWednesdayStart { get; set; }

        public string StrThursdayStart { get; set; }

        public string StrFridayStart { get; set; }

        public string StrSaturdayStart { get; set; }

        public string StrSundayStart { get; set; }

        public EventSchedule(Event @event, int intMinutesWeekday, int intMinutesWeekend, string strMondayStart, string strTuesdayStart, string strWednesdayStart, string strThursdayStart, string strFridayStart, string strSaturdayStart, string strSundayStart)
        {
            Event = @event;
            PeriodBuilder periodBuilderWeekday = new PeriodBuilder
            {
                Minutes = intMinutesWeekday
            };
            PeriodBuilder periodBuilderWeekend = new PeriodBuilder
            {
                Minutes = intMinutesWeekend
            };

            PerWeekdayDuration = periodBuilderWeekday.Build();
            PerWeekendDuration = periodBuilderWeekend.Build();
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            var pattern = LocalTimePattern.Create("h:mmtt", culture);
            TmeMondayStart = pattern.Parse(strMondayStart).Value;
            TmeTuesdayStart = pattern.Parse(strTuesdayStart).Value;
            TmeWednesdayStart = pattern.Parse(strWednesdayStart).Value;
            TmeThursdayStart = pattern.Parse(strThursdayStart).Value;
            TmeFridayStart = pattern.Parse(strFridayStart).Value;
            TmeSaturdayStart = pattern.Parse(strSaturdayStart).Value;
            TmeSundayStart = pattern.Parse(strSundayStart).Value;
        }

        public EventSchedule() { }
    }
}