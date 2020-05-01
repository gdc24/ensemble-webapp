using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;

namespace ensemble_webapp.Database
{
    public static class Globals
    {
        public static bool LOGIN_STATUS = false;

        public static Users LOGGED_IN_USER { get; set; }

        public static bool IS_ADMIN = false;

        public static ReportsHomeVM PDF { get; set; }

        public static int rID { get; set; }

        private static Users[] admins =
        {
            new Users(3, "Giuliana"),
            new Users(5, "Ryan"),
            new Users(16, "Devin")
        };

        public static List<Users> ADMINS = new List<Users>(admins);
    }
}