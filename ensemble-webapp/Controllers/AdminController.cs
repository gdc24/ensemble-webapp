using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Globals.IS_ADMIN)
            {
                AdminHomeVM model = new AdminHomeVM();

                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.LstAllGroups = get.GetAllGroups();
                model.LstAllEvents = get.GetAllEvents();
                model.LstAdminEvents = get.GetAdminEventsByUser(Globals.LOGGED_IN_USER.IntUserID);
                foreach (Event e in model.LstAdminEvents)
                {
                    e.LstRehearsalParts = get.GetRehearsalPartsByEvent(e);
                    foreach(RehearsalPart rp in e.LstRehearsalParts)
                    {
                        rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                    }
                }
                model.LstAllTypes = get.GetAllTypes();

                model.LstAllUsersForAdminEvents = get.GetAllUsersForAdminEvents(model.LstAdminEvents);
                get.CloseConnection();
                get.OpenConnection();
                foreach (Users u in model.LstAllUsersForAdminEvents)
                {
                    u.LstConflicts = get.GetConflictsByUser(u).OrderBy(x => x.DtmStartDateTime).ToList();
                }

                get.CloseConnection();

                return View("AdminHome", model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CheckInMembers()
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

        public ActionResult AdminHome()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddGroup(AdminHomeVM vm)
        {
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.InsertGroup(vm.NewGroup);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEvent(AdminHomeVM vm)
        {
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.InsertEvent(vm.NewEvent);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEventSchedule(AdminHomeVM vm)
        {
            InsertDAL insert = new InsertDAL();
            //vm.NewEventSchedule.PerWeekdayDuration = NodaTime.Period.FromMinutes(vm.NewEventSchedule.PerWeekdayDuration.Minutes);
            //vm.NewEventSchedule.PerWeekendDuration = NodaTime.Period.FromMinutes(vm.NewEventSchedule.PerWeekendDuration.Minutes);

            EventSchedule newEventSchedule = new EventSchedule(
                vm.NewEventSchedule.Event,
                (int)vm.NewEventSchedule.IntWeekdayDuration,
                (int)vm.NewEventSchedule.IntWeekendDuration,
                vm.NewEventSchedule.StrMondayStart,
                vm.NewEventSchedule.StrTuesdayStart,
                vm.NewEventSchedule.StrWednesdayStart,
                vm.NewEventSchedule.StrThursdayStart,
                vm.NewEventSchedule.StrFridayStart,
                vm.NewEventSchedule.StrSaturdayStart,
                vm.NewEventSchedule.StrSundayStart);

            insert.OpenConnection();

            insert.InsertEventSchedule(newEventSchedule);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddTypes(AdminHomeVM vm)
        {
            InsertDAL insert = new InsertDAL();

            insert.OpenConnection();

            insert.InsertType(vm.NewType);

            insert.CloseConnection();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult AddRehearsalPart(AdminHomeVM vm)
        {
            PeriodBuilder builder = new PeriodBuilder();
            builder.Minutes = vm.NewRehearsalPart.IntLengthMinutes;            
            vm.NewRehearsalPart.DurLength = builder.Build();

            GetDAL get = new GetDAL();
            get.OpenConnection();
            foreach(var id in vm.NewRehearsalPart.ArrMemberNeededIDs)
            {
                Users tmpUser = get.GetUserByID(id);
                vm.NewRehearsalPart.LstMembers.Add(tmpUser);
            }
            get.CloseConnection();

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            int newRehearsalPartID = insert.InsertRehearsalPart(vm.NewRehearsalPart);
            vm.NewRehearsalPart.IntRehearsalPartID = newRehearsalPartID;
            foreach (var m in vm.NewRehearsalPart.LstMembers)
            {
                AttendancePlanned ap = new AttendancePlanned(vm.NewRehearsalPart, m);
                insert.InsertAttendancePlanned(ap);
            }
            insert.CloseConnection();
            return RedirectToAction("Index");
        }
    }
}