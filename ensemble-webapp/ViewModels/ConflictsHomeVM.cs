﻿using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class ConflictsHomeVM
    {

        public Users CurrentUser { get; set; }

        public List<Conflict> LstConflicts { get; set; }

        public Conflict ConflictToAdd { get; set; }
    }
}