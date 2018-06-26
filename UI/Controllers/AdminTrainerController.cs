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
    public class AdminTrainerController : Controller
    {
        TrainerRepo trnRepo = new TrainerRepo();
        ClassroomRepo clsRepo = new ClassroomRepo();  


        public ActionResult TrainerList(int sayfa = 1)
        {
            List<Trainer> trnList = trnRepo.GetAll().Where(trainer => trainer.IsActive == true).OrderByDescending(s => s.CreatedDate).ToList();
            ViewBag.TrainerCount = trnList.Count;
            return View(trnList.ToPagedList(sayfa, 20));
            
        }

        public ActionResult Delete(int id)
        {
            Trainer trn = trnRepo.GetById(id);
            trn.IsActive = false;
            trnRepo.Edit(trn);
            //stdRepo.Delete(id);
            return RedirectToAction("TrainerList", "AdminTrainer");
        }

        public ActionResult Add()
        {
            TempData["classroom"] = clsRepo.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(TrainerVM data)
        {
            Trainer trn = new Trainer();
            trn.FirstName = data.FirstName;
            trn.LastName = data.LastName;
            trn.EMail = data.Email;
            trn.IsActive = true;
            trn.Username = data.FirstName.Substring(0, 2) + "." + data.LastName;
            trn.Password = data.FirstName.Substring(0, 2) + "." + data.LastName + "123";
            trn.FullName = data.FirstName + " " + data.LastName;
            trn.CreatedDate = DateTime.UtcNow;

            trnRepo.Add(trn);
            return RedirectToAction("TrainerList", "AdminTrainer");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Trainer trn = trnRepo.GetById(id);
            TrainerVM model = new TrainerVM {
                ID=trn.Id,
                FirstName=trn.FirstName,
                LastName=trn.LastName,
                Email=trn.EMail
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(TrainerVM data)
        {
            Trainer std = trnRepo.GetById(data.ID);
            std.FirstName = data.FirstName;
            std.LastName = data.LastName;
            std.EMail = data.Email;
            trnRepo.Edit(std);
            return RedirectToAction("TrainerList", "AdminTrainer");
        }

    }
}