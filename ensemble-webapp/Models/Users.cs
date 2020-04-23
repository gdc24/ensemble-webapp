using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Users
    {

        public int IntUserID { get; set; }

        public byte[] BytSalt { get; set; }

        public byte[] BytKey { get; set; }

        public string StrUsername { get; set; }

        public string StrName { get; set; }

        public string StrEmail { get; set; }

        public int IntPhone { get; set; }
    }
}