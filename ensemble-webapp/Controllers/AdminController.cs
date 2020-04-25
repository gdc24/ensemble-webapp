using ensemble_webapp.Database;
using ensemble_webapp.ViewModels;
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

                get.CloseConnection();

                return View("AdminHome", model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult AddEvenSchedule(AdminHomeVM vm)
        {
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.InsertEventSchedule(vm.NewEventSchedule);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }
    }
}