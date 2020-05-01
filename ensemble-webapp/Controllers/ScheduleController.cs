using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ensemble_webapp.Controllers
{
    public class ScheduleController : Controller
    {

        public static FinalSchedule tmpRehearsalPartsSchedule = new FinalSchedule();

        // GET: Schedule
        public ActionResult Index()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ScheduleHomeVM model = new ScheduleHomeVM();
                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.LstUpcomingRehearsals = get.GetUpcomingRehearsalsByUser(Globals.LOGGED_IN_USER);

                foreach (var r in model.LstUpcomingRehearsals)
                {
                    r.LstRehearsalParts = get.GetRehearsalPartsByRehearsal(r);
                    foreach (RehearsalPart rp in r.LstRehearsalParts)
                    {
                        rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                    }
                }

                model.LstUserRehearsalParts = get.GetUpcomingRehearsalPartsByUser(Globals.LOGGED_IN_USER);
                model.LstUnscheduledRehearsalParts = model.LstUserRehearsalParts.Where(x => x.DtmStartDateTime.Equals(null)).ToList();

                //foreach (var e in get.GetEventsByUser(Globals.LOGGED_IN_USER.IntUserID))
                //{
                //    get.CloseConnection();
                //    get.OpenConnection();
                //    model.LstUserRehearsalParts = model.LstUserRehearsalParts.Concat(get.GetRehearsalPartsByEvent(e)).ToList();
                //}
                //get.CloseConnection();
                //get.OpenConnection();
                //model.LstUpcomingRehearsalParts = get.GetUpcomingRehearsalPartsByUser(Globals.LOGGED_IN_USER);

                //model.LstUnscheduledRehearsalParts = model.LstUserRehearsalParts.Where(x => x.DtmStartDateTime.Equals(null)).ToList();

                //model.LstUpcomingRehearsalParts = model.LstUpcomingRehearsalParts.Except(model.LstUnscheduledRehearsalParts.ToList()).ToList();

                //model.LstUpcomingRehearsals = get.GetUpcomingRehearsalsByUser(Globals.LOGGED_IN_USER);

                //foreach (var r in model.LstUpcomingRehearsals)
                //{
                //    r.LstRehearsalParts = get.GetRehearsalPartsByRehearsal(r);
                //}

                //get.CloseConnection();
                //get.OpenConnection();
                model.LstAdminEvents = get.GetAdminEventsByUser(Globals.LOGGED_IN_USER.IntUserID);

                //foreach (RehearsalPart rp in model.LstUserRehearsalParts)
                //{
                //    rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                //}
                //foreach (RehearsalPart rp in model.LstUpcomingRehearsalParts)
                //{
                //    rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                //}
                get.CloseConnection();

                return View("ScheduleHome", model);
            }
        }

        public ActionResult ScheduleHome()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ConfirmSingleRehearsal(string strLocation, string strNotes, DateTime dtmStart, DateTime dtmEnd)
        {
            Rehearsal newRehearsal = new Rehearsal
            {
                DtmStartDateTime = dtmStart,
                DtmEndDateTime = dtmEnd,
                StrLocation = strLocation,
                StrNotes = strNotes
            };
            newRehearsal.LstRehearsalParts = tmpRehearsalPartsSchedule.LstScheduledRehearsalParts.Where(x => x.DtmStartDateTime.GetValueOrDefault().Date.Equals(newRehearsal.DtmStartDateTime.Date)).ToList();

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            if (insert.InsertRehearsal(newRehearsal))
                return Json(new { success = true, responseTest = "confirmed!" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseTest = "error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConfirmSchedule()
        {

            // update rehearsal part times
            //vm.LstConfirmScheduledRehearsalParts = tmpRehearsalPartsSchedule.LstScheduledRehearsalParts;
            // insert start and end dates for each rehearsal part into db
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();
            foreach (var rp in tmpRehearsalPartsSchedule.LstScheduledRehearsalParts)
            {
                insert.UpdateRPTimes(rp.IntRehearsalPartID, rp.DtmStartDateTime.GetValueOrDefault(), rp.DtmEndDateTime.GetValueOrDefault());
            }

            insert.CloseConnection();

            return Json(new { success = true, response = "confirmed!" }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("Index", "Schedule");
        }

        [HttpPost]
        public ActionResult CreateNewSchedule(ScheduleHomeVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            Event e = get.GetEventByID(vm.SelectedEvent.IntEventID);

            List<RehearsalPart> rehearsalParts = get.GetRehearsalPartsByEvent(e);
            foreach (RehearsalPart rp in rehearsalParts)
            {
                rp.LstMembers = get.GetUsersByRehearsalPart(rp);
            }
            get.CloseConnection();

            //List<RehearsalPart> rehearsalParts = vm.LstAllRehearsalParts.Where(x => x.Event.Equals(e)).ToList();

            Schedule newSchedule = new Schedule(rehearsalParts, e);

            ScheduleViewVM model = new ScheduleViewVM();
            model.Schedule = newSchedule.FinalSchedule;
            tmpRehearsalPartsSchedule = model.Schedule;

            foreach (LocalDate d in uniqueDatesOfRehearsals(model.Schedule.LstScheduledRehearsalParts))
            {
                Rehearsal tmpRehearsal = new Rehearsal();
                // get earliest rehearsal on any day
                DateTime fromDateOnly = new DateTime(d.Year, d.Month, d.Day);
                // get start of earliest rehearsal part on that day
                RehearsalPart earliest = model.Schedule.LstScheduledRehearsalParts.Where(x => x.DtmStartDateTime.GetValueOrDefault().Date.Equals(fromDateOnly)).OrderBy(x => x.DtmStartDateTime.GetValueOrDefault()).FirstOrDefault();
                tmpRehearsal.DtmStartDateTime = earliest.DtmStartDateTime.GetValueOrDefault();

                // get end of latest rehearsal part on that day
                RehearsalPart lastest = model.Schedule.LstScheduledRehearsalParts.Where(x => x.DtmEndDateTime.GetValueOrDefault().Date.Equals(fromDateOnly)).OrderByDescending(x => x.DtmEndDateTime.GetValueOrDefault()).FirstOrDefault();
                tmpRehearsal.DtmEndDateTime = lastest.DtmEndDateTime.GetValueOrDefault();

                tmpRehearsal.LstRehearsalParts = model.Schedule.LstScheduledRehearsalParts.Where(x => x.DtmStartDateTime.GetValueOrDefault().Date.Equals(fromDateOnly)).ToList();

                model.LstTmpRehearsals.Add(tmpRehearsal);
            }

            return View("ScheduleView", model);
        }

        private List<LocalDate> uniqueDatesOfRehearsals(List<RehearsalPart> rehearsalParts)
        {
            List<LocalDate> retval = new List<LocalDate>();
            foreach (RehearsalPart rp in rehearsalParts)
            {
                LocalDate dateOnly = LocalDate.FromDateTime(rp.DtmStartDateTime.Value.Date);
                if (!retval.Contains(dateOnly)) {
                    retval.Add(dateOnly);
                };
            }

            return retval;
        }

        public ActionResult ScheduleView()
        {
            return RedirectToAction("Index");
        }

        //public ActionResult ViewSchedule(FinalSchedule schedule)
        //{
        //    if (!Globals.LOGIN_STATUS)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    else
        //    {
        //        ScheduleViewVM model = new ScheduleViewVM();
        //        model.Schedule = schedule;
    }
}