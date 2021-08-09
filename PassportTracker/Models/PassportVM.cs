using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class PassportVM
    {
        public int VM_Id { get; set; }
        public string VM_P_ViewColumn { get; set; }
        public string VM_P_Initial_Location { get; set; }
        public string VM_P_Name { get; set; }
        public string VM_P_Passport_Number { get; set; }
        public DateTime VM_PF_Date_Of_Birth { get; set; }     
        public string VM_PF_CaseId { get; set; }
        public DateTime VM_PF_Date_HandedIn { get; set; }
        public Nullable<DateTime> VM_PF_Date_Returned { get; set; }
    }
}