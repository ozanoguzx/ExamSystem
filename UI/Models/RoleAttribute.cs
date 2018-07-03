using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Models
{
    public class RolesAttribute : AuthorizeAttribute
    {
        private int Role1=1000;
        private int Role2=1000;

        public RolesAttribute(int role)
        {
            this.Role1 = role;
        }
        public RolesAttribute(int role1,int role2)
        {
            this.Role1 = role1;
            this.Role2 = role2;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["UserType"] != null)
            {

                if (this.Role1 == Convert.ToInt32(HttpContext.Current.Session["UserType"]) || this.Role2 == Convert.ToInt32(HttpContext.Current.Session["UserType"]))
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