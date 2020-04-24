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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            LoginVM model = new LoginVM();

            GetDAL get = new GetDAL();
            get.OpenConnection();
            model.LstAllEvents = get.GetAllEvents();

            get.CloseConnection();

            return View("Login", model);
        }

        [HttpPost]
        public ActionResult LoginUser(LoginVM vm)
        {
            if (Database.Login.VerifyUser(vm.logInUser)) {
                return RedirectToAction("ProfileHome");
            }

            //ProfileHomeVM model = new ProfileHomeVM();

            //GetDAL get = new GetDAL();
            //get.OpenConnection();

            //Globals.LOGGED_IN_USER = get.GetUserByName(vm.logInUser.StrName);
            //model.LstAllEvents = get.GetAllEvents();

            //get.CloseConnection();

            //model.CurrentUser = Globals.LOGGED_IN_USER;
            //model.EditedUserProfile = model.CurrentUser;

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult NewUser(Users newUser)
        {
            if (Database.Login.CreateUser(newUser)) {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            if (Database.Login.Logout())
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}