using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class PassportOfficeVM
    {
        public int VM_PO_Id { get; set; }
        public string VM_PO_Location_Country { get; set; }
        public string VM_PO_Location_Name { get; set; }
    }
}