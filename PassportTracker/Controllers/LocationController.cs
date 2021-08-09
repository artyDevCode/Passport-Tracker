using PassportTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PassportTracker.Controllers
{
    public class LocationController : Controller
    {
          PassportTrackerDB _db;

        public LocationController()
        {
            _db = new PassportTrackerDB();
          
        }

        public LocationController(PassportTrackerDB db)
        {
            _db = db;
        }

        //
        // GET: /Location/
        [SharePointContextFilter]
        public ActionResult Index()
        {
            // Check access levels and pass to view
            int index = User.Identity.Name.IndexOf("\\");
            string user = User.Identity.Name.Substring(index + 1);
            List<Access> AccessGroupsModel = _db.tblAccess
                             .Where(r => r.UserId == user)
                             .ToList();

            ViewData["InOwnerRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("owner")).Count() > 0 ? "true" : "false";
            ViewData["InAuthorRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("author")).Count() > 0 ? "true" : "false";
            ViewData["InCountryRole"] = AccessGroupsModel.Where(r => r.AccessGroup.ToLower().Contains("country")).Count() > 0 ? "true" : "false";

            if ((ViewData["InAuthorsRole"] != "true") && ViewData["InCountryRole"] != "true" && ViewData["InOwnerRole"] != "true")
            {
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            LocationVM location = new LocationVM();
            location.lawCourtsVM = _db.tblLawCourts
                .Where(a => a.LC_Deleted == false)
                .Select(r => new LawCourtsVM { 
                    VM_LC_Id = r.LC_Id, 
                    VM_LC_Name = r.LC_Name
                }).ToList<LawCourtsVM>();

            location.passportOfficeVM = _db.tblPassportOffice
                .Where(a => a.PO_Deleted == false)
                .Select(v => new PassportOfficeVM
                {
                    VM_PO_Id = v.PO_Id,
                    VM_PO_Location_Country = v.PO_Location_Country,
                    VM_PO_Location_Name = v.PO_Location_Name
                }).ToList<PassportOfficeVM>();


            return View(location);
        }

 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}