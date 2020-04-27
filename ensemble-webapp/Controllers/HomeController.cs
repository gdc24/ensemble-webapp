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
    public class HomeController : Controller
    {
        static readonly double DAYS_TO_SHOW_TASKS = 2;
        public ActionResult Index()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        public ActionResult About()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }
        }

        public ActionResult Login(bool isInvalidPasswordAttempt = false)
        {
            LoginVM model = new LoginVM();
            model.IsInvalidPasswordAttempt = isInvalidPasswordAttempt;

            GetDAL get = new GetDAL();
            get.OpenConnection();
            model.LstAllEvents = get.GetAllEvents();
            get.CloseConnection();

            return View("Login", model);
        }

        [HttpPost]
        public ActionResult LoginUser(LoginVM vm)
        {
            if (Database.Login.VerifyUser(vm.logInUser)) {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", new { isInvalidPasswordAttempt = true });
            }

        }

        [HttpPost]
        public ActionResult NewUser(LoginVM vm)
        {
            Users newUser = vm.newUser;
            if (Database.Login.CreateUser(newUser)) {
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Login");
        }

        public ActionResult Dashboard()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login");
            }
            else
            {
                DashboardVM model = new DashboardVM();
                model.CurrentUser = Globals.LOGGED_IN_USER;
                model.LstEvents = model.CurrentUser.LstEvents;

                GetDAL get = new GetDAL();
                get.OpenConnection();

                var taskEqualityComparer = new TaskEqualityComparer();

                // upcoming tasks should be all tasks due before two days from now EXCEPT ones already completed EXCEPT ones overdue
                // orrrr upcoming tasks should be all unfinished tasks due after today EXCEPT all tasks due after two days from now
                model.LstUpcomingTasks = get.GetUnfinishedTasksDueAfter(model.CurrentUser, DateTime.Now).Except(get.GetUnfinishedTasksDueAfter(model.CurrentUser, DateTime.Now.AddDays(DAYS_TO_SHOW_TASKS)), taskEqualityComparer).ToList();

                // overdue tasks should be all tasks that are unfinished that are due before today
                model.LstOverdueTasks = get.GetUnfinishedTasksDueBefore(model.CurrentUser, DateTime.Now);


                /****************************** upcoming rehearsals stuff start *********/

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
                foreach (RehearsalPart rp in model.LstUpcomingRehearsalParts)
                {
                    rp.LstMembers = get.GetUsersByRehearsalPart(rp);
                }
                get.CloseConnection();
                /****************************** upcoming rehearsals stuff end *********/

                //foreach (Event e in model.LstEvents)
                //{
                //    List<Rehearsal> rehearsals = get.GetRehearsalsByEvent(e);
                //    if (rehearsals.Any())
                //    { 
                //        model.LstUpcomingRehearsals = rehearsals;
                //    }
                //}

                get.CloseConnection();

                return View("Dashboard", model);
            }
        }

        public ActionResult DashboardHome()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Logout()
        {
            if (Database.Login.Logout())
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}