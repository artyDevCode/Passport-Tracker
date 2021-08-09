using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PassportTracker.Models;
using PassportTracker.Reports;
using Microsoft.Reporting.WebForms;



namespace PassportTracker.Controllers
{
    public class PassportFormController : Controller
    {
        PassportReport rec = new PassportReport();
        private PassportTrackerDB db = new PassportTrackerDB();

        private void SaveLogInformation(PassportForm model, string ChangesLog)
        {
            InformationLog infoLog = new InformationLog
            {
                IL_ModifiedBy = User.Identity.Name,
                PF_Id = model.PF_Id == 0 ? 1 : model.PF_Id,
                IL_DateModified = DateTime.Now,
                IL_ChangesLog = ChangesLog
            };
            // Save log information
            db.Entry(infoLog).State = EntityState.Added;
            db.SaveChanges();
        }
        public ActionResult DoTransfer(int id, string CL, string OL)
        {
            // save the changes and reset the parameters.
            var model = db.tblPassportForm
              .Where(r => r.PF_Id == id).Single();
            if (model != null)
            {
                model.PF_Status = "Pending";
                model.PF_Date_Transfered = DateTime.Now;
                model.PF_TransferedBy = User.Identity.Name;
                model.PF_Current_Location = CL;
                model.PF_OriginalTransferedLoc = OL;
                model.PF_Date_Transfered = DateTime.Now;
                model.PF_TransferedBy = User.Identity.Name;
                //if (model.PF_Country != "Australia")
                //    rec.getDefendantReceipt("PassportTransfered-NonAus.rdlc", model.PF_Id);
                //else
                //{
                //    rec.getDefendantReceipt("PassportTransfered-Aus.rdlc", model.PF_Id);
                //}
                //rec.getDefendantReceipt("PassportRetDef.rdlc", model.PF_Id);
                //db.Entry(model).State = EntityState.Modified;
                //db.SaveChanges();

                //SaveLogInformation(model, "Pending transfer at " + model.PF_OriginalTransferedLoc + " by ");

            }
            //return;
            return  RedirectToAction("Edit", "PassportForm", new { id = model.PF_Id, SPHostUrl = SharePointContextFilterAttribute.sphosturl }); ;
        }

        public ActionResult AcceptTransfer(int? id)
        {
            // save the changes and reset the parameters.
            var model = db.tblPassportForm
              .Where(r => r.PF_Id == id).Single();

            if (model != null && model.PF_Status.ToLower() == "pending")
            {
                SaveLogInformation(model, "Reviewed at " + model.PF_OriginalTransferedLoc + " by ");

                model.PF_Status = string.Empty;
                model.PF_Date_Transfered = null;
                model.PF_TransferedBy = string.Empty;
                model.PF_OriginalTransferedLoc = string.Empty;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });

            }
           return RedirectToAction("Edit", "PassportForm", new { id = model.PF_Id, SPHostUrl = SharePointContextFilterAttribute.sphosturl });
        }


        public ActionResult CancelTransfer(int? id)
        {
            //Cancel the transfer and reset all the fields
            var model = db.tblPassportForm
              .Where(r => r.PF_Id == id).Single();
            if (model != null && model.PF_Status.ToLower() == "pending")
            {
                SaveLogInformation(model, "Transfer cancelled by ");

                model.PF_Current_Location = model.PF_OriginalTransferedLoc.ToString();
                model.PF_Status = string.Empty;
                model.PF_Date_Transfered = null;
                model.PF_TransferedBy = string.Empty;
                model.PF_OriginalTransferedLoc = string.Empty;


                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            return RedirectToAction("Edit", "PassportForm", new { id=model.PF_Id, SPHostUrl = SharePointContextFilterAttribute.sphosturl });
        }


        public async Task<ActionResult> ReturnedPrint(int? id)
        {
            var model = db.tblPassportForm
                .Where(r => r.PF_Id == id).Single();
            if (model != null)
            {
                model.PF_Status = "Returned";
                model.PF_Date_Returned = DateTime.Now;
                model.PF_ReturnedBy = User.Identity.Name;
                db.Entry(model).State = EntityState.Modified;

                SaveLogInformation(model, "Passport returned by ");
                await db.SaveChangesAsync();
                if (model.PF_Country != "Australia")
                    // PassportReturnNonAus(id);
                    rec.getDefendantReceipt("PassportReturn-NonAus.rdlc", id);
                else
                    // PassportReturnAus(id);
                    rec.getDefendantReceipt("PassportReturn-Aus.rdlc", id);
                //  PassportRetDef(id);
                rec.getDefendantReceipt("PassportRetDef.rdlc", id);
            }
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        public async Task<ActionResult> UnmarkRetuned(int? id)
        {
            var model = db.tblPassportForm
               .Where(r => r.PF_Id == id).Single();
            SaveLogInformation(model, "Passport unmark returned by ");
            if (model != null)
            {
                model.PF_Status = string.Empty;
                model.PF_Date_Returned = null;
                model.PF_ReturnedBy = string.Empty;
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }


        //START REPORTING
        [SharePointContextFilter]
        public ActionResult PassportRetDef(int? id)
        {
            rec.getDefendantReceipt("PassportRetDef.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        [SharePointContextFilter]
        public ActionResult PassportTransferedAus(int? id)
        {
            rec.getDefendantReceipt("PassportTransfered-Aus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult PassportTransferedNonAus(int? id)
        {
            rec.getDefendantReceipt("PassportTransfered-NonAus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult PassportReturnAus(int? id)
        {
            rec.getDefendantReceipt("PassportReturn-Aus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult PassportReturnNonAus(int? id)
        {
            rec.getDefendantReceipt("PassportReturn-NonAus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult DefendantReceipt(int? id)
        {
            rec.getDefendantReceipt("DefendantReceipt.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult PassportHeldAus(int? id)
        {
            rec.getDefendantReceipt("PassportHeld-Aus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult PassportHeldNonAus(int? id)
        {
            rec.getDefendantReceipt("PassportHeld-NonAus.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
        [SharePointContextFilter]
        public ActionResult BenchClerkAlert(int? id)
        {
            rec.getDefendantReceipt("BenchClerkAlert.rdlc", id);
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }


        //END REPORTING

        //private List<CountrySelection> getPassportOffice()
        public List<CountrySelection> getPassportOffice()
        {

            List<CountrySelection> model = db.tblPassportOffice
                .Select(r => new CountrySelection
                {
                    CS_Id = r.PO_Id,
                    CS_Country = r.PO_Location_Country + " -- " + r.PO_Location_Name
                }).ToList();
            return model;
        }

        private List<string> getLawCourts()
        {
            List<string> model = db.tblLawCourts
                .Select(r => r.LC_Name)
                .ToList();

            return model;
        }
        // GET: /PassportForm/
        [SharePointContextFilter]
        public async Task<ActionResult> Index()
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            Access currentUser = (from u in db.tblAccess
                                  where u.UserId == user
                                  select u).FirstOrDefault();

            if (currentUser != null)
                ViewData["UserName"] = currentUser.UserName;
            else
            { ViewData["UserName"] = User.Identity.Name; }
            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(await db.tblPassportForm.ToListAsync());
        }

        // GET: /PassportForm/Details/5
        [SharePointContextFilter]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassportForm passportform = await db.tblPassportForm.FindAsync(id);
            if (passportform == null)
            {
                return HttpNotFound();
            }
            return View(passportform);
        }

        // GET: /PassportForm/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            Access currentUser = (from u in db.tblAccess
                                  where u.UserId == user
                                  select u).FirstOrDefault();

            if (currentUser != null)
                ViewData["UserName"] = currentUser.UserName;
            else
            { ViewData["UserName"] = User.Identity.Name; }
            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            List<CountrySelection> CModel = getPassportOffice();
            SelectList CMNames = new SelectList(CModel.Select(r => r.CS_Country));
            ViewData["CNames"] = CMNames;

            var ILModel = getLawCourts();
            SelectList ILMNames = new SelectList(ILModel);
            ViewData["ILNames"] = ILMNames;

            PassportForm passportform = new PassportForm();
            passportform.PF_Next_Hearing_Date = DateTime.Now;
            passportform.PF_Passport_Expiry_Date = DateTime.Now;
            passportform.PF_Date_Of_Birth = DateTime.Now;
            passportform.PF_Created = User.Identity.Name;
            passportform.PF_Date_Created = DateTime.Now;
            passportform.PF_Case_Id = "N/A";

            return View(passportform);
        }

        // POST: /PassportForm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]   //PF_DateCreated,
        [SharePointContextFilter]
        public async Task<ActionResult> Create([Bind(Include = "PF_Case_Id,PF_Country,PF_Created, PF_Date_Created, PF_Date_Of_Birth,PF_Initial_Location,PF_Current_Location,PF_Name,PF_Other,PF_Passport_Expiry_Date,PF_Passport_Number, PF_Place_Of_Birth,PF_Date_Returned,PF_Status,PF_Next_Hearing_Date ")]  PassportForm passportform)
        {

            // passportform.PF_Country_Location = passportform.PF_Country.Split("--",0);

            passportform.PF_Country_Location = passportform.PF_Country.Substring(passportform.PF_Country.IndexOf("--") + 3);
            string[] country = passportform.PF_Country.Split(' ');
            passportform.PF_Country = country[0];
            passportform.PF_Created = User.Identity.Name;
            passportform.PF_Deleted = false;
            passportform.PF_Current_Location = passportform.PF_Initial_Location;
            passportform.PF_Status = string.Empty;

            if (ModelState.IsValid)
            {
                db.tblPassportForm.Add(passportform);
                await db.SaveChangesAsync();

                SaveLogInformation(passportform, "Created on " + DateTime.Now + " by ");


                if (passportform.PF_Country == "Australia")
                    rec.getDefendantReceipt("PassportHeld-Aus.rdlc", passportform.PF_Id);
                else
                    rec.getDefendantReceipt("PassportHeld-NonAus.rdlc", passportform.PF_Id);

                rec.getDefendantReceipt("DefendantReceipt.rdlc", passportform.PF_Id);
                rec.getDefendantReceipt("BenchClerkAlert.rdlc", passportform.PF_Id);
                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            List<CountrySelection> CModel = getPassportOffice();
            SelectList CMNames = new SelectList(CModel.Select(r => r.CS_Country), "CS_Id", "CS_Country");
            ViewData["CNames"] = CMNames;

            var ILModel = getLawCourts();
            SelectList ILMNames = new SelectList(ILModel);
            ViewData["ILNames"] = ILMNames;
            return View(passportform);
        }

        // GET: /PassportForm/Edit/5
        [SharePointContextFilter]
        public async Task<ActionResult> Edit(int? id)
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            Access currentUser = (from u in db.tblAccess
                                  where u.UserId == user
                                  select u).FirstOrDefault();

            if (currentUser != null)
                ViewData["UserName"] = currentUser.UserName;
            else
            { ViewData["UserName"] = User.Identity.Name; }
            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            PassportForm passportform = await db.tblPassportForm.FindAsync(id);

            if (passportform == null)
            {
                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            //var selectedItem = db.tblPassportOffice.Where(r => r.PO_Location_Country == passportform.PF_Country).Select(t => t.PO_Id).Single();
            //List<CountrySelection> CModel = getPassportOffice();
            //SelectList CMNames = new SelectList(CModel, "CS_Id", "CS_Country", selectedItem.ToString() );
            //ViewData["CNames"] = CMNames;


            var selectedItem = db.tblPassportOffice.Where(r => r.PO_Location_Country == passportform.PF_Country).Select(t => t.PO_Id).Single();
            SelectList CMNames = new SelectList(getPassportOffice(), "CS_Id", "CS_Country", selectedItem.ToString());
            ViewData["CNames"] = CMNames;


            var ILModel = getLawCourts();
            SelectList ILMNames = new SelectList(ILModel);
            ViewData["ILNames"] = ILMNames;
            return View(passportform);
        }

        // POST: /PassportForm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //PF_DateCreated,
        [SharePointContextFilter]
        public async Task<ActionResult> Edit([Bind(Include = "PF_Id, PF_OriginalTransferedLoc,PF_Case_Id,PF_Country,PF_Country_Location,PF_Created,PF_Date_Created,PF_Date_Of_Birth,PF_Initial_Location,PF_Current_Location,PF_Name,PF_Other,PF_Passport_Expiry_Date,PF_Passport_Number, PF_Place_Of_Birth,PF_Date_Returned,PF_Status,PF_Next_Hearing_Date ")]  PassportForm passportform)
        {
            passportform.PF_Country_Location = passportform.PF_Country.Substring(passportform.PF_Country.IndexOf("--") + 3);
            string[] country = passportform.PF_Country.Split(' ');
            passportform.PF_Country = country[0];
            passportform.PF_Created = User.Identity.Name;
            passportform.PF_Deleted = false;
            passportform.PF_Current_Location = passportform.PF_Initial_Location;
            passportform.PF_Status = string.Empty;

            if (ModelState.IsValid)
            {
                SaveLogInformation(passportform, "Modified on " + DateTime.Now + " by ");


                // Save passport information         
                passportform.PF_Deleted = false;
                db.Entry(passportform).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            var selectedItem = db.tblPassportOffice.Where(r => r.PO_Location_Country == passportform.PF_Country).Select(t => t.PO_Id).Single();
            List<CountrySelection> CModel = getPassportOffice();
            SelectList CMNames = new SelectList(CModel.Select(r => r.CS_Country), "CS_Id", "CS_Country", selectedItem.ToString());
            ViewData["CNames"] = CMNames;

            var ILModel = getLawCourts();
            SelectList ILMNames = new SelectList(ILModel);
            ViewData["ILNames"] = ILMNames;
            return View(passportform);
        }

        // GET: /PassportForm/Delete/5
        [SharePointContextFilter]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassportForm passportform = await db.tblPassportForm.FindAsync(id);
            if (passportform == null)
            {
                return HttpNotFound();
            }
            return View(passportform);
        }

        // POST: /PassportForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PassportForm passportform = await db.tblPassportForm.FindAsync(id);
            passportform.PF_Deleted = true;
            SaveLogInformation(passportform, "Deleted on " + DateTime.Now + " by ");

            //  db.tblPassportForm.Remove(passportform);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        [SharePointContextFilter]
        public ActionResult Unauthorised(string alertMessage)
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);

            Access currentUser = (from u in db.tblAccess
                                  where u.UserId == user
                                  select u).FirstOrDefault();

            if (currentUser != null)
                ViewData["UserName"] = currentUser.UserName;
            else
            { ViewData["UserName"] = User.Identity.Name; }

            TempData["alertMessage"] = alertMessage;
            return View("Unauthorised");
        }
        [SharePointContextFilter]
        public ActionResult Alert(string alertMessage)
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            Access currentUser = (from u in db.tblAccess
                                  where u.UserId == user
                                  select u).FirstOrDefault();

            if (currentUser != null)
                ViewData["UserName"] = currentUser.UserName;
            else
            { ViewData["UserName"] = User.Identity.Name; }
            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            TempData["alertMessage"] = alertMessage;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
