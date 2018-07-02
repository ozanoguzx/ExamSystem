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
    [RolesAttribute(0)]
    public class ClassroomController : Controller
    {
        ClassroomRepo clsRepo = new ClassroomRepo();
        TrainerRepo trnRepo = new TrainerRepo();

        public ActionResult ClassroomList(int sayfa = 1)
        {
            List<Classroom> clsList = clsRepo.GetAll().Where(classroom => classroom.IsActive == true).OrderByDescending(s => s.CreatedDate).ToList();
            ViewBag.ClassroomCount = clsList.Count;
            return View(clsList.ToPagedList(sayfa, 20));
        }

        public ActionResult Delete(int id)
        {
            Classroom cls = clsRepo.GetById(id);
            cls.IsActive = false;
            clsRepo.Edit(cls);
            //stdRepo.Delete(id);
            return RedirectToAction("ClassroomList", "Classroom");
        }

        public ActionResult Add()
        {
            TempData["classroom"] = clsRepo.GetAll();
            TempData["trainer"] = trnRepo.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Classroom data)
        {
            Classroom cls = new Classroom();

            cls.Name = data.Name;
            cls.CreatedDate = data.CreatedDate;
            cls.ClosedDate = data.ClosedDate;

            if (data.TrainerID == null)
            {
                cls.TrainerID = 1011;
            }
            else
            {
                cls.TrainerID = data.TrainerID;
            }

            cls.SectionID = 1000;
            cls.IsActive = true;

            clsRepo.Add(cls);

            return RedirectToAction("ClassroomList", "Classroom");
        }


        [HttpGet]
        public ActionResult Update(int id)
        {
            TempData["trainer"] = trnRepo.GetAll();
            Classroom cls = clsRepo.GetById(id);
            ClassroomVM model = new ClassroomVM {
               Name=cls.Name,
               ClosedDate=(DateTime)cls.ClosedDate,
               CreatedDate=(DateTime)cls.CreatedDate,
               ID=cls.Id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ClassroomVM data)
        {

            Classroom cls = clsRepo.GetById(data.ID);
            cls.Name = data.Name;
            cls.TrainerID = data.TrainerID;
            cls.CreatedDate = data.CreatedDate;
            cls.ClosedDate = data.ClosedDate;
            clsRepo.Edit(cls);
            return RedirectToAction("ClassroomList","Classroom");
        }





    }
}