using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Models
{
	public class QuestionVM
	{
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen soru alanını doldurunuz")]
        public string Content { get; set; }
        public string RightAnswer { get; set; }
        [Required]
        public int TrainerID { get; set; }
        [Required]
        public int SubjectID { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public int TotalQuestion { get; set; }
        public List<Question> QuestionList { get; set; }
        //public List<SelectListItem> QuestionType { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
        public List<SelectListItem> AnswerDropDown { get; set; }
        public bool IsUpdate { get; set; }
        public bool? Deleted { get; set; }

    }
}