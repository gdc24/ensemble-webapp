using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class CallboardHomeVM
    {
        public Users CurrentUser { get; set; }

        public List<Callboard> LstAllCallboards { get; set; }
    }
}