using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Models
{
    public class ExamVM
    {
        public int Id { get; set; }
        public int ClassroomID { get; set; }
        public int SectionID { get; set; }
        public string Name { get; set; }
        public int QuestionID { get; set; }
        public Nullable<int> RemainingTime { get; set; }
        public System.DateTime ExamDate { get; set; }
        public int StudentID { get; set; }
        public Nullable<int> ExamStartTime { get; set; }
        public Nullable<int> ExamEndTime { get; set; }

        public List<Exam> Examlist { get; set; }
        public List<Trainer> Trainerlist { get; set; }
        public List<SelectListItem> SectionList { get; set; }
        public List<SelectListItem> ClassList { get; set; }
    }
}