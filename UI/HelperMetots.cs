using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI
{
    public class HelperMetots
    {
        public static void IsLogin()
        {
            if (HttpContext.Current.Session["UserType"] == null | Convert.ToInt32(HttpContext.Current.Session["UserType"]) != 1)
            {
                HttpContext.Current.Response.Redirect("/Home/Login");
                HttpContext.Current.Response.End();
            }
        }
        
 
    }
}