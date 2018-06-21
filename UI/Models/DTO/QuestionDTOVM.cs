using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.DTO
{
    public class QuestionDTOVM
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public long UserID { get; set; }
        public int SubjectID { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
    }
}