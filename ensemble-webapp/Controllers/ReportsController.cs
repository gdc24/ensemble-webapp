using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using IronPdf;

namespace ensemble_webapp.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ReportsHomeVM model = new ReportsHomeVM();
                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.LstAllRehearsalParts = get.GetAllRehearsalParts();

                model.LstAllEvents = get.GetEventsByUser(Globals.LOGGED_IN_USER.IntUserID);
                model.LstAllRehearsals = new List<Rehearsal>();
                foreach (Event e in model.LstAllEvents)
                {
                    get.CloseConnection();
                    get.OpenConnection();
                    model.LstAllRehearsals.AddRange(get.GetRehearsalsByEvent(e));
                }

                get.CloseConnection();

                foreach (Rehearsal r in model.LstAllRehearsals)
                {
                    r.DateWithEvent = r.Event.StrName + " " + r.DtmStartDateTime;
                }


                return View("ReportsHome", model);
            }
        }

        public ActionResult ReportsHome()
        {
            return RedirectToAction("Index");
        }
        
        ////add text to GenerateReport.cshtml
        public ActionResult MakeReport(int rehearsalID){
            
            ReportsHomeVM model = new ReportsHomeVM();
            
            GetDAL get = new GetDAL();
            get.OpenConnection();

            Rehearsal r = get.GetRehearsalByID(rehearsalID);
            List<RehearsalPart> pLst = get.GetRehearsalPartsByRehearsal(r);

            model.EventName = r.Event.StrName;;
            model.GroupName = r.Event.Group.StrName;
            model.Location = r.StrLocation;
            model.StartTime = r.DtmStartDateTime.ToString();
            model.EndTime = r.DtmEndDateTime.ToString();
            model.RehearsalDate = r.DtmStartDateTime.Date.ToString();

            foreach (RehearsalPart rp in pLst)
            {
                foreach (AttendancePlanned ap in get.GetAttendancePlannedByRehearsalPart(rp))
                {
                    get.CloseConnection();
                    get.OpenConnection();
                    model.ActualAttendance.AddRange(get.GetAttendanceActualByPlanned(ap));
                    model.PlannedAttendance.Add(ap);
                }
            }

            model.Notes = r.StrNotes;

            get.CloseConnection();
            
            return RedirectToAction("GenerateReport", model);
        }

        public ActionResult GenerateReport()
        {
            return RedirectToAction("Index");
        }
        
        //turn GenerateReport.cshtml into pdf
        [HttpPost]
        public ActionResult GeneratePDF(ReportsHomeVM vm){
            var Renderer = new HtmlToPdf();
            var PDF = Renderer.RenderHTMLFileAsPdf("GenerateReport.cshtml");
            var OutputPath = "~/Downloads/"+ vm.GroupName + "_" + vm.RehearsalDate + "_Report.pdf";
            PDF.SaveAs(OutputPath);
            return RedirectToAction("Index", "Reports");
        }
    }
}
