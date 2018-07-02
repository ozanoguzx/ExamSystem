using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class ClassroomVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TrainerID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}