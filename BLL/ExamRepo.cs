using BLL.Interfaces;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ExamRepo : IRepository<Exam>
    {
        BasExamContext context;
        bool result = false;
        public List<Exam> GetAll()
        {
            List<Exam> examList = null;
            using (context = new BasExamContext())
            {
                examList = context.Exams.ToList();
            }
            return examList;
        }

        public Exam GetById(int id)
        {
            Exam exam = null;
            using (context = new BasExamContext())
            {
                exam = context.Exams.Find(id);
            }
            return exam;
        }

        public bool Add(Exam item)
        {
            using (context = new BasExamContext())
            {
                context.Entry(item).State = EntityState.Added;
                try
                {
                    result = context.SaveChanges() > 0;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool Edit(Exam item)
        {
            using (context = new BasExamContext())
            {
                context.Entry(item).State = EntityState.Modified;
                try
                {
                    result = context.SaveChanges() > 0;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool Delete(int id)
        {
            Exam exam = null;
            using (context = new BasExamContext())
            {
                exam = context.Exams.Find(id);

                context.Entry(exam).State = EntityState.Deleted;
                try
                {
                    result = context.SaveChanges() > 0;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
