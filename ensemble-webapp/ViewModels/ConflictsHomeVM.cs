using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ConflictsHomeVM
    {

        List<Conflict> LstConflicts { get; set; }

        Conflict ConflictToPost { get; set; }
    }
}