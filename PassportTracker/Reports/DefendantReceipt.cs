using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportTracker.Reports
{
    public class DefendantReceipt
    {
        public int DR_Id { get; set; }
        public string DR_Name { get; set; }
        public string DR_Street { get; set; }
        public string DR_Suburb { get; set; }
        public string DR_State { get; set; }
        public string DR_DX { get; set; }
        public int DR_PostCode { get; set; }
        public string DR_Phone { get; set; }
        public string DR_Fax { get; set; }
        public string DR_Created { get; set; }
        public string DR_Location_Country { get; set; }
        public string DR_Location_Name { get; set; }
        public string DR_Location_Suburb { get; set; }
        public string DR_Location_State { get; set; }
        public string DR_Location_Street { get; set; }
        public int DR_Location_PostCode { get; set; }
        public string DR_Case_Id { get; set; }
        public DateTime DR_Date_Of_Birth { get; set; }
        public string DR_Place_Of_Birth { get; set; }
        public string DR_Name_Def { get; set; }
        public DateTime DR_Passport_Expiry_Date { get; set; }
        public string DR_Passport_Number { get; set; }
        public DateTime DR_Next_Hearing_Date { get; set; }
        public string DR_Jurisdiction { get; set; }
    }
}