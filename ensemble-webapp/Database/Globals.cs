using ensemble_webapp.Models;
using System;

namespace ensemble_webapp.Database
{
    public static class Globals
    {
        public static bool LOGIN_STATUS = false;

        public static Users LOGGED_IN_USER { get; set; }
    }
}