using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class LocationVM
    {  //not stored in db

        public ICollection<PassportOfficeVM> passportOfficeVM { get; set; }
        public ICollection<LawCourtsVM> lawCourtsVM { get; set; }
    }
}