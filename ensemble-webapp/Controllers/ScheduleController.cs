﻿using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class ScheduleController : Controller
    {
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
                model.LstAllRehearsalParts = get.GetAllRehearsalParts();
                get.CloseConnection();

                get.OpenConnection();
                model.LstAdminEvents = get.GetAdminEventsByUser(Globals.LOGGED_IN_USER.IntUserID);
                foreach (RehearsalPart rp in model.LstAllRehearsalParts)
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
        public ActionResult CreateNewSchedule(ScheduleHomeVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            Event e = get.GetEventByID(vm.SelectedEvent.IntEventID);

            List<RehearsalPart> rehearsalParts = get.GetRehearsalPartsByEvent(e);
            foreach (RehearsalPart rp in e.LstRehearsalParts)
            {
                rp.LstMembers = get.GetUsersByRehearsalPart(rp);
            }
            get.CloseConnection();

            //List<RehearsalPart> rehearsalParts = vm.LstAllRehearsalParts.Where(x => x.Event.Equals(e)).ToList();

            Schedule newSchedule = new Schedule(rehearsalParts, e);
            return RedirectToAction("Index");
        }
    }
}