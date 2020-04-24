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
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {

            ProfileHomeVM model = new ProfileHomeVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;
            model.EditedUserProfile = model.CurrentUser;

            GetDAL get = new GetDAL();
            get.OpenConnection();

            model.LstAllEvents = get.GetAllEvents();

            get.CloseConnection();

            return View("ProfileHome", model);
        }

        public ActionResult ProfileHome()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUserToEvent(Users editedUserProfile)
        {
            // add user to group
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            foreach (var e in editedUserProfile.LstEvents)
            {
                insertDAL.InsertToUserEvents(e, editedUserProfile);
            }

            insertDAL.CloseConnection();

            return RedirectToAction("Index");
        }
    }
}
