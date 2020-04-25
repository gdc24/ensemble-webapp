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

            var equalityComparer = new EventEqualityComparer();

            IEnumerable<Event> difference = model.LstAllEvents.Except(model.CurrentUser.LstEvents, equalityComparer);
            model.LstEventsToJoin = difference.ToList();

            get.CloseConnection();

            return View("ProfileHome", model);
        }

        public ActionResult ProfileHome()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUserToEvent(ProfileHomeVM vm)
        {
            // add user to group
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            insertDAL.InsertToUserEvents(vm.NewEvent, vm.CurrentUser);

            insertDAL.CloseConnection();

            return RedirectToAction("Index");
        }

        //public ActionResult EditUserName(Users editedUserProfile)
        //{
        //    editedUserProfile.setName(editedUserProfile.StrName); //???THIS DOESN'T SEEM RIGHT???//
        //    return View();
        //}
    }
}
