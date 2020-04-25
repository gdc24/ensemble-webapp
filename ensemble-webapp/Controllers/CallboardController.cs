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
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                CallboardHomeVM model = new CallboardHomeVM();
                model.CurrentUser = Globals.LOGGED_IN_USER;

                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.LstAllCallboards = new List<Callboard>();

                foreach (Event e in model.CurrentUser.LstEvents)
                {
                    List<Callboard> callboards = get.GetCallboardsByEvent(e);
                    model.LstAllCallboards.AddRange(callboards);
                }

                if (model.LstAllCallboards != null)
                {
                    model.LstAllCallboards.Sort();
                }

                model.LstAdminEvents = get.GetAdminEventsByUser(model.CurrentUser.IntUserID);


                get.CloseConnection();

                return View("CallboardHome", model);
            }
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