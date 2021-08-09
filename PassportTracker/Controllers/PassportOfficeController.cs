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

namespace PassportTracker.Controllers
{
    public class PassportOfficeController : Controller
    {
        private PassportTrackerDB db = new PassportTrackerDB();

        // GET: /PassportOffice/
        [SharePointContextFilter]
        public async Task<ActionResult> Index()
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorsRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(await db.tblPassportOffice
                .Where(r => r.PO_Deleted == false)
                .ToListAsync());
        }

        // GET: /PassportOffice/Details/5
        [SharePointContextFilter]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassportOffice passportoffice = await db.tblPassportOffice.FindAsync(id);
            if (passportoffice == null)
            {
                return HttpNotFound();
            }
            return View(passportoffice);
        }

        // GET: /PassportOffice/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorsRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View();
        }

        // POST: /PassportOffice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public async Task<ActionResult> Create([Bind(Include="PO_Id,PO_Location_Country,PO_Location_Name,PO_Location_Street,PO_Location_Suburb,PO_Location_State,PO_Location_PostCode,PO_Location_Phone,PO_Location_Fax")] PassportOffice passportoffice)
        {        
            passportoffice.PO_DateCreated = DateTime.Now;
            passportoffice.PO_DateModified = DateTime.Now;
            passportoffice.PO_CreatedBy = User.Identity.Name;
            passportoffice.PO_ModifiedBy = User.Identity.Name;
            passportoffice.PO_Deleted = false;
            if (ModelState.IsValid)
            {
                db.tblPassportOffice.Add(passportoffice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Location", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(passportoffice);
        }

        // GET: /PassportOffice/Edit/5
        [SharePointContextFilter]
        public async Task<ActionResult> Edit(int? id)
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorsRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassportOffice passportoffice = await db.tblPassportOffice.FindAsync(id);
            if (passportoffice == null)
            {
                return HttpNotFound();
            }
            return View(passportoffice);
        }

        // POST: /PassportOffice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public async Task<ActionResult> Edit([Bind(Include="PO_Id,PO_Location_Country,PO_Location_Name,PO_Location_Street,PO_Location_Suburb,PO_Location_State,PO_Location_PostCode,PO_Location_Phone,PO_Location_Fax")] PassportOffice passportoffice)
        {
            passportoffice.PO_DateCreated = passportoffice.PO_DateCreated == DateTime.MinValue ? DateTime.Now : passportoffice.PO_DateCreated;
            passportoffice.PO_DateModified = DateTime.Now;
            passportoffice.PO_ModifiedBy = User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.Entry(passportoffice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Location", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
           
            return View(passportoffice);
        }

        // GET: /PassportOffice/Delete/5
        [SharePointContextFilter]
        public async Task<ActionResult> Delete(int? id)
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorsRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassportOffice passportoffice = await db.tblPassportOffice.FindAsync(id);
            if (passportoffice == null)
            {
                return HttpNotFound();
            }
            return View(passportoffice);
        }

        // POST: /PassportOffice/Delete/5
        [SharePointContextFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PassportOffice passportoffice = await db.tblPassportOffice.FindAsync(id);
            passportoffice.PO_Deleted = true;
          //  db.tblPassportOffice.Remove(passportoffice);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Location", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
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
