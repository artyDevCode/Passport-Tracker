using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PassportTracker.Helpers
{
    public static class ShortDateFormat
    {                                       
       // public static MvcHtmlString ShortDate<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
         public static IHtmlString ShortDate(this HtmlHelper helper, DateTime thisDate, string name,  bool edit)
        {
            var myDate = thisDate.ToString("dd-MMM-yyyy");   //thisDate.ToShortDateString().ToString();
           // TValue valueOfBar = expression.Compile()(html.ViewData.Model); //thisDate.ToShortDateString().ToString();
            if (edit)
                return new MvcHtmlString(string.Format("<input type='text' name='{1}' id='{1}' class='datepicker' style='width:100%;' value='{0}'>", myDate, name));
            else
                return new MvcHtmlString(string.Format("<p style='text-align: center' id='{1}'>{0}</p>", myDate, name));
          //  return ShortDate<TModel, TValue>(html, expression);
             
        }
    }
}