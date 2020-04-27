using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            return View("ReportsHome", model);
        }

        public ActionResult ReportsHome()
        {
            return RedirectToAction("Index");
        }
        
        //add text to GenerateReport.cshtml
        public ActionResult MakeReport(int rehearsalID){
            return RedirectToAction("Index");

            
        }
        
        //turn GenerateReport.cshtml into pdf
        public ActionResult GenerateReport(){
            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.RenderHtmlAsPdf("ensemble-webapp/ensemble-webapp/Views/Reports/GenerateReport.cshtml").SaveAs("Report.pdf");
            return RedirectToAction("Index");
        }
    }
}
