﻿using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
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

                foreach (var e in get.GetEventsByUser(Globals.LOGGED_IN_USER.IntUserID))
                {
                    get.CloseConnection();
                    get.OpenConnection();
                    model.LstUserRehearsalParts = model.LstUserRehearsalParts.Concat(get.GetRehearsalPartsByEvent(e)).ToList();
                }
                get.CloseConnection();
                get.OpenConnection();
                model.LstUpcomingRehearsalParts = get.GetUpcomingRehearsalPartsByUser(Globals.LOGGED_IN_USER);

                model.LstUnscheduledRehearsalParts = model.LstUserRehearsalParts.Where(x => x.DtmStartDateTime.Equals(null)).ToList();

                model.LstUpcomingRehearsalParts = model.LstUpcomingRehearsalParts.Except(model.LstUnscheduledRehearsalParts.ToList()).ToList();

                model.LstUpcomingRehearsals = get.GetUpcomingRehearsalsByUser(Globals.LOGGED_IN_USER);

                foreach (var r in model.LstUpcomingRehearsals)
                {
                    r.LstRehearsalParts = get.GetRehearsalPartsByRehearsal(r);
                }

                get.CloseConnection();
                get.OpenConnection();
                model.LstAdminEvents = get.GetAdminEventsByUser(Globals.LOGGED_IN_USER.IntUserID);

                foreach (RehearsalPart rp in model.LstUserRehearsalParts)
                {
                    rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                }
                foreach (RehearsalPart rp in model.LstUpcomingRehearsalParts)
                {
                    rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                }
                get.CloseConnection();

                return View("ScheduleHome", model);
            }
        }

        public ActionResult ScheduleHome()
        {
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult ConfirmSchedule(ScheduleViewVM vm)
        {
            vm.LstConfirmScheduledRehearsalParts = tmpRehearsalPartsSchedule.LstScheduledRehearsalParts;
            // insert start and end dates for each rehearsal part into db
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();
            foreach (var rp in vm.LstConfirmScheduledRehearsalParts)
            {
                insert.UpdateRPTimes(rp.IntRehearsalPartID, rp.DtmStartDateTime.GetValueOrDefault(), rp.DtmEndDateTime.GetValueOrDefault());
            }

            insert.CloseConnection();

            return RedirectToAction("Index", "Schedule");
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

            return View("ScheduleView", model);
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