using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{
    public class InformationLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IL_Id { get; set; }
        public int PF_Id { get; set; }
        public DateTime IL_DateModified { get; set; }
        public string IL_ModifiedBy { get; set; }
        public string IL_ChangesLog { get; set; }
    }
}