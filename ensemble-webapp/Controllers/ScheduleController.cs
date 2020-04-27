using ensemble_webapp.Database;
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
            foreach (RehearsalPart rp in rehearsalParts)
            {
                rp.LstMembers = get.GetUsersByRehearsalPart(rp);
            }
            get.CloseConnection();

            //List<RehearsalPart> rehearsalParts = vm.LstAllRehearsalParts.Where(x => x.Event.Equals(e)).ToList();

            Schedule newSchedule = new Schedule(rehearsalParts, e);

            ScheduleViewVM model = new ScheduleViewVM();
            model.Schedule = newSchedule.FinalSchedule;

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
        public ActionResult CheckUserIn(ScheduleHomeVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();
 
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(vm.CurrentRehearsalPart))
            {
                if (p.User.Equals(vm.UsersToCheckInOut))
                {
                    insert.InsertAttendanceActual(new AttendanceActual(1, DateTime.Now, DateTime.Now, true, p)); 
                    // when first inserting, they're only there for that millisecond 
                    // also idk how to do incrementing attendanceActualID
                }
            }

            insert.CloseConnection();
            get.CloseConnection();
            return RedirectToAction("Index");
        }

        //public ActionResult CheckUserOut(ScheduleHomeVM vm)
        //{
        //    GetDAL get = new GetDAL();
        //    get.OpenConnection();

        //    InsertDAL insert = new InsertDAL();
        //    insert.OpenConnection();

        //    foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(vm.CurrentRehearsalPart))
        //    {
        //        if (p.User.Equals(vm.UsersToCheckInOut))
        //        {
        //            AttendanceActual a = get.GetAttendanceActualByRehearsalPartAndUser(p.User, vm.CurrentRehearsalPart); // need to implement this method
        //            a.DtmOutTime = DateTime.Now;
        //            insert.InsertAttendanceActual(a);
        //        }
        //    }

        //    insert.CloseConnection();
        //    get.CloseConnection();
        //    return RedirectToAction("Index");
        //}
    }
}