using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class CallboardController : Controller
    {
        // GET: Callboard
        public ActionResult Index()
        {
            CallboardHomeVM model = new CallboardHomeVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;

            GetDAL get = new GetDAL();
            get.OpenConnection();

            foreach (Event e in model.CurrentUser.LstEvents)
            {
                model.LstAllCallboards.Concat(get.GetCallboardsByEvent(e));
            }

            return View();
        }
    }
}