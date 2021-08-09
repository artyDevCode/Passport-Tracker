using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class PassportOffice
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PO_Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Country")]
        public string PO_Location_Country { get; set; }
        [StringLength(200)]
        [Display(Name = "Location Name")]
        public string PO_Location_Name { get; set; }
        [StringLength(200)]
        [Display(Name = "Address")]
        public string PO_Location_Street { get; set; }
        [StringLength(200)]
        [Display(Name = "Suburb")]
        public string PO_Location_Suburb { get; set; }
        [StringLength(200)]
        [Display(Name = "State")]
        public string PO_Location_State { get; set; }

        [Display(Name = "Post Code")]
        public int PO_Location_PostCode { get; set; }
        [StringLength(200)]
        [Display(Name = "Phone Number")]
        public string PO_Location_Phone { get; set; }
        [StringLength(200)]
        [Display(Name = "Fax Number")]
        public string PO_Location_Fax { get; set; }
        [Display(Name = "Date Modified")]
        public DateTime PO_DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string PO_ModifiedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime PO_DateCreated { get; set; }

        [Display(Name = "Created By")]
        public string PO_CreatedBy { get; set; }
        public bool PO_Deleted { get; set; }
    }
}