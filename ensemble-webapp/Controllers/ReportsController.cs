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
            ReportsHomeVM model = new ReportsHomeVM();
            GetDAL get = new GetDAL();
            get.OpenConnection();

            model.LstAllRehearsalParts = get.GetAllRehearsalParts();

            get.CloseConnection();

            return View("ReportsHome", model);
        }

        public ActionResult ReportsHome()
        {
            return RedirectToAction("Index");
        }
        
        ////add text to GenerateReport.cshtml
        //public ActionResult MakeReport(int rehearsalID){
            
        //    ReportsHomeVM model = new ReportsHomeVM();
        //    model.CurrentUser = Globals.LOGGED_IN_USER;
            
        //    GetDAL get = new GetDAL();
        //    get.OpenConnection();

        //    Rehearsal r = get.GetRehearsalbyID(rehearsalID);
        //    List<RehearsalPart> pLst = GetRehearsalPartsByRehearsal(r);

        //    model.EventName = pLst.item[0].@event.strEventName;

        //    model.RehearsalDate = get.GetRehearsalByID(rehearsalID).dtmStartDateTime.Date.toString();

        //    model.GroupName = pLst.item[0].@event.group;

        //    model.Location = get.GetRehearsalByID(rehearsalID).strLocation;

        //    model.StartTime = get.GetRehearsalByID(rehearsalID).dtmStartDateTime.TimeOfDay.toString();

        //    model.EndTime = get.GetRehearsalByID(rehearsalID).dtmStartDateTime.TimeOfDay.toString();

        //    for(int i = 0; pLst.item[i] != null; i++){

        //        model.PlannedAttendance = model.PlannedAttendance.AddRage(get.GetAttendancePlannedByRehearsalPart(pLst.item[i]));
            
        //    }

        //    for(int j = 0; pLst.item[j] != null; j++){

        //        model.ActualAttendance = model.ActualAttendance.AddRage(get.GetAttendanceActualByRehearsalPart(pLst.item[j]));
            
        //    }
            
        //    model.Notes = get.GetRehearsalByID(rehearsalID).strNotes;

        //    get.CloseConnection();
            
        //    return RedirectToAction("Index");

            
        //}
        
        //turn GenerateReport.cshtml into pdf
        public ActionResult GenerateReport(ReportsHomeVM vm){
            var Renderer = new HtmlToPdf();
            var PDF = Renderer.RenderHTMLFileAsPdf("GenerateReport.cshtml");
            var OutputPath = "~/Downloads/"+ vm.GroupName + "_" + vm.RehearsalDate + "_Report.pdf";
            PDF.SaveAs(OutputPath);
            return RedirectToAction("Index", "Reports");
        }
    }
}
