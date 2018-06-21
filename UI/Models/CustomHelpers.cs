using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI
{
    public static class CustomHelpers
    {
        public static MvcHtmlString GetHour(this HtmlHelper help, DateTime dt1, DateTime dt2)
        {
            MvcHtmlString mhs = new MvcHtmlString("");
            TimeSpan fark = dt2 - dt1;

            if (fark.TotalHours < 1)
            {
                mhs = MvcHtmlString.Create("<div class=\"time-ago\">" + fark.Minutes + " Dakika</div>");
            }
            else if (fark.TotalHours > 1)
            {

                mhs = MvcHtmlString.Create("<div class=\"time-ago\"> " + fark.Hours + " Saat" +""+ fark.Minutes + " Dakika</div>");

            }
            return mhs;
        }
    }
}