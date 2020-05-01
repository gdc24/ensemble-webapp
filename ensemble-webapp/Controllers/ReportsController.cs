using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using IronPdf;
using Rotativa;

namespace ensemble_webapp.Controllers
{
    public class ReportsController : Controller
    {

        private static int rID { get; set; }

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
        public ActionResult MakeReport(ReportsHomeVM vm)
        {
            
            ReportsHomeVM model = new ReportsHomeVM();

            if (vm.ChosenRehearsal == null)
                vm.ChosenRehearsal = new Rehearsal(Globals.rID);

            GetDAL get = new GetDAL();
            get.OpenConnection();

            Rehearsal r = get.GetRehearsalByID(vm.ChosenRehearsal.IntRehearsalID);
            Globals.rID = r.IntRehearsalID;
            //Rehearsal r = vm.ChosenRehearsal;
            List<RehearsalPart> rehearsalPartsForToday = get.GetRehearsalPartsByRehearsal(r);

            model.EventName = r.Event.StrName;;
            model.GroupName = r.Event.Group.StrName;
            model.Location = r.StrLocation;
            model.StartTime = r.DtmStartDateTime.ToString();
            model.EndTime = r.DtmEndDateTime.ToString();
            model.RehearsalDate = r.DtmStartDateTime.Date.ToString();

            foreach (RehearsalPart rp in rehearsalPartsForToday)
            {
                rp.AttendancePlanned = get.GetAttendancePlannedByRehearsalPart(rp);
                rp.AttendanceActual = new List<AttendanceActual>();
                foreach (AttendancePlanned ap in rp.AttendancePlanned)
                {
                    rp.AttendanceActual = rp.AttendanceActual.Concat(get.GetAttendanceActualByPlanned(ap)).ToList();
                }
            }

            model.Notes = r.StrNotes;
            model.LstAllRehearsalParts = rehearsalPartsForToday;

            get.CloseConnection();

            Globals.PDF = model;
            
            return View("GenerateReport", model);
        }

        public ActionResult GenerateReport()
        {
            return RedirectToAction("Index");
        }
        
        //turn GenerateReport.cshtml into pdf
        [HttpPost]
        public ActionResult GeneratePDF(){
            //return new ViewAsPdf("", this.PDF);
            return new ActionAsPdf("MakeReport", Globals.PDF)
            {
                //var OutputPath = "~/Downloads/"+ vm.GroupName + "_" + vm.RehearsalDate + "_Report.pdf";
                FileName = Globals.PDF.GroupName + "_" + Globals.PDF.RehearsalDate + "_Report.pdf"
            };
            //var html = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "TestInvoice1.html"));
            //var htmlToPdf = new HtmlToPdf();
            //var pdf = htmlToPdf.RenderHtmlAsPdf(html);
            //pdf.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "HtmlToPdfExample1.Pdf"));

            //var Renderer = new HtmlToPdf();
            //var PDF = Renderer.RenderHTMLFileAsPdf("~/Views/Reports/GenerateReport.cshtml");
            //var OutputPath = "~/Downloads/"+ vm.GroupName + "_" + vm.RehearsalDate + "_Report.pdf";
            //PDF.SaveAs(OutputPath);
            //return RedirectToAction("Index", "Reports");
        }
    }
}
