using BLL;
using DAL.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UI.Models;
namespace UI.Controllers
{
    [RolesAttribute(2)]
    public class TrainerStudentController : Controller
    {

        StudentRepo stdRepo = new StudentRepo();
        ClassroomRepo clsRepo = new ClassroomRepo();
        SectionRepo sRepo = new SectionRepo();
        ExamOfStudentRepo exStdRepo = new ExamOfStudentRepo();
        ExamRepo exRepo = new ExamRepo();

        public ActionResult StudentList()
        {

            int trainerID = (int)Session["UserID"];

            SelectList classes = new SelectList(clsRepo.GetAll().Where(cls=>cls.TrainerID==trainerID),"Id","Name");
            ViewBag.Classrooms= classes;

            return View();
        }

        public ActionResult ExamResults(int id)
        {
            StudentExamsVM model = new StudentExamsVM();
            Student std = stdRepo.GetById(id);
            model.FirstName = std.FirstName;
            model.LastName = std.LastName;
            List<ExamOfStudent> examList = exStdRepo.GetAll().Where(x=>x.StudentID==id).ToList();


            foreach (Exam ex in exRepo.GetAll())
            {
                foreach (ExamOfStudent exStd in examList)
                {
                    if (exStd.ExamID == ex.Id)
                    {
                        model.ExamScoreList.Add(ex.Name, exStd.Score);
                    }
                }

            }

            return View(model);
        }



        public JsonResult StudentsByClassroom(int id)
        {
            int trainerID = (int)Session["UserID"];
            List<TrainerStudentListVM> stdList = new List<TrainerStudentListVM>();
            //stdList = stdRepo.GetAll();
            //stdList.AddRange(stdRepo.GetAll().Where(x => x.Classrooms.Any(cls => cls.TrainerID == trainerID && cls.Id == id)).Select(y => new TrainerStudentListVM
            //{

            //}).ToList());

            //Repository içine bu metotlar yerleştirilmeli

            foreach (Student std in stdRepo.GetAll())
            {
                foreach (Classroom cls in std.Classrooms)
                {
                    if (cls.Id == id)
                    {
                        stdList.Add(new TrainerStudentListVM
                        {
                            Id = std.Id,
                            FirstName = std.FirstName,
                            LastName = std.LastName,
                            Email=std.Email
                        });
                    }
                }
            }

            ViewBag.studentCount = stdList.Count;
            
            return Json(stdList,JsonRequestBehavior.AllowGet);
        }
    }
}