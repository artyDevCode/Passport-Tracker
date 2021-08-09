using PassportTracker.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;


namespace PassportTracker.Migrations
{


    internal sealed class Configuration : DbMigrationsConfiguration<PassportTracker.Models.PassportTrackerDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PassportTracker.Models.PassportTrackerDB context)
        {
            context.tblJurisdictionSelection.AddOrUpdate(a => a.J_NameLocation,
             new JurisdictionSelection
             {
                 J_NameLocation = "ChildrensCourt"
             },
             new JurisdictionSelection
             {
                 J_NameLocation = "County Court"
             },
             new JurisdictionSelection
             {
                 J_NameLocation = "Magistrates Court"
             },
             new JurisdictionSelection
             {
                 J_NameLocation = "Supreme Court"
             });

            //context.tblLawCourts.AddOrUpdate(a => a.LC_Name,
            //new LawCourts
            //{

            //    LC_Jurisdiction = "County Court",
            //    LC_Name = "County Court Victoria - Melbourne",
            //    LC_Street = "250 William Street",
            //    LC_Suburb = "Melbourne",
            //    LC_State = "Victoria",
            //    LC_DX = "DX 2900 Melbourne",
            //    LC_PostCode = 3000,
            //    LC_Phone = "(03)8636 6051",
            //    LC_Fax = "(03) 8636 6051",
            //    LC_CreatedBy = "Arty",
            //    LC_DateCreated = DateTime.Now,
            //    LC_DateModified = DateTime.Now,
            //    LC_ModifiedBy = "Arty1",
            //    LC_Deleted = false
            //},
            // new LawCourts
            // {

            //     LC_Jurisdiction = "Magistrates Court",
            //     LC_Name = "Melbourne Magistrates Court",
            //     LC_Street = "233 William Street",
            //     LC_Suburb = "Melbourne",
            //     LC_State = "Victoria",
            //     LC_DX = "DX 350080 Melbourne",
            //     LC_PostCode = 3000,
            //     LC_Phone = "(03) 9628 7777",
            //     LC_Fax = "(03) 9628 7826",
            //     LC_CreatedBy = "Arty",
            //     LC_DateCreated = DateTime.Now,
            //     LC_DateModified = DateTime.Now,
            //     LC_ModifiedBy = "Arty1",
            //     LC_Deleted = false
            // },
            // new LawCourts
            // {

            //     LC_Jurisdiction = "Magistrates Court",
            //     LC_Name = "Geelong Court Complex",
            //     LC_Street = "Railway Terrace",
            //     LC_Suburb = "Geelong",
            //     LC_State = "Victoria",
            //     LC_DX = "DX 216376",
            //     LC_PostCode = 3220,
            //     LC_Phone = "(03) 5225 3333",
            //     LC_Fax = "(03) 5225 3392",
            //     LC_CreatedBy = "Arty",
            //     LC_DateCreated = DateTime.Now,
            //     LC_DateModified = DateTime.Now,
            //     LC_ModifiedBy = "Arty1",
            //     LC_Deleted = false
            // });

            //context.tblPassportOffice.AddOrUpdate(a => a.PO_Location_Name,
            //  new PassportOffice
            //  {
            //      PO_Location_Country = "Australia",
            //      PO_Location_Name = "Department of Foreign Affairs & Trade Passport Office Casseldon Place",
            //      PO_Location_Street = "13th Floor, 2 Lonsdale Street",
            //      PO_Location_Suburb = "Melbourne",
            //      PO_Location_State = "Victoria",
            //      PO_Location_PostCode = 3000,
            //      PO_Location_Phone = "(03) 9221 5505",
            //      PO_Location_Fax = "(03) 9221 5589",
            //      PO_CreatedBy = "Arty",
            //      PO_DateCreated = DateTime.Now,
            //      PO_DateModified = DateTime.Now,
            //      PO_ModifiedBy = "Arty1",
            //      PO_Deleted = false

            //  },
            //   new PassportOffice
            //   {
            //       PO_Location_Country = "Canada",
            //       PO_Location_Name = "Consulate-General of Canada",
            //       PO_Location_Street = "Level 5, Quay West 111 Harrington Street",
            //       PO_Location_Suburb = "Sydney",
            //       PO_Location_State = "NSW",
            //       PO_Location_PostCode = 2000,
            //       PO_Location_Phone = "(02) 9364 3000",
            //       PO_Location_Fax = "(02) 9364 3001",
            //       PO_CreatedBy = "Arty",
            //       PO_DateCreated = DateTime.Now,
            //       PO_DateModified = DateTime.Now,
            //       PO_ModifiedBy = "Arty1",
            //       PO_Deleted = false

            //   },
            // new PassportOffice
            //  {
            //      PO_Location_Country = "Brazil",
            //      PO_Location_Name = "Embassy of Brazil",
            //      PO_Location_Street = "GPO BOX 1540",
            //      PO_Location_Suburb = "Canberra",
            //      PO_Location_State = "ACT",
            //      PO_Location_PostCode = 2601,
            //      PO_Location_Phone = "(02) 6283 2372",
            //      PO_Location_Fax = "(02) 6283 2374",
            //      PO_CreatedBy = "Arty",
            //      PO_DateCreated = DateTime.Now,
            //      PO_DateModified = DateTime.Now,
            //      PO_ModifiedBy = "Arty1",
            //      PO_Deleted = false,

            //  });

            //string filepath = "C:\\Users\\user1\\Desktop\\PassportForm1.csv";
            //StreamReader sr = new StreamReader(filepath);
            //string line = sr.ReadLine();
            //string[] value = line.Split(',');
            //DataTable dt = new DataTable();
            //DataRow row;
            //if (filepath.Length > 0)
            //    foreach (string dc in value)
            //    {
            //        dt.Columns.Add(new DataColumn(dc));

            //    }

            //while (!sr.EndOfStream)
            //{
            //    value = sr.ReadLine().Split(',');
            //    if (value.Length == dt.Columns.Count)
            //    {
            //        row = dt.NewRow();
            //        row.ItemArray = value;
            //        dt.Rows.Add(row);
            //        context.tblPassportForm.AddOrUpdate(a => a.PF_Name,
            //        new PassportForm
            //        {

            //            PF_Case_Id = value[0],
            //            PF_Country = value[1],
            //            PF_Created = value[2],
            //            PF_Date_Created = //DateTime.ParseExact(value[3], @"d/M/yy", System.Globalization.CultureInfo.InvariantCulture),
            //                    Convert.ToDateTime(value[3],	System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
            //            PF_Date_Of_Birth = //DateTime.ParseExact(value[4], @"d/M/yy", System.Globalization.CultureInfo.InvariantCulture),
            //                    Convert.ToDateTime(value[4], System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
            //          //  PF_EndDate = DateTime.Now, //Convert.ToDateTime(value[5],	System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
            //            PF_Initial_Location = value[6],
            //            PF_Current_Location = value[7],
            //            PF_Name = value[8],
            //            PF_Other = value[9],
            //            PF_Passport_Expiry_Date = //DateTime.ParseExact(value[10], @"d/M/yy", System.Globalization.CultureInfo.InvariantCulture),
            //                    Convert.ToDateTime(value[10], System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
            //            PF_Passport_Number = value[11],
            //            PF_Place_Of_Birth = value[12],
            //            PF_Date_Returned = //DateTime.ParseExact(value[13], @"d/M/yy", System.Globalization.CultureInfo.InvariantCulture),
            //                    Convert.ToDateTime(value[13], System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
            //            PF_Status = value[14],
            //            PF_Next_Hearing_Date = //DateTime.ParseExact(value[15], @"d/M/yy", System.Globalization.CultureInfo.InvariantCulture),
            //                    Convert.ToDateTime(value[15], System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
            //            PF_DateModified = DateTime.Now,
            //            PF_ModifiedBy = "Arty",
            //            PF_DateCreated = DateTime.Now,
            //            PF_CreatedBy = "Arty",
            //            PF_Deleted = false

            //        });
            //    }
            //}
        }
    }
}



