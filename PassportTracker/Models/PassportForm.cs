using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class PassportForm
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PF_Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Case Id"), Required]
        public string PF_Case_Id { get; set; }      
        [StringLength(200)]
        [Display(Name = "Country")]
        public string PF_Country { get; set; } //from lookup dropdown
        [StringLength(300)]
        [Display(Name = "Country Location")]
        public string PF_Country_Location { get; set; } //from lookup dropdown
        [Display(Name = "Handed in To")]
        public string PF_Created { get; set; }
        [Display(Name = "Handed in On")]
        public DateTime PF_Date_Created { get; set; }
        [Display(Name = "Date of Birth"), Required]
        public DateTime PF_Date_Of_Birth { get; set; }
        [Display(Name = "Handed in At")]
        public string PF_Initial_Location { get; set; }
        [Display(Name = "Current Location")]
        public string PF_Current_Location { get; set; }
        [StringLength(200)]
        [Display(Name = "Name"), Required]
        public string PF_Name { get; set; }
        [Display(Name = "Other")]
        public string PF_Other { get; set; }
        [Display(Name = "Expiry Date"), Required]
        public DateTime PF_Passport_Expiry_Date { get; set; }
        [StringLength(200), Required]
        [Display(Name = "Passport Number")]
        public string PF_Passport_Number { get; set; }
        [StringLength(200)]
        [Display(Name = "Place of Birth")]
        public string PF_Place_Of_Birth { get; set; }
        [Display(Name = "Returned At")]
        public Nullable<DateTime> PF_Date_Returned { get; set; }
        [Display(Name = "Returned By")]
        public string PF_ReturnedBy { get; set; }
        [Display(Name = "Transfered At")]
        public Nullable<DateTime> PF_Date_Transfered { get; set; }
        [Display(Name = "Transfered By")]
        public string PF_TransferedBy { get; set; }
        [Display(Name = "Original Transfered Location")]
        public string PF_OriginalTransferedLoc { get; set; }
        [StringLength(200)]
        [Display(Name = "Status")]
        public string PF_Status { get; set; }
        [Display(Name = "Next Hearing Date")]
        public DateTime PF_Next_Hearing_Date { get; set; }
        public bool PF_Deleted { get; set; }
        public virtual ICollection<InformationLog> PF_InformationLog { get; set; }
    }
}