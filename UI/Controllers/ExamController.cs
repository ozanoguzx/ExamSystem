using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Data;
using UI.Models;
using System.Data.Entity.Validation;


namespace UI.Controllers
{
    [RolesAttribute(0)]
    public class ExamController : Controller
    {
        BasExamContext context = new BasExamContext();
        SectionRepo sectionrepo = new SectionRepo();
        ClassroomRepo cRepo = new ClassroomRepo();
        SubjectRepo sRepo = new SubjectRepo();
        ExamRepo eRepo = new ExamRepo();
        ExamOfStudentRepo eIslem = new ExamOfStudentRepo();
        public ActionResult Index()
        {
            //HelperMetots.isAdmin();

            return View();
        }

        public List<Question> GetQuestions(int subjectID, int questionCount)
        {
            //List<Question> questionList = new List<Question>();
            Subject subject = context.Subjects.Find(subjectID);
            string subjectName = subject.Name;
            var questions = (from q in context.Questions
                             where q.SubjectID == subjectID
                             orderby Guid.NewGuid()
                             select q).Skip(1).Take(questionCount);

            return questions.ToList();
        }

        [HttpGet]
        public ActionResult Add()
        {
            //HelperMetots.isAdmin();
            TempData["subject"] = sRepo.GetAll();
            TempData["classroom"] = cRepo.GetAll();

            return View();
        }

        [HttpPost]
        public ActionResult Add(Exam item, string classroomName, FormCollection fcl)
        {
            Classroom cRoom = context.Classrooms.Where(x => x.Name == classroomName).FirstOrDefault();
            List<Question> questionList = new List<Question>();
            List<ExamQuestion> examQuestionList = new List<ExamQuestion>();
            ExamQuestion examQuestion;
            //int intExamType = Convert.ToInt32(examType);
            int itemCount = fcl.Count;
            var keys = Request.Form.AllKeys;

            //Sınav Ekleme 
            //Öncelikle sınav ekliyoruz daha sonra sınavın konularını ve sorularını ekliyoruz.
            if (cRoom != null && Session["UserID"] != null)
            {
                item.Name = Request.Form.Get(keys[0]);
                item.ClassroomID = cRoom.Id;
                //item.Type = Convert.ToInt32(examType);
                item.TrainerID = (int)Session["UserID"];
                item.State = true;

                //Eklenecek olan sınavın daha önce kaydedilip kaydedilmediğini kontrol eder
                if (context.Exams.Any(x => x.Name == item.Name && x.ClassroomID == cRoom.Id))
                {
                    TempData["msg"] = "Sınav ekleme başarısız!!! " + cRoom.Name + " Sınıfının " + item.Name + "`ı daha önce yapılmıştır";
                    return RedirectToAction("Add");
                }
                //Veritabanında istenilen soru adedinin konu bazında yeterli olup olmadığını kontrol eder.
                for (int i = 2; i < itemCount - 4; i++)
                {
                    Subject subject = context.Subjects.Find(Convert.ToInt32(keys[i]));
                    int numberOfQuestion = context.Questions.Where(x => x.SubjectID == subject.Id).Count();

                    if (numberOfQuestion < Convert.ToInt32(Request.Form.Get(keys[i])))
                    {
                        TempData["msg1"] = "Sınav ekleme başarısız!!! " + subject.Name + " konusunda istediğiniz miktarda soru bulunmamaktadır. Bu konuda veritabanında '" + numberOfQuestion + "' adet soru bulunmaktadır";
                        return RedirectToAction("Add");
                    }
                }
                context.Entry(item).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
                //List<Student> studentList = context.Students.Where(x => x.ClassroomID == item.ClassroomID)
                List<Student> studentList = context.Students.Where(x => x.Classrooms.Any(y => y.Id == item.ClassroomID)).ToList();
                foreach (Student student in  studentList)
                {
                    ExamOfStudent studentExam = new ExamOfStudent();
                    studentExam.ExamID = item.Id;
                    studentExam.StudentID = student.Id;
                    studentExam.Score = 0;
                    studentExam.IsActive = true;
                    context.Entry(studentExam).State = System.Data.Entity.EntityState.Added;
                }
            }

            //Sınav için formdan gelen subjectleri sınava ekler
            for (int i = 2; i < itemCount - 4; i++)
            {
                int subID = Convert.ToInt32(keys[i]);
                Subject sub = context.Subjects.Find(subID);
                item.Subjects.Add(sub);
            }

            //Sınavın sorularını sınava ekler
            for (int i = 2; i < itemCount - 4; i++)
            {
                questionList = GetQuestions(Convert.ToInt32(keys[i]), Convert.ToInt32(Request.Form.Get(keys[i])));
                foreach (Question q in questionList)
                {
                    examQuestion = new ExamQuestion();
                    examQuestion.QuestionID = q.Id;
                    examQuestion.ClassroomID = cRoom.Id;
                    examQuestion.ExamID = item.Id;
                    examQuestionList.Add(examQuestion);
                }
            }
            foreach (ExamQuestion e in examQuestionList)
            {
                context.Entry(e).State = System.Data.Entity.EntityState.Added;
            }
            try
            {
                if (context.SaveChanges() > 0)
                {
                    TempData["message"] = true;
                }
            }
            catch 
            {
                throw;
            }
            return RedirectToAction("Add");
        }

        public ActionResult Delete(int id)
        {
            Exam exam = context.Exams.Find(id);
            List<ExamQuestion> examQuestions = context.ExamQuestions.Where(x => x.ExamID == id).ToList();
            List<ExamOfStudent> examOfStudent = context.ExamOfStudents.Where(x => x.ExamID == exam.Id).ToList();
            List<QuestionOfStudent> qOfStudentList = new List<QuestionOfStudent>();
            QuestionOfStudent qOfStudent = new QuestionOfStudent();
            foreach (ExamOfStudent item in examOfStudent)
            {
                qOfStudentList = context.QuestionOfStudents.Where(x => x.ExamOfStudentID == item.Id).ToList();
                if (context.QuestionOfStudents.Where(x => x.ExamOfStudentID == item.Id).ToList().Count > 0)
                {
                    TempData["IsDelete"] = true;
                    return RedirectToAction("Examlist");
                }
                context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                //exam.ExamOfStudents.Remove(item);
            }
            foreach (Subject item in exam.Subjects.ToList())
            {
                exam.Subjects.Remove(item);
            }

            foreach (ExamQuestion item in examQuestions)
            {
                context.ExamQuestions.Remove(item);
            }
            context.SaveChanges();
            context.Exams.Remove(exam);
            context.SaveChanges();

            return RedirectToAction("Examlist");
        }
        public ActionResult Passive(int id)
        {
            Exam exam = context.Exams.Find(id);
            exam.State = false;
            //ExamOfStudent eOStudent;
            //List<ExamOfStudent> examOfStudent = eIslem.GetAll().Where(x => x.Score == 123 && x.IsActive == true).ToList();
            //for (int i = 0; i < examOfStudent.Count; i++)
            //{
            //    eOStudent = new ExamOfStudent();
            //    eOStudent = examOfStudent[i];
            //    eOStudent.Score = 0;
            //    eOStudent.IsActive = false;
            //    eIslem.Edit(eOStudent);
            //}
            context.SaveChanges();
            return RedirectToAction("Examlist");
        }
        public ActionResult Examlist()
        {
            ViewBag.Classroom = cRepo.GetAll();
            List<Exam> examList = context.Exams.OrderBy(x => x.Id).ToList();

            return View(examList);
        }

        [HttpPost]
        public ActionResult Examlist(string classroom, string state)
        {
            Classroom c = new Classroom();
            List<Exam> examList = new List<Exam>();
            bool status = true;

            if (state == "1")
                status = true;
            else if (state == "2")
                status = false;

            if (classroom != "Seçiniz")
                c = context.Classrooms.Where(x => x.Name == classroom).Single();

            if (classroom != "Seçiniz" && Convert.ToInt32(state) != 0)
                examList = context.Exams.Where(x => x.State == status && x.ClassroomID == c.Id).OrderBy(x => x.Id).ToList();
            else if (classroom != "Seçiniz" && Convert.ToInt32(state) == 0)
                examList = context.Exams.Where(x => x.ClassroomID == c.Id).OrderBy(x => x.Id).ToList();
            else if (classroom == "Seçiniz" && Convert.ToInt32(state) != 0)
                examList = context.Exams.Where(x => x.State == status).OrderBy(x => x.ExamDate).ToList();
            else
                examList = context.Exams.OrderBy(x => x.Id).ToList();

            ViewBag.Classroom = context.Classrooms.ToList();

            return View(examList);
        }
        [HttpGet]
        public ActionResult ExamResult()
        {
            //BasExamContext context = new BasExamContext();
            ExamVM Exvm = new ExamVM();
            //List<Exam> Examliste = context.Exams.ToList();
            //Exvm.Examlist = Examliste;
            Exvm.ClassList = cRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            }).ToList();
            Exvm.SectionList = sectionrepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(Exvm);
        }
        [HttpPost]
        public ActionResult ExamResult(ExamVM model)
        {
            BasExamContext context = new BasExamContext();
            ExamVM Exvm = new ExamVM();
            List<Exam> Examliste = context.Classrooms.Find(model.ClassroomID).Exams.ToList();
            Exvm.Examlist = Examliste;

            Exvm.ClassList = cRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            }).ToList();
            Exvm.SectionList = sectionrepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View(Exvm);
        }
        [HttpGet]
        public ActionResult StudentResult(int id)
        {
            BasExamContext context = new BasExamContext();
            List<ExamOfStudent> examstudentlist = context.ExamOfStudents.Where(x => x.ExamID == id).ToList();
            return View(examstudentlist);
        }

        [HttpGet]
        public ActionResult StudentRigth(int id)
        {
            context = new BasExamContext();
            List<QuestionOfStudent> questions = context.QuestionOfStudents.Where(x => x.ExamOfStudentID == id).ToList();
            return View(questions);
        }
    }
}
