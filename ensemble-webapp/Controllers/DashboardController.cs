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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardVM model = new DashboardVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;
            model.LstEvents = model.CurrentUser.LstEvents;

            GetDAL get = new GetDAL();
            get.OpenConnection();

            model.LstUpcomingTasks = get.GetTasksDueAfter(model.CurrentUser, DateTime.Now).Except(get.GetTasksDueAfter(model.CurrentUser, DateTime.Now.AddDays(2.0))).ToList();
            
            foreach (Event e in model.LstEvents)
            {
                model.LstUpcomingRehearsals.Concat(get.GetRehearsalsByEvent(e));
            }
            
            get.CloseConnection();

            return View();
        }

        public ActionResult DashboardHome()
        {
            return RedirectToAction("Index");
        }
    }
}