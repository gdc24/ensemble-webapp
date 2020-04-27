using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class CheckInOutController : Controller
    {
        // GET: CheckInOut
        public ActionResult Index()
        {
            if (!Globals.IS_ADMIN)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                CheckInMembersVM model = new CheckInMembersVM();

                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.LstAdminEvents = get.GetAdminEventsByUser(Globals.LOGGED_IN_USER.IntUserID);
                foreach (Event e in model.LstAdminEvents)
                {
                    e.LstRehearsalParts = get.GetRehearsalPartsByEvent(e);
                    foreach (RehearsalPart rp in e.LstRehearsalParts)
                    {
                        rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                    }
                    e.MembersForToday = LstAllMembersForRehearsalParts(e, get);
                }
                get.CloseConnection();

                return View("CheckInMembers", model);
            }
        }

        public ActionResult CheckInOutHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult CheckUserIn(CheckInMembersVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(vm.CurrentRehearsalPart))
            {
                if (p.User.Equals(vm.ChosenEvent.MembersForToday))
                {
                    insert.InsertAttendanceActual(new AttendanceActual(1, DateTime.Now, DateTime.Now, true, p));
                    // when first inserting, they're only there for that millisecond 
                    // also idk how to do incrementing attendanceActualID
                }
            }

            //update users to check out to users checked in

            insert.CloseConnection();
            get.CloseConnection();
            return RedirectToAction("Index");
        }

        //public ActionResult CheckUserOut(CheckInMembersVM vm)
        //{
            //GetDAL get = new GetDAL();
            //get.OpenConnection();

            //InsertDAL insert = new InsertDAL();
            //insert.OpenConnection();

            //foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(vm.CurrentRehearsalPart))
            //{
                //if (p.User.Equals(vm..ChosenEvent.MembersForToday))
                //{
                    //AttendanceActual a = get.GetAttendanceActualByRehearsalPartAndUser(p.User, vm.CurrentRehearsalPart); // need to implement this method
                    //a.DtmOutTime = DateTime.Now;
                    //insert.InsertAttendanceActual(a);
                //}
            //}
            
            //update users to check in to users checked out

            //insert.CloseConnection();
            //get.CloseConnection();
            //return RedirectToAction("Index");
        //}

        private List<Users> LstAllMembersForRehearsalParts(Event e, GetDAL connection)
        {
            List<Users> retval = new List<Users>();
            // go through each rehearsal part's list of members
            List<RehearsalPart> today = e.LstRehearsalParts.Where(x => x.DtmStartDateTime.GetValueOrDefault().Date.Equals(DateTime.Now.Date)).ToList();
            foreach (RehearsalPart rp in today)
            {
                retval = retval.Concat(rp.LstMembers.Where(x => !retval.Any(y => y.Equals(x)))).ToList();
            }
            //GetDAL get = new GetDAL();
            //get.OpenConnection();
            foreach (Users m in retval)
            {
                m.TimeScheduled = connection.GetFirstTimeByDayAndUser(DateTime.Now.Date, m);
            }
            //get.CloseConnection();
            return retval;
        }
    }
}