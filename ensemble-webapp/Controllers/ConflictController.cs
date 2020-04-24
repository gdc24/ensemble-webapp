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
    public class ConflictController : Controller
    {
        // GET: Conflict
        public ActionResult Index()
        {
            ConflictsHomeVM model = new ConflictsHomeVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;

            GetDAL get = new GetDAL();
            get.OpenConnection();

            model.LstConflicts = get.GetConflictsByUser(model.CurrentUser);

            get.CloseConnection();

            return View();
        }

        public ActionResult ConflictsHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddConflict(Conflict c)
        {
            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.InsertConflict(c);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }
    }
}