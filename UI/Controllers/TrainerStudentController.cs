using BLL;
using DAL.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    [RolesAttribute(2)]
    public class TrainerStudentController : Controller
    {

        StudentRepo stdRepo = new StudentRepo();
        ClassroomRepo clsRepo = new ClassroomRepo();
        SectionRepo sRepo = new SectionRepo();



        public ActionResult StudentList()
        {

            int trainerID = (int)Session["UserID"];

            SelectList classes = new SelectList(clsRepo.GetAll().Where(cls=>cls.TrainerID==trainerID),"Id","Name");
            ViewBag.Classrooms= classes;

            return View();
        }

        public JsonResult StudentsByClassroom(int id)
        {
            int trainerID = (int)Session["UserID"];
            List<Student> stdList = new List<Student>();
            stdList = stdRepo.GetAll();
            //stdList.AddRange( stdRepo.GetAll().Where(x => x.Classrooms.Any(cls => cls.TrainerID == trainerID && cls.Id== id)).ToList());
            ViewBag.studentCount = stdList.Count;

            
            return Json(stdList,JsonRequestBehavior.AllowGet);
        }


    }
}