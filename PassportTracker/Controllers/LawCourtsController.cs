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
    public class LawCourtsController : Controller
    {
        private PassportTrackerDB db = new PassportTrackerDB();

        // GET: /LawCourts/
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
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(await db.tblLawCourts
                .Where(r => r.LC_Deleted == false)
                .ToListAsync());
        }

        // GET: /LawCourts/Details/5
        [SharePointContextFilter]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LawCourts lawcourts = await db.tblLawCourts.FindAsync(id);
            if (lawcourts == null)
            {
                return HttpNotFound();
            }
            return View(lawcourts);
        }

        private List<string> getJuristdiction()
        {
            var model = db.tblJurisdictionSelection
            .GroupBy(a => a.J_NameLocation)
            .Select(r => r.FirstOrDefault().J_NameLocation).ToList();

            return model;
        }

        // GET: /LawCourts/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            var Jmodel = getJuristdiction();

            SelectList JNames = new SelectList(Jmodel);
            ViewData["JNames"] = JNames;
            return View();
        }

        // POST: /LawCourts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public async Task<ActionResult> Create([Bind(Include="LC_Id,LC_Jurisdiction,LC_Name,LC_Street,LC_Suburb,LC_State,LC_DX,LC_PostCode,LC_Phone,LC_Fax")] LawCourts lawcourts)
        {
           
            lawcourts.LC_DateCreated = DateTime.Now;
            lawcourts.LC_DateModified = DateTime.Now;
            lawcourts.LC_CreatedBy = User.Identity.Name;
            lawcourts.LC_ModifiedBy = User.Identity.Name;
            lawcourts.LC_Deleted = false;
            if (ModelState.IsValid)
            {
                db.tblLawCourts.Add(lawcourts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Location", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(lawcourts);
        }

        // GET: /LawCourts/Edit/5
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

            LawCourts lawcourts = await db.tblLawCourts.FindAsync(id);
            if (lawcourts == null)
            {
                return HttpNotFound();
            }
            var Jmodel = getJuristdiction();

            SelectList JNames = new SelectList(Jmodel);
            ViewData["JNames"] = JNames;
            return View(lawcourts);
        }

        // POST: /LawCourts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public async Task<ActionResult> Edit([Bind(Include="LC_Id,LC_Jurisdiction,LC_Name,LC_Street,LC_Suburb,LC_State,LC_DX,LC_PostCode,LC_Phone,LC_Fax")] LawCourts lawcourts)
        {
            lawcourts.LC_DateCreated = lawcourts.LC_DateCreated == DateTime.MinValue ? DateTime.Now : lawcourts.LC_DateCreated;         
            lawcourts.LC_ModifiedBy = User.Identity.Name;
            lawcourts.LC_DateModified = DateTime.Now;
            if (ModelState.IsValid)
            {    
                db.Entry(lawcourts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Location", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            return View(lawcourts);
        }

        // GET: /LawCourts/Delete/5
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
            LawCourts lawcourts = await db.tblLawCourts.FindAsync(id);
            if (lawcourts == null)
            {
                return HttpNotFound();
            }
            return View(lawcourts);
        }

        // POST: /LawCourts/Delete/5
        [SharePointContextFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LawCourts lawcourts = await db.tblLawCourts.FindAsync(id);
            lawcourts.LC_Deleted = true;
           // db.tblLawCourts.Remove(lawcourts);
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
