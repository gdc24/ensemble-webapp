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
            return View();
        }

        [HttpPost]
        public ActionResult AddUserToGroup(List<Event> newEvents)
        {
            // add user to group
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            List<Member> membershipsToAdd = new List<Member>();
            foreach (Event e in newEvents)
            {
                Member tmpMember = new Member(e, Globals.LOGGED_IN_USER);
                membershipsToAdd.Add(tmpMember);
            }

            foreach (Member m in membershipsToAdd)
            {
                insertDAL.InsertMember(m);
            }

            insertDAL.CloseConnection();

            GetDAL getDAL = new GetDAL();
            getDAL.OpenConnection();

            ProfileHomeVM model = new ProfileHomeVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;
            model.EditedUserProfile = model.CurrentUser;
            model.LstAllEvents = getDAL.GetAllEvents();
            model.LstUsersEvents = getDAL.GetEventsByUser(Globals.LOGGED_IN_USER);

            getDAL.CloseConnection();
            return View("ProfileHome");
        }
    }
}
