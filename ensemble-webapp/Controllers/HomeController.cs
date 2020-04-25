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
                return View();
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

        public ActionResult Contact()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
        }

        public ActionResult Login()
        {
            LoginVM model = new LoginVM();

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

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult NewUser(Users newUser)
        {
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
                model.LstUpcomingTasks = get.GetTasksDueAfter(model.CurrentUser, DateTime.Now).Except(get.GetTasksDueBefore(model.CurrentUser, DateTime.Now.AddDays(DAYS_TO_SHOW_TASKS)), taskEqualityComparer).ToList();


                foreach (Event e in model.LstEvents)
                {
                    List<Rehearsal> rehearsals = get.GetRehearsalsByEvent(e);
                    if (rehearsals.Any())
                    { 
                        model.LstUpcomingRehearsals = rehearsals;
                    }
                }

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