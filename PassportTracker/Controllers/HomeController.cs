using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PassportTracker.Models;



namespace PassportTracker.Controllers
{
    public class HomeController : Controller
    {
        PassportTrackerDB _db = new PassportTrackerDB();
        
        public class JQueryDataTableParamModel
        {
            public string sEcho { get; set; }
            public string sSearch { get; set; }
            public int iDisplayLength { get; set; }
            public int iDisplayStart { get; set; }
            public string first_data { get; set; }
            public string second_data { get; set; }

        }
        
       


        public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        {

            var model = _db.tblPassportForm
               .Where(r => r.PF_Deleted == false)
               .Select(CheckValues).ToList<PassportVM>();

    
            List<PassportVM> modelsorted;
           
            int totalRowsCount = 0;
            int filteredRowsCount = 0;
            string[][] aaData;
            
            if (param.first_data != null && param.second_data != null)
            {
                List<PassportVM> aa = new List<PassportVM>();
                foreach (var group in model)
                {
                    var test = group.VM_P_ViewColumn.Replace(" ","").Trim('-').ToLower();
                    if (test.Equals(param.first_data) && 
                        group.VM_P_Initial_Location.ToLower().Contains(param.second_data.Trim()))
                        aa.Add(group);
                }

                modelsorted = aa.OrderBy(r => r.VM_P_ViewColumn).ThenBy(e => e.VM_P_Initial_Location).ToList<PassportVM>();
                aa = null;
                aaData = modelsorted.Select(d => new string[] 
                { 
                    d.VM_P_ViewColumn, 
                    d.VM_P_Initial_Location,
                    d.VM_P_Passport_Number, 
                    d.VM_P_Name, d.VM_PF_Date_Of_Birth.ToString("dd-MM-yyyy"), 
                    d.VM_PF_CaseId, d.VM_PF_Date_HandedIn.ToString("dd-MM-yyy"), 
                    d.VM_PF_Date_Returned.HasValue ? d.VM_PF_Date_Returned.GetValueOrDefault().ToString("dd-MM-yyy") : "", 
                    d.VM_Id.ToString() 
                }).ToArray();

                totalRowsCount = modelsorted.Count();
                filteredRowsCount = modelsorted.Count();

                return Json(new
                {
                    sEcho = param.sEcho,
                    aaData = aaData,
                    iTotalRecords = Convert.ToInt32(totalRowsCount),
                    iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount)
                }, JsonRequestBehavior.AllowGet);
              

            }



            if (param.sSearch != null)
            {
                if (param.sSearch.Length > 2)
                {
                    
                    // string[] words = param.sSearch.Split(' ');
                    List<PassportVM> aa = new List<PassportVM>();
                    foreach (var group in model)
                    {
                        if (group.VM_P_Name.ToLower().Contains(param.sSearch.ToLower()) || group.VM_P_Passport_Number.ToLower().Contains(param.sSearch.ToLower()) ||
                            group.VM_PF_CaseId.ToLower().Contains(param.sSearch.ToLower()) || group.VM_P_ViewColumn.Contains(param.sSearch) || group.VM_P_Initial_Location.ToLower().Contains(param.sSearch.ToLower()))
                            aa.Add(group);
                       
                    }

                    modelsorted = aa.OrderBy(r => r.VM_P_ViewColumn).ThenBy(e => e.VM_P_Initial_Location).ToList<PassportVM>();
                    aa = null;
                    aaData = modelsorted.Select(d => new string[] { d.VM_P_ViewColumn, d.VM_P_Initial_Location, d.VM_P_Passport_Number, d.VM_P_Name, d.VM_PF_Date_Of_Birth.ToString("dd-MM-yyyy"), d.VM_PF_CaseId, d.VM_PF_Date_HandedIn.ToString("dd-MM-yyy"), d.VM_PF_Date_Returned.ToString(), d.VM_Id.ToString() }).ToArray();

                    totalRowsCount = modelsorted.Count();
                    filteredRowsCount = modelsorted.Count();

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        aaData = aaData,
                        iTotalRecords = Convert.ToInt32(totalRowsCount),
                        iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount)
                    }, JsonRequestBehavior.AllowGet);
                }
 
            }

 

            totalRowsCount = model.Count();
            filteredRowsCount = model.Count();
            var vv = model.Select(r => new { r.VM_P_ViewColumn, r.VM_P_Initial_Location }).Distinct().OrderBy(r => r.VM_P_ViewColumn).ThenBy(e => e.VM_P_Initial_Location).ToList();
           // modelsorted = model.OrderBy(r => r.VM_P_ViewColumn).ThenBy(e => e.VM_P_Initial_Location).ToList();
       
           // aaData = modelsorted.Select(d => new string[] { d.VM_P_ViewColumn, "", "", "", "", "", "", "", "" }).Distinct().ToArray();
            aaData = vv.Select(d => new string[] { d.VM_P_ViewColumn, d.VM_P_Initial_Location, "", "", "", "", "", "", "" }).ToArray();
           
            //new string[] { d.VM_P_ViewColumn, d.VM_P_Initial_Location, d.VM_P_Passport_Number, d.VM_P_Name, d.VM_PF_Date_Of_Birth.ToString("dd-MM-yyyy"), d.VM_PF_CaseId, d.VM_PF_Date_HandedIn.ToString("dd-MM-yyy"), d.VM_PF_Date_Returned.ToString(), d.VM_Id.ToString() }).ToArray();
           
            return Json(new
            {
                sEcho = param.sEcho,
                aaData = aaData,
                iTotalRecords = Convert.ToInt32(totalRowsCount),
                iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount)
            }, JsonRequestBehavior.AllowGet);

        }



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
                return RedirectToAction("Unauthorised", "PassportForm", new { SPHostUrl = SharePointContextFilterAttribute.sphosturl  }); //SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            // var model = _db.tblPassportForm
            //     .Where(r => r.PF_Deleted == false)
            //     .Select(CheckValues).ToList<PassportVM>();

            return View();
            // return View(model.OrderBy(r =>r.VM_P_ViewColumn).OrderBy(e => e.VM_P_Initial_Location));
        }

      

        private static PassportVM CheckValues(PassportForm c)
        {

            PassportVM v = new PassportVM
            {
                VM_Id = c.PF_Id,
                VM_P_Name = c.PF_Name,
                VM_P_Initial_Location = c.PF_Initial_Location,
                VM_P_Passport_Number = c.PF_Passport_Number,
                VM_PF_Date_Of_Birth = c.PF_Date_Of_Birth,
                VM_PF_CaseId = c.PF_Case_Id,
                VM_PF_Date_HandedIn = c.PF_Date_Created,
                VM_PF_Date_Returned = c.PF_Date_Returned //need to figure out where this gets populated
            };

            switch (c.PF_Status.ToLower())
            {
                case "active":
                    v.VM_P_ViewColumn = "ACTIVE";
                    break;
                case "returned":
                    v.VM_P_ViewColumn = "RETURNED";
                    break;
                case "pending":
                    if (c.PF_Date_Returned == null && DateTime.Now > c.PF_Passport_Expiry_Date)
                        v.VM_P_ViewColumn = "PENDING TRANSFER EXPIRED";
                    else
                    if (c.PF_Date_Returned == null && DateTime.Now <= c.PF_Passport_Expiry_Date)
                        v.VM_P_ViewColumn = "PENDING TRANSFER";                
                    break;
                case "":
                    if (c.PF_Date_Returned == null && DateTime.Now > c.PF_Passport_Expiry_Date)
                        v.VM_P_ViewColumn = "PASSPORT EXPIRED CURRENTLY HELD";
                    else
                    if (c.PF_Date_Returned == null && DateTime.Now <= c.PF_Passport_Expiry_Date)
                        v.VM_P_ViewColumn = "CURRENTLY HELD";                   
                     break;
                default:
                     v.VM_P_ViewColumn = "MISC";
                     break;
            }


            //if (c.PF_Date_Returned == null && DateTime.Now > c.PF_Passport_Expiry_Date && string.IsNullOrEmpty(c.PF_Status))
            //    v.VM_P_ViewColumn = "PASSPORT EXPIRED CURRENTLY HELD";

            //if (c.PF_Date_Returned == null && DateTime.Now <= c.PF_Passport_Expiry_Date && string.IsNullOrEmpty(c.PF_Status))
            //    v.VM_P_ViewColumn = "CURRENTLY HELD";

            //if (c.PF_Date_Returned == null && DateTime.Now > c.PF_Passport_Expiry_Date && c.PF_Status.ToLower() == "pending")
            //    v.VM_P_ViewColumn = "PENDING TRANSFER EXPIRED";

            //if (c.PF_Date_Returned == null && DateTime.Now <= c.PF_Passport_Expiry_Date && c.PF_Status.ToLower() == "pending")
            //    v.VM_P_ViewColumn = "PENDING TRANSFER";

            //if (c.PF_Status.ToLower() == "returned")
            //    v.VM_P_ViewColumn = "RETURNED";

            //if (c.PF_Status.ToLower() == "active")
            //    v.VM_P_ViewColumn = "ACTIVE";

            return v;
        }


        [SharePointContextFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [SharePointContextFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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