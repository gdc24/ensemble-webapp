using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

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

        [HttpPost]
        public ViewResult SendEmail()
        {
            MailMessage mail = new MailMessage();
            GetDAL get = new GetDAL();
            get.OpenConnection();
            Event e = get.GetEventByID(model.Event.intEventID);
            foreach (string t in get.GetAllUsersByEvent(e))
            {
                mail.To.Add(t.StrEmail);
            }
            mail.From = new MailAddress(model.CurrentUser.StrEmail);
            mail.Subject = model.StrSubject;
            mail.Body = model.StrNote;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Enter seders User name and password   
            smtp.EnableSsl = true;
            smtp.Send(mail);
            get.CloseConnection();
            return View("Index");
        }
    }
}