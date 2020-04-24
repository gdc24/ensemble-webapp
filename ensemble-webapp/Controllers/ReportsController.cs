using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronPdf;

namespace ensemble_webapp.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult MakeReport(int rehearsalID){
            
        }
        
        public ActionResult GenerateReport(){
            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.RenderHtmlAsPdf("ensemble-webapp/ensemble-webapp/Views/Reports/GenerateReport.cshtml").SaveAs("Report.pdf");
        }
    }
}
