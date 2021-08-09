using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class LawCourts
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LC_Id { get; set; }

        [StringLength(200)]
        [Display(Name = "Jurisdiction")]
        public string LC_Jurisdiction { get; set; }

        [StringLength(200)]
        [Display(Name = "Location Name")]
        public string LC_Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Address")]
        public string LC_Street { get; set; }

        [StringLength(200)]
        [Display(Name = "Suburb")]
        public string LC_Suburb { get; set; }

        [StringLength(200)]
        [Display(Name = "State")]
        public string LC_State { get; set; }

        [StringLength(200)]
        [Display(Name = "Postcode")]
        public string LC_DX { get; set; }

        [Display(Name = "DX Number")]
        public int LC_PostCode { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string LC_Phone { get; set; }

        [StringLength(20)]
        [Display(Name = "Fax Number")]
        public string LC_Fax { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime LC_DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string LC_ModifiedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime LC_DateCreated { get; set; }

        [Display(Name = "Created By")]
        public string LC_CreatedBy { get; set; }
        public bool LC_Deleted { get; set; }
    }
}