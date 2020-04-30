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
        public static Event ChosenEvent = new Event();

        public static List<RehearsalPart> LstRehearsalParts = new List<RehearsalPart>();

        public static RehearsalPart ChosenRehearsalPart = new RehearsalPart();

        public static List<Users> UsersNotCurrentlyAtRehearsal = new List<Users>();

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
                CheckInOutVM model = new CheckInOutVM();

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

                return View("Index", model);
            }
        }

        public ActionResult CheckInOutHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult CheckInOutVM()
        {
            return View("Index");
        }

        public ActionResult CheckInOutMid()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChooseEvent(CheckInOutVM c)
        {
            GetDAL get = new GetDAL();

            get.OpenConnection();
            ChosenEvent = get.GetEventByID(c.ChosenEvent.IntEventID);
            LstRehearsalParts = get.GetRehearsalPartsByEvent(c.ChosenEvent);

            CheckInOutMidVM model = new CheckInOutMidVM
            {
                ChosenEvent = ChosenEvent,
                LstRehearsalParts = LstRehearsalParts
            };

            get.CloseConnection();

            return View("CheckInOutMid", model);
        }

        [HttpPost]
        public ActionResult ChooseRehearsalPart(CheckInOutMidVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();
            ChosenRehearsalPart = get.GetRehearsalPartByID(vm.ChosenRehearsalPart.IntRehearsalPartID);
            UsersNotCurrentlyAtRehearsal = get.GetUsersByRehearsalPart(ChosenRehearsalPart);

            CheckInOutViewVM model = new CheckInOutViewVM
            {
                CurrentRehearsalPart = ChosenRehearsalPart,
                UsersNotCurrentlyAtRehearsal = UsersNotCurrentlyAtRehearsal
            };

            get.CloseConnection();

            return View("CheckInOutView", model);
        }

        public ActionResult CheckInOutView()
        {
            return RedirectToAction("Index");
        }

        public ActionResult CheckUserIn(int intUserID)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            Users u = get.GetUserByID(intUserID);

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            foreach (AttendancePlanned ap in get.GetAttendancePlannedByRehearsalPart(ChosenRehearsalPart))
            {
                if (u.Equals(ap.User))
                {
                    insert.InsertAttendanceActual(new AttendanceActual(DateTime.Now, true, ap));
                    UsersNotCurrentlyAtRehearsal.Remove(u);
                }
            }
            
            insert.CloseConnection();
            get.CloseConnection();
            return RedirectToAction("Index");
        }

        /*public ActionResult CheckUserOut(CheckInOutViewVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(vm.CurrentRehearsalPart))
            {
                foreach (AttendanceActual a in get.GetAttendanceActualByRehearsalPart(vm.CurrentRehearsalPart))
                {
                    if (a.AttendancePlanned.User.Equals(p.User) && vm.UsersCurrentlyAtRehearsal.Contains(p.User))
                    {
                        a.DtmOutTime = DateTime.Now;
                        vm.UsersCurrentlyAtRehearsal.Remove(p.User);
                        vm.UsersNotCurrentlyAtRehearsal.Add(p.User);
                        insert.InsertAttendanceActual(a);
                    }
                }
            }

            insert.CloseConnection();
            get.CloseConnection();
            return RedirectToAction("Index", vm);
        }
        */

        /*public ActionResult FinishRehearsal(CheckInOutViewVM vm)
        {
            GetDAL get = new GetDAL();
            get.OpenConnection();

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();


            foreach (AttendancePlanned p in get.GetAttendancePlannedByRehearsalPart(get.GetRehearsalPartByID(vm.CurrentRehearsalPart.IntRehearsalPartID)))
            {
                foreach (AttendanceActual a in get.GetAttendanceActualByPlanned(p))
                {
                    a.DtmOutTime = vm.CurrentRehearsalPart.DtmEndDateTime;
                    insert.InsertAttendanceActual(a);
                }
            }

            insert.CloseConnection();

            get.CloseConnection();

            return RedirectToAction("Index");
        } */

        private List<Users> LstAllMembersForRehearsalParts(Event e, GetDAL connection)
        {
            List<Users> retval = new List<Users>();

            // go through each rehearsal part's list of members
            List<RehearsalPart> today = e.LstRehearsalParts.Where(x => x.DtmStartDateTime.GetValueOrDefault().Date.Equals(DateTime.Now.Date)).ToList();
            foreach (RehearsalPart rp in today)
            {
                retval = retval.Concat(rp.LstMembers.Where(x => !retval.Any(y => y.Equals(x)))).ToList();
            }

            foreach (Users m in retval)
            {
                m.TimeScheduled = connection.GetFirstTimeByDayAndUser(DateTime.Now.Date, m);
            }

            return retval;
        }
    }
}