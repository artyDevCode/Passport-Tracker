using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportTracker.Helpers
{
    public static class ShortTimeFormat
    {
        public static IHtmlString ShortTime(this HtmlHelper helper, DateTime thisTime, string name, bool edit)
        {
            //DateTime.ParseExact( model.StartDateTime, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");)

            var myTime = thisTime.ToString("hh:mm tt");
            
            if (edit)
                return new MvcHtmlString(string.Format("<input type='text' name='{1}' id='{1}' style='width:100%;' value='{0}'>", myTime, name));
            else
                return new MvcHtmlString(string.Format("<p style='text-align: center' id='{1}'>{0}</p>", myTime, name));
        }
    }
}