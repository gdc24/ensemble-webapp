using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Models
{
    public class Types
    {
        public Types(string strName)
        {
            StrName = strName;
        }

        public Types(int intTypeID, string strName)
        {
            IntTypeID = intTypeID;
            StrName = strName;
        }

        public int IntTypeID { get; set; }

        public string StrName { get; set; }

        public Types() { }
    }
}