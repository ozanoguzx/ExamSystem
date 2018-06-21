//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            this.ExamOfStudents = new HashSet<ExamOfStudent>();
            this.ExamQuestions = new HashSet<ExamQuestion>();
            this.Subjects = new HashSet<Subject>();
        }
    
        public int Id { get; set; }
        public Nullable<int> TrainerID { get; set; }
        public Nullable<int> ClassroomID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> ExamDate { get; set; }
        public Nullable<System.DateTime> ExamStartTime { get; set; }
        public Nullable<System.DateTime> ExamEndTime { get; set; }
        public Nullable<bool> State { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual Classroom Classroom { get; set; }
        public virtual Trainer Trainer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamOfStudent> ExamOfStudents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
