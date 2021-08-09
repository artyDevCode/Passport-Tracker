using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportTracker.Helpers
{
    public static class ReadOnlyInput
    {  //ie:   ReadOnlyInputFor(Model.PF_Date_Created, "PF_Date_Created", "datetime")
        public static IHtmlString ReadOnlyInputFor<T>(this HtmlHelper helper,  T val, string name, string text)
        {
            string isDate=null;
           if (typeof(T) == typeof(DateTime))
               isDate = ((DateTime)(object)val).ToString("dd-MMM-yyyy");

           return new MvcHtmlString(string.Format("<input style='width:100%; border:none; background-color:transparent;' class='text-box single-line' id='{0}' name='{0}' type='{1}' readonly='readonly' value='{2}' />", name, text, isDate == null ? val.ToString() : isDate));

        }
    }
}