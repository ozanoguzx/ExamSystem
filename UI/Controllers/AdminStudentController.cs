using BLL;
using DAL.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.Models.DTO;

namespace UI.Controllers
{
    [RolesAttribute(0)]
    public class AdminStudentController : Controller
    {
        StudentRepo stdRepo = new StudentRepo();
        ClassroomRepo clsRepo = new ClassroomRepo();
        SectionRepo sRepo = new SectionRepo();




        #region Admin Öğrenci İşlemleri

        public ActionResult StudentList(int sayfa = 1)
        {
            List<Student> stdList = stdRepo.GetAll().Where(student => student.IsActive == true).OrderByDescending(s=>s.CreatedDate).ToList();
            ViewBag.QuestionCount = stdList.Count;
            return View(stdList.ToPagedList(sayfa, 20));
        }


        public ActionResult Delete(int id)
        {
            Student std = stdRepo.GetById(id);
            std.IsActive = false;
            stdRepo.Edit(std);
            //stdRepo.Delete(id);
            return RedirectToAction("StudentList", "AdminStudent");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            TempData["classroom"] = clsRepo.GetAll();
            
            Student std = stdRepo.GetById(id);
            StudentVM model = new StudentVM {
                ID=std.Id,
                FirstName=std.FirstName,
                LastName=std.LastName,
                Email=std.Email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(StudentVM data)
        {
            Student std = stdRepo.GetById(data.ID);
            std.FirstName = data.FirstName;
            std.LastName = data.LastName;
            std.Email = data.Email;
            stdRepo.Edit(std);
            return RedirectToAction("StudentList","AdminStudent");
        }


        public ActionResult Add()
        {
            TempData["classroom"] = clsRepo.GetAll();
            TempData["section"] = sRepo.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(StudentVM data)
        {
            Student std = new Student();
            std.FirstName = data.FirstName;
            std.LastName = data.LastName;
            std.Email = data.Email;
            std.Classrooms.Add(clsRepo.GetById(data.ClassroomID));
            std.IsActive = true;
            std.UserName = data.FirstName.Substring(0, 2) + "." + data.LastName;
            std.Password = data.FirstName.Substring(0, 2) + "." + data.LastName + "123";
            std.CreatedDate = DateTime.UtcNow;
            
            stdRepo.Add(std);
            return RedirectToAction("StudentList","AdminStudent");
        }





        #endregion
    }
}