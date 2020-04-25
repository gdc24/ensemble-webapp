using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Group
    {
        public Group(int intGroupID, string strName)
        {
            IntGroupID = intGroupID;
            StrName = strName;
        }

        public int IntGroupID { get; set; }

        public string StrName { get; set; }

        public Group() { }
    }
}