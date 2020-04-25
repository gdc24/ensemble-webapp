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
                model.LstAllCallboards = get.GetCallboardsByEvent(e);
            }

            if (model.LstAllCallboards != null)
            {
                model.LstAllCallboards.Sort();
            }

            model.LstAllEvents = get.GetAllEvents();


            get.CloseConnection();

            return View("CallboardHome", model);
        }

        public ActionResult CallboardHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddAnnouncement(CallboardHomeVM vm)
        {
            Callboard newCallboard = vm.NewAnnouncement;

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.InsertCallboard(newCallboard);

            insert.CloseConnection();

            return RedirectToAction("Index");


        }
    }
}