using BLL;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Controllers;
using UI.Models;

namespace UI.Views
{
    public class HomeController : Controller
    {
        TrainerRepo tRepo = new TrainerRepo();
        ClassroomRepo clasRepo = new ClassroomRepo();
        StudentRepo sRepo = new StudentRepo();
        QuestionsRepo qRepo = new QuestionsRepo();
        SubjectRepo subRepo = new SubjectRepo();
        Student student = new Student();
        Trainer trainer = new Trainer();
        Trainer admin = new Trainer();

        public class ChartClass
        {
            public int value { get; set; }
            public string color { get; set; }
            public string highlight { get; set; }
            public string label { get; set; }

        }

        public string ColorCode()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        public static List<ChartClass> chartList = new List<ChartClass>();
        public List<ChartClass> ChartValue()
        {
            List<Subject> liste = subRepo.GetAll();

            foreach (var item in liste)
            {
                int count = qRepo.GetAll().Where(p => p.SubjectID == item.Id).ToList().Count;
                if (count > 0)
                {
                    ChartClass c = new ChartClass();
                    c.value = count;
                    c.highlight = "#006391";
                    c.label = item.Name;
                    c.color = "#0072A5";
                    chartList.Add(c);
                }
            }
            return chartList;
        }

        public static List<ChartClass> chartListUser = new List<ChartClass>();
        public List<ChartClass> ChartUserValue()
        {
            int studentCount = sRepo.GetAll().ToList().Count;
            int trainerCount = tRepo.GetAll().ToList().Count;
            ChartClass c = new ChartClass();
            c.value = studentCount;
            c.highlight = "#00BEBB";
            c.label = "Öğrenciler";
            c.color = "#1CCDCA";
            chartListUser.Add(c);

            ChartClass c1 = new ChartClass();
            c1.value = trainerCount;
            c1.highlight = "#868686";
            c1.label = "Eğitmenler";
            c1.color = "#868686";
            chartListUser.Add(c1);

            return chartListUser;
        }

        public JsonResult GetData()
        {
            List<string> lstTrainer = new List<string>();
            List<int> lstCount = new List<int>();
            List<Trainer> liste = tRepo.GetAll();
            foreach (var item in liste)
            {
                lstTrainer.Add(item.FullName);
                int count = qRepo.GetAll().Where(p => p.TrainerID == item.Id).ToList().Count();
                lstCount.Add(count);
            }
            var BarChart = new
            {
                Trainer = lstTrainer,
                Count = lstCount
            };

            return Json(BarChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassData()
        {
            List<ExamAvg> liste = Average.AverageExam(1003);
            List<string> lstClass = new List<string>();
            List<double> lstCount = new List<double>();
            //List<CLASSROOM> liste = clasRepo.GetAll();
            foreach (var item in liste)
            {
                lstClass.Add(item.examName);
                lstCount.Add(item.average);
            }
            var BarChart = new
            {
                Class = lstClass,
                Count = lstCount
            };

            return Json(BarChart, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetFullData()
        //{
        //    List<ExamAvg> liste = liste = Average.AverageOfAllExam();
        //    List<string> lstClass = new List<string>();
        //    List<double> lstCount = new List<double>();
        //    //List<CLASSROOM> liste = clasRepo.GetAll();
        //    foreach (var item in liste)
        //    {
        //        lstClass.Add(item.examName);
        //        lstCount.Add(item.average);
        //    }
        //    var BarChart = new
        //    {
        //        Class = lstClass,
        //        Count = lstCount
        //    };

        //    return Json(BarChart, JsonRequestBehavior.AllowGet);
        //}




        //QUESTİON LİST CHART
        public JsonResult GetChartData()
        {
            var polarData = chartList;
            polarData.Clear();
            polarData = ChartValue();
            return Json(polarData, JsonRequestBehavior.AllowGet);
            chartList.Clear();
            polarData.Clear();
        }

        //USER LİST CHART
        public JsonResult GetChartDataUser()
        {
            var UserData = chartListUser;
            UserData.Clear();
            UserData = ChartUserValue();
            return Json(UserData, JsonRequestBehavior.AllowGet);
            chartListUser.Clear();
            UserData.Clear();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserType"] != null)
            {
                if (Convert.ToInt32(Session["UserType"]) == 0)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (Convert.ToInt32(Session["UserType"]) == 1)
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Remove("UserName");
            Session.Remove("UserID");
            Session.Remove("UserType");
            return RedirectToAction("LogIn", "Home");
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            trainer = tRepo.GetAll().Where(x => x.Username == email && x.Password == password).FirstOrDefault();
            student = sRepo.GetAll().Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            admin = tRepo.GetAll().Where(x => x.Username == email && x.Password == password && x.IsSuperAdmin==true).FirstOrDefault();
            if (admin != null)
            {
                Session["UserID"] = admin.Id;
                Session["UserName"] = admin.FullName;
                Session["UserType"] = 0;
                return RedirectToAction("index", "Admin");
            }
            else if (student != null)
            {
                Session["UserID"] = student.Id;
                Session["UserName"] = student.FirstName + " " + student.LastName;
                Session["UserType"] = 1;
                return RedirectToAction("Index", "Student");
            }
            else if (trainer!=null)
            {
                Session["UserID"] = trainer.Id;
                Session["UserName"] = trainer.FullName;
                Session["UserType"] = 2;
                return RedirectToAction("index", "Trainer");
            }
            else
            {
                ViewBag.Message = false;
            }

            return View();
        }

       
    }
}