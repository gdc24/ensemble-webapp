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
        public ActionResult Index(bool IsInvalidAttempt = false)
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ProfileHomeVM model = new ProfileHomeVM();
                model.CurrentUser = Globals.LOGGED_IN_USER;
                model.EditedUserProfile = model.CurrentUser;
                model.IsInvalidPasswordAttempt = IsInvalidAttempt;

                GetDAL get = new GetDAL();
                get.OpenConnection();
                model.LstAllEvents = get.GetAllEvents();

                var equalityComparer = new EventEqualityComparer();

                IEnumerable<Event> difference = model.LstAllEvents.Except(model.CurrentUser.LstEvents, equalityComparer);
                model.LstEventsToJoin = difference.ToList();

                get.CloseConnection();

                return View("ProfileHome", model);
            }
        }

        public ActionResult ProfileHome()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditUser(ProfileHomeVM vm)
        {
            Users currentUser = Globals.LOGGED_IN_USER;

            currentUser.StrEmail = vm.EditedUserProfile.StrEmail;
            currentUser.StrPhone = vm.EditedUserProfile.StrPhone;

            InsertDAL insert = new InsertDAL();
            insert.OpenConnection();

            insert.ChangeEmailAndPhone(currentUser);

            insert.CloseConnection();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUserToEvent(ProfileHomeVM vm)
        {
            Users currentUser = Globals.LOGGED_IN_USER;
            // add user to group
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            insertDAL.InsertToUserEvents(vm.NewEvent, currentUser);

            insertDAL.CloseConnection();

            GetDAL get = new GetDAL();
            get.OpenConnection();
            Globals.LOGGED_IN_USER.LstEvents = get.GetEventsByUser(currentUser.IntUserID);
            get.CloseConnection();

            return RedirectToAction("Index");
        }

        public ActionResult ChangePass(ProfileHomeVM vm)
        {
            if (vm.OldPass1.Equals(vm.OldPass2) && Login.ChangePass(Globals.LOGGED_IN_USER, vm.OldPass1, vm.NewPass))
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", new { IsInvalidAttempt = true });
        }
    }
}
