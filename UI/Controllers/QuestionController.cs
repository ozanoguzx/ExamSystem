using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL.Data;
using System.ComponentModel.DataAnnotations;
using UI.Models;
using PagedList;

namespace UI.Controllers
{

    
    [RolesAttribute(0,2)]
    public class QuestionController : Controller
    {
        QuestionsRepo quesRepo = new QuestionsRepo();
        SubjectRepo subRepo = new SubjectRepo();


        IEnumerable<SelectListItem> ProjectTypes()
        {

            return new List<SelectListItem>
            {
                 new SelectListItem { Text="Test",Value="1" },
                 new SelectListItem { Text="Proje",Value="2" }
            };

        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(int sayfa = 1)
        {

            //HelperMetots.IsLogin();
            BasExamContext db = new BasExamContext();
            List<Question> qlist = db.Questions.OrderByDescending(x=>x.CreatedDate).ToList();
            ViewBag.QuestionCount = qlist.Count;
            return View(qlist.ToPagedList(sayfa, 20));
        }

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    Question s = quesRepo.GetById(id);
        //    if (s.Deleted==true)
        //    {
        //        s.Deleted =false;
        //    }
        //    else
        //    {
        //        s.Deleted = true;
        //    }
        //    quesRepo.Edit(s);
        //    BasExamContext db = new BasExamContext();
        //    QuestionVM mdl = new QuestionVM();
        //    List<Question> questionList = db.Questions.ToList();
        //    mdl.QuestionList = questionList;
        //    mdl.TotalQuestion = questionList.Count;

        //    return Index(mdl);
        //}
        public ActionResult Delete(int id)
        {
            Question s = quesRepo.GetById(id);
            if (s.Deleted==true)
            {
                s.Deleted =false;
            }
            else
            {
                s.Deleted = true;
            }
            quesRepo.Edit(s);
            BasExamContext db = new BasExamContext();
            QuestionVM mdl = new QuestionVM();
            List<Question> questionList = db.Questions.ToList();
            mdl.QuestionList = questionList;
            mdl.TotalQuestion = questionList.Count;

            return Index(mdl);
        }

        [HttpPost]
        public ActionResult Index(QuestionVM model)
        {
            return View();
        }


        [HttpGet]
        public ActionResult Add([System.ComponentModel.DefaultValue(0)]int id)
        {
            //HelperMetots.IsLogin();
            Question ques = quesRepo.GetById(id);

            QuestionVM mdl = new QuestionVM();
            mdl.SubjectList = (subRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })).ToList();

            //mdl.QuestionType = new List<SelectListItem>
            //{    new SelectListItem { Text="Seçiniz",Value="0" },
            //     new SelectListItem { Text="Test",Value="1" },
            //     new SelectListItem { Text="Proje",Value="2" }
            //};

            mdl.AnswerDropDown = new List<SelectListItem>
            {    new SelectListItem { Text="Seçiniz",Value="" },
                 new SelectListItem { Text="A Seçeneği",Value="A" },
                 new SelectListItem { Text="B Seçeneği",Value="B" },
                 new SelectListItem { Text="C Seçeneği",Value="C" },
                 new SelectListItem { Text="D Seçeneği",Value="D" },
                 new SelectListItem { Text="E Seçeneği",Value="E" },
            };

            if (ques != null)
            {
                mdl.A = ques.A;
                mdl.B = ques.B;
                mdl.C = ques.C;
                mdl.D = ques.D;
                mdl.E = ques.E;
                mdl.Id = ques.Id;
                //mdl.Type = ques.Type;
                mdl.RightAnswer = ques.RightAnswer;
                //mdl.UserID = ques.UserID;
                mdl.SubjectID = ques.SubjectID;
                mdl.Content = ques.Content;
                mdl.IsUpdate = true;
                mdl.Deleted = ques.Deleted;
            }
            else
            {
                mdl.IsUpdate = false;
            }

            return View(mdl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(QuestionVM model)
        {
            if (ModelState.IsValid)
            {
                Question question;
                if (model.Id > 0)
                {
                    question = quesRepo.GetByIDs(model.Id);
                }
                else
                {
                    question = new Question();
                }
                question.CreatedDate = DateTime.UtcNow;
                question.TrainerID = (int)Session["UserID"];
                question.A = model.A;
                question.B = model.B;
                question.C = model.C;
                question.D = model.D;
                question.E = model.E;
                question.RightAnswer = model.RightAnswer;
                //question.Type = model.Type;
                question.SubjectID = model.SubjectID;
                question.Content = model.Content;
                //question.UserID = Convert.ToInt32(Session["UserID"]);
                question.Deleted = false;

                if (model.IsUpdate)
                {
                    if (quesRepo.Edit(question))
                    {
                        ViewBag.Update = true;
                        ViewBag.Message = true;
                    }
                    else
                    {
                        ViewBag.Message = false;
                    }

                }
                else
                {
                    if (quesRepo.Add(question))
                    {
                        ViewBag.Update = false;
                        ViewBag.Message = true;
                    }
                    else
                    {
                        ViewBag.Message = false;
                    }
                }
            }
            else
            {
                return Add(0);
            }
            model = null;
            return Add(0);

        }

    }
}