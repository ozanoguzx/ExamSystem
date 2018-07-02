using BLL;
using DAL.Data;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UI.Models;
using UI.Models.DTO;

namespace UI.Controllers
{
    [RolesAttribute(1)]
    public class StudentController : Controller
    {
        BasExamContext db = new BasExamContext();
        private static Dictionary<string, string> answers;
        StudentRepo stdRepo = new StudentRepo();
        //Sınav işlemleri için kullanılır.
        ExamRepo erepo = new ExamRepo();
        //Soru İşlemleri için kullanılır.
        QuestionsRepo qrepo = new QuestionsRepo();
        //Öğrencinin verdiği cevapları kaydetmek ve gerekli işlemleri yapmak için kullanılır.
        QuestionOfStudentRepo quesRepo = new QuestionOfStudentRepo();
        //Öğrenciye ait sınav oluşturmak için kullanılır.Puan burada tutulur.
        ExamOfStudentRepo eIslem = new ExamOfStudentRepo();
        //Öğrenciye ait sınav burada yakalanır
        Exam examlist = new Exam();
        //Sınava ait soruları tutmaya yarar.

        ClassroomRepo classRepo = new ClassroomRepo();

        ExamQuestionsRepo eQuestionRepo = new ExamQuestionsRepo();
        //Static Kısmı düzenlenecek.
        static List<Question> quesPool = new List<Question>();
        //static Dictionary<int, Question> havuz = new Dictionary<int, Question>();

        //Sınav bittikten sonra sonucu examScore`da tutuluyor ki kullanıcıya gösterilsin.
        static private double examScore = 0;
        //Sınav içerisinde ileri ve geri işlemleri yaparken bu değişkene göre işlem yapılır.
        //static int state = 0;
        static List<ExamQuestion> examQuestions;
        #region Sınav İşlemleri
        DateTime ExamEnd;
        private static int examID;
        public ActionResult Exam(int id)
        {
            //examID = id;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            Exam exam = erepo.GetById(id);
            if (exam != null && exam.ExamEndTime > DateTime.Now && exam.ExamStartTime < DateTime.Now)
            {
                Session.Add("ExamTempID", id);
                Student student = stdRepo.GetById(Convert.ToInt32(Session["UserID"]));
                ExamOfStudent examOfStudent = eIslem.GetAll().Where(x => x.StudentID == student.Id && x.IsActive == true && x.ExamID == id).SingleOrDefault();
                if (examOfStudent != null)
                {
                    Session.Add("ExamID", examOfStudent.ExamID);
                    Session.Add("ExamOfStudentID", examOfStudent.Id);
                }

                if (Session["UserID"] != null && Session["ExamOfStudentID"] != null)
                {
                    examQuestions = new List<ExamQuestion>();
                    examQuestions = eQuestionRepo.GetAll().Where(x => x.ExamID == id).ToList();
                    QuestionDTO questionDTO = new QuestionDTO();
                    List<Question> questionList = new List<Question>();
                    questionList = StripQuestions(examQuestions);
                    ExamRepo eRepo = new ExamRepo();
                    Exam Sinav = eRepo.GetById(id);
                    ExamEnd = Convert.ToDateTime(Sinav.ExamEndTime);
                    Session["ExamEnd"] = ExamEnd;
                    TempData["ExamName"] = exam.Name;
                    return View(questionDTO.QuestionToDTO(questionList));
                }
            }
            return RedirectToAction("NoExamError", "Error");
        }
        #region Öğrenciye göre soru sıralaması


        //Öğrenciye göre soruları karıştır.
        private List<Question> StripQuestions(List<ExamQuestion> examQuestions)
        {
            List<Question> questions = new List<Question>();
            Question question;
            foreach (ExamQuestion item in examQuestions)
            {
                question = qrepo.GetByIDs(item.QuestionID);
                questions.Add(question);
            }

            var questionList = (from q in questions
                                orderby Guid.NewGuid()
                                select q).Take(questions.Count);

            return questionList.ToList();
        }
        #endregion
        public void AddAnswer(string answer)
        {
            string[] parses = answer.Split('-');
            //int key = Convert.ToInt32(parses[0]);
            string key = parses[0];
            string value = parses[1];
            //1025.25-A
            //1026.25-D
            if (answers == null)
                answers = new Dictionary<string, string>();

            if (!answers.ContainsKey(key))
            {
                answers.Add(key, value);
            }
            else
            {
                answers.Remove(key);
                answers.Add(key, value);
            }
        }
        [HttpGet]
        public ActionResult SaveExam()
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            Question question;
            QuestionOfStudent quesOfStudent;
            double rightCount = 0;
            ExamOfStudent examOfStudent = new ExamOfStudent();
            examOfStudent = eIslem.GetAll().Where(x => x.StudentID == Convert.ToInt32(Session["UserId"]) && x.IsActive == true && x.ExamID == examID).FirstOrDefault();
            double examResult = 0;

            try
            {
                int Id = (int)Session["UserID"];
                foreach (KeyValuePair<string, string> item in answers)
                {
                    string[] parses = item.Key.Split('.');
                    int userID = Convert.ToInt32(parses[0]);
                    int key = Convert.ToInt32(parses[1]);
                    string value = item.Value;

                    if (userID == Id)
                    {
                        quesOfStudent = new QuestionOfStudent();
                        quesOfStudent.QuestionID = key;
                        quesOfStudent.AnswerOfStudent = value;
                        quesOfStudent.ExamOfStudentID = examOfStudent.Id;

                        question = new Question();
                        question = qrepo.GetById(key);

                        if (question.RightAnswer == value)
                        {
                            rightCount++;
                            quesOfStudent.IsTrue = true;
                        }
                        else
                        {
                            quesOfStudent.IsTrue = false;
                        }
                        quesRepo.Add(quesOfStudent);
                    }

                    //quesOfStudent = new QuestionOfStudent();
                    //quesOfStudent.QuestionID = item.Key;
                    //quesOfStudent.AnswerOfStudent = item.Value;
                    //quesOfStudent.ExamOfStudentID = examOfStudent.Id;

                    //question = new Question();
                    //question = qrepo.GetById(item.Key);

                    //if (question.RightAnswer == item.Value)
                    //{
                    //    rightCount++;
                    //    quesOfStudent.IsTrue = true;
                    //}
                    //else
                    //{
                    //    quesOfStudent.IsTrue = false;
                    //}
                    //quesRepo.Add(quesOfStudent);
                }
                examResult = Math.Ceiling((100 / (double)examQuestions.Count) * rightCount);
                examOfStudent.Score = (byte)examResult;
                examOfStudent.IsActive = false;
                examScore = examResult;
                eIslem.Edit(examOfStudent);
                Session.Add("Score", examResult);
            }
            catch (Exception e)
            {
                return RedirectToAction("NoExamError", "Error", e.Message);
            }

            return RedirectToAction("Result");
        }
        #endregion
        #region Ilk
        //Burda hata olmuş olabilir tam kestiremedim.
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                int id = Convert.ToInt32(Session["UserId"]);
                ViewBag.ExOfStudent = eIslem.GetAll().Where(z => z.StudentID == id).ToList();
                return View(ExamFill(id));
            }
            return View();
        }

        List<Exam> ExamFill(int id)
        {

            List<Exam> examList = new List<Exam>();
            SqlConnection con = new SqlConnection("server=.;database=Bas;uid=sa;pwd=Password1;");

            SqlCommand cmd = new SqlCommand("select ex.Id, ex.TrainerID, ex.ClassroomID, ex.Name, ex.ExamDate, ex.ExamStartTime, ex.ExamEndTime, ex.[State], ex.CreatedDate, ex.ModifiedDate, ex.ModifiedBy from Exam EX join Classroom CR on ex.ClassroomID = cr.Id inner join  StudentToClassroom sc on CR.Id = sc.ClassroomID inner join Student st on sc.StudentID = st.Id where st.Id = @Id", con);

            cmd.Parameters.AddWithValue("@Id", id);
            if (con.State == System.Data.ConnectionState.Closed) con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            Exam exam;
            while (dr.Read())
            {
                if (dr.HasRows)
                {
                    exam = new Exam();
                    exam.Id = dr.GetInt32(0);

                    exam.TrainerID = dr.GetInt32(1);
                    exam.ClassroomID = dr.GetInt32(2);
                    exam.Name = dr.GetString(3);
                    exam.ExamDate = dr.GetDateTime(4);
                    exam.ExamStartTime = dr.GetDateTime(5);
                    exam.ExamEndTime = dr.GetDateTime(6);
                    exam.State = dr.GetBoolean(7);
                    //exam.CreatedDate = dr.GetDateTime(8);
                    //exam.ModifiedDate = dr.GetDateTime(9);
                    //exam.ModifiedBy = dr.GetInt32(10);

                    examList.Add(exam);

                }
            }
            dr.Close();
            if(con.State == System.Data.ConnectionState.Open) con.Close();

            return examList;
        }
        public ActionResult Information(int id)
        {
            examID = id;
            Student student = stdRepo.GetById(Convert.ToInt32(Session["UserID"]));
            ExamOfStudent examOfStudent = eIslem.GetAll().Where(x => x.StudentID == student.Id && x.IsActive == true && x.ExamID == examID).SingleOrDefault();
            if (examOfStudent != null)
            {
                Session.Add("ExamID", examOfStudent.ExamID);
            }
            //List<ExamQuestion> questionList = eQuestionRepo.GetAll().Where(x => x.ExamID == (int)Session["ExamID"] && x.ClassroomID == student.ClassroomID).ToList();
            List<ExamQuestion> questionList = eQuestionRepo.GetAll().Where(x => x.ExamID == (int)Session["ExamID"]).ToList();
            ViewBag.Score = Math.Round((100 / (decimal)questionList.Count), 1);

            examlist = db.Exams.FirstOrDefault(x => x.Id == id && x.State == true);
            return View(examlist);
        }
        public ActionResult AutomaticFinish()
        {
            return RedirectToAction("Result");
        }

        public ActionResult Result()
        {
            //if (Session["Score"] != null)
            //{
            Session.Remove("ExamOfStudentID");
            double result = 0;
            int GetQuesID = Convert.ToInt32(Session["ExamID"]);
            int studentID = Convert.ToInt32(Session["UserID"]);
            result = examScore;
            ExamOfStudent c = new ExamOfStudent();
            ExamOfStudent guncellenecek = new ExamOfStudent();
            if (Session["ExamID"] != null)
            {
                int idd = Convert.ToInt32(Session["ExamID"]);
                c = db.ExamOfStudents.Where(p => p.ExamID == GetQuesID && p.StudentID == studentID).FirstOrDefault();
                int cevaplanan = db.QuestionOfStudents.Where(p => p.ExamOfStudentID == GetQuesID).Count();
                if (c.Score == 123 && cevaplanan == 0)
                {
                    guncellenecek.Score = 0;
                    guncellenecek.Id = c.Id;
                    guncellenecek.IsActive = false;
                    guncellenecek.ExamID = c.ExamID;
                    guncellenecek.StudentID = c.StudentID;
                    eIslem.Edit(guncellenecek);
                }
                else
                {
                    byte examResult = Convert.ToByte(Math.Ceiling(result));
                    guncellenecek.Id = c.Id;
                    guncellenecek.IsActive = false;
                    guncellenecek.Score = examResult;
                    guncellenecek.ExamID = c.ExamID;
                    guncellenecek.StudentID = c.StudentID;
                    eIslem.Edit(guncellenecek);
                }
                Session.Remove("ExamID");
            }

            return View(guncellenecek);
            //}
            //else
            //{
            //    return RedirectToAction("NotExamResult", "Error");
            //}
        }

        public ActionResult PastExam()
        {
            if (Session["UserId"] != null)
            {
                int id = Convert.ToInt32(Session["UserId"]);
                StudentRepo srepo = new StudentRepo();
                Student student = srepo.GetById(id);
                BasExamContext context = new BasExamContext();

                List<ExamOfStudent> studentexamlist = context.ExamOfStudents.Where(x => x.StudentID == student.Id).ToList();

                return View(studentexamlist);
            }
            return View();
        }

        public ActionResult ExamStudentRigth(int id)
        {
            BasExamContext context = new BasExamContext();
            List<QuestionOfStudent> StudentQuestion = context.QuestionOfStudents.Where(x => x.ExamOfStudentID == id).ToList();
            return View(StudentQuestion);
        }
        public class Time
        {
            public int Hours { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }

        }
        [HttpGet]
        public JsonResult GetTime()
        {
            var Examend = Session["ExamEnd"].ToString();
            var examstart = DateTime.Now;
            TimeSpan fark = Convert.ToDateTime(Examend) - Convert.ToDateTime(examstart);
            if (fark.TotalHours < 1)
            {
                Time t = new Time();
                t.Hours = fark.Hours;
                t.Minute = fark.Minutes;
                t.Second = fark.Seconds;
                return Json(t, JsonRequestBehavior.AllowGet);
            }
            else if (fark.TotalHours > 1)
            {
                Time t = new Time();
                t.Hours = fark.Hours;
                t.Minute = fark.Minutes;
                t.Second = fark.Seconds;
                return Json(t, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion



    



    }
}