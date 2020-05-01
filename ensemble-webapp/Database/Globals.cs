using ensemble_webapp.Models;
using System;
using System.Collections.Generic;

namespace ensemble_webapp.Database
{
    public static class Globals
    {
        public static bool LOGIN_STATUS = false;

        public static Users LOGGED_IN_USER { get; set; }

        public static bool IS_ADMIN = false;

        private static Users[] admins =
        {
            new Users(3, "Giuliana"),
            new Users(5, "Ryan"),
            new Users(16, "Devin")
        };

        public static List<Users> ADMINS = new List<Users>(admins);
    }
}