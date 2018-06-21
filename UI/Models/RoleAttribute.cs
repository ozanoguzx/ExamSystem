using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Models
{
    public class RolesAttribute : AuthorizeAttribute
    {
        private int Role;
        public RolesAttribute(int role)
        {
            this.Role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["UserType"] != null)
            {
                if (this.Role == Convert.ToInt32(HttpContext.Current.Session["UserType"]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/Home/Login");
                return false;
            }
        }
    }
}