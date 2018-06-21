using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    [RolesAttribute(0)]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            //HelperMetots.IsLogin();
            return View();
        }
    }
}