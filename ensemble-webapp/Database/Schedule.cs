using ensemble_webapp.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Database
{
    public class Schedule
    {

        List<RehearsalPart> UnscheduledRehearsalParts { get; set; }

        public FinalSchedule FinalSchedule { get; set; }

        EventSchedule EventSchedule { get; set; }

        public Schedule(List<RehearsalPart> allRehearsalParts, Event @event)
        {
            UnscheduledRehearsalParts = allRehearsalParts;
            GetDAL get = new GetDAL();
            get.OpenConnection();
            EventSchedule = get.GetEventScheduleByEvent(@event.IntEventID);
            get.CloseConnection();

            CreateSchedule(DateTime.Now.AddDays(1), @event.DtmDate);
        }

        public FinalSchedule CreateSchedule(DateTime startDate, DateTime eventDate)
        {
            // starting at the rehearsal start date
            List<DateTime> rehearsalDates = Enumerable.Range(0, 1 + eventDate.Subtract(startDate).Days)
                                                      .Select(offset => startDate.AddDays(offset))
                                                      .ToList();

            List<RehearsalPart> scheduledRehearsalParts = new List<RehearsalPart>();

            foreach (var day in rehearsalDates)
            {
                if (UnscheduledRehearsalParts.Any())
                {
                    switch (day.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            DateTime monday = new DateTime(day.Year, day.Month, day.Day,
                                                           EventSchedule.TmeMondayStart.Hour,
                                                           EventSchedule.TmeMondayStart.Minute,
                                                           EventSchedule.TmeMondayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(monday, EventSchedule.PerWeekdayDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Tuesday:
                            DateTime tuesday = new DateTime(day.Year, day.Month, day.Day,
                                                            EventSchedule.TmeTuesdayStart.Hour,
                                                            EventSchedule.TmeTuesdayStart.Minute,
                                                            EventSchedule.TmeTuesdayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(tuesday, EventSchedule.PerWeekdayDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Wednesday:
                            DateTime wednesday = new DateTime(day.Year, day.Month, day.Day,
                                                              EventSchedule.TmeWednesdayStart.Hour,
                                                              EventSchedule.TmeWednesdayStart.Minute,
                                                              EventSchedule.TmeWednesdayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(wednesday, EventSchedule.PerWeekdayDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Thursday:
                            DateTime thursday = new DateTime(day.Year, day.Month, day.Day,
                                                             EventSchedule.TmeThursdayStart.Hour,
                                                             EventSchedule.TmeThursdayStart.Minute,
                                                             EventSchedule.TmeThursdayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(thursday, EventSchedule.PerWeekdayDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Friday:
                            DateTime friday = new DateTime(day.Year, day.Month, day.Day,
                                                           EventSchedule.TmeFridayStart.Hour,
                                                           EventSchedule.TmeFridayStart.Minute,
                                                           EventSchedule.TmeFridayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(friday, EventSchedule.PerWeekdayDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Saturday:
                            DateTime saturday = new DateTime(day.Year, day.Month, day.Day,
                                                             EventSchedule.TmeSaturdayStart.Hour,
                                                             EventSchedule.TmeSaturdayStart.Minute,
                                                             EventSchedule.TmeSaturdayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(saturday, EventSchedule.PerWeekendDuration.ToDuration())).ToList();
                            break;
                        case DayOfWeek.Sunday:
                            DateTime sunday = new DateTime(day.Year, day.Month, day.Day,
                                                           EventSchedule.TmeSundayStart.Hour,
                                                           EventSchedule.TmeSundayStart.Minute,
                                                           EventSchedule.TmeSundayStart.Second);
                            scheduledRehearsalParts = scheduledRehearsalParts.Concat(ScheduleDay(sunday, EventSchedule.PerWeekendDuration.ToDuration())).ToList();
                            break;
                    }
                }
            }

            FinalSchedule = new FinalSchedule(scheduledRehearsalParts, UnscheduledRehearsalParts);
            return FinalSchedule;
        }

        private List<RehearsalPart> ScheduleDay(DateTime start, Duration length)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();
            //DateTime end = start.AddMinutes(length.TotalMinutes);
            Duration totalRehearsalTimeForDay = Duration.Zero;

            DateTime rehearsalPartStart = start;
            DateTime rehearsalPartEnd = rehearsalPartStart;

            foreach (RehearsalPart rp in UnscheduledRehearsalParts.ToArray())
            {
                double minutesRehearsalPartLength = rp.DurLength.ToDuration().TotalMinutes;
                rehearsalPartEnd = rehearsalPartEnd.AddMinutes(minutesRehearsalPartLength);
                // if the list of needed members do not have conflicts between start and end
                // AND the duration of the rehearsal part plus total time so far is less than the max length of the rehearsal
                bool hasConflicts = HasConflicts(rp.LstMembers, rehearsalPartStart, rehearsalPartEnd);
                bool rehearsalLengthFits = rp.DurLength.ToDuration().Plus(totalRehearsalTimeForDay) < length;
                if (!hasConflicts && rehearsalLengthFits)
                {
                    // start rehearsal part at given time
                    rp.DtmStartDateTime = rehearsalPartStart;
                    // end rehearsal part at given time
                    rp.DtmEndDateTime = rehearsalPartStart.AddMinutes(minutesRehearsalPartLength);
                    // move new rehearsal part start time to end
                    rehearsalPartStart = (DateTime)rp.DtmEndDateTime;
                    // reset rehearsal part end time to new start time
                    rehearsalPartEnd = rehearsalPartStart;
                    // add to total rehearsal time for day 
                    totalRehearsalTimeForDay = totalRehearsalTimeForDay.Plus(Duration.FromMinutes(minutesRehearsalPartLength));


                    // remove from unschedule rehearsal parts list
                    UnscheduledRehearsalParts.Remove(rp);

                    // add this rehearsal part to the list of scheduled parts
                    retval.Add(rp);
                }
            }
            return retval;
        }

        /// <summary>
        /// Returns true if at least one of the users has a conflict. Returns false if none of the users have a conflict for the given time
        /// </summary>
        /// <param name="LstMembers">List of members needed at that rehearsal</param>
        /// <param name="start">start time of rehearsal part</param>
        /// <param name="end">end time of rehearsal part</param>
        /// <returns>True if at least one of the users has a conflict</returns>
        private bool HasConflicts(List<Users> LstMembers, DateTime start, DateTime end)
        {
            GetDAL get = new GetDAL();
            foreach (Users m in LstMembers)
            {
                List<Conflict> conflicts = get.GetConflictsByUserAndDay(m, new LocalDate(start.Year, start.Month, start.Day));
                // as soon as we find someone with a conflict, return true:/
                if ((conflicts.Exists(c => c.DtmEndDateTime > start) &&
                     conflicts.Exists(c => c.DtmStartDateTime < end)) ||
                    (conflicts.Exists(c => c.DtmStartDateTime < end) &&
                     conflicts.Exists(c => c.DtmEndDateTime > start)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}