using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class StudentExamsVM
    {
        public StudentExamsVM()
        {
            ExamScoreList = new Dictionary<string, int>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Dictionary<string, int> ExamScoreList { get; set; }
    }
}