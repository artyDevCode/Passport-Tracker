using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PassportTracker.Models
{

    public class PassportTrackerDB : DbContext //, ICJKnowledgeEntity
    {
        public PassportTrackerDB()
            : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public DbSet<LawCourts> tblLawCourts { get; set; }
        public DbSet<PassportForm> tblPassportForm { get; set; }
        public DbSet<PassportOffice> tblPassportOffice { get; set; }
        public DbSet<JurisdictionSelection> tblJurisdictionSelection { get; set; }
        public DbSet<InformationLog> tblInformationLog { get; set; }
        public DbSet<Access> tblAccess { get; set; }
    }
}