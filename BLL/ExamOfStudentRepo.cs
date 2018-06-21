using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Data;
using System.Data.Entity;
namespace BLL
{
  public  class ExamOfStudentRepo:IRepository<ExamOfStudent>
    {
        BasExamContext context;
        bool result = false;

        public bool Add(ExamOfStudent item)
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
                    return result;
                }
            }

            return result;
        }


        public bool Ekle_ReturnSonID(ExamOfStudent c, out int ID)
        {
            using (context = new BasExamContext())
            {
                ID = 0;
                context.Entry(c).State = EntityState.Added;
                context.SaveChanges();
                ID = c.Id;
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (context = new BasExamContext())
            {
                ExamOfStudent eos = context.ExamOfStudents.Find(id);
                context.Entry(eos).State = System.Data.Entity.EntityState.Deleted;

                try
                {
                    result = context.SaveChanges() > 0;
                }
                catch (Exception)
                {
                    return result;
                }
            }

            return result;
        }

        public bool Edit(ExamOfStudent item)
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
                    return result;
                }
            }

            return result;
        }

        public List<ExamOfStudent> GetAll()
        {
            List<ExamOfStudent> examofstudentlist;
            using (context = new BasExamContext())
            {
                examofstudentlist = context.ExamOfStudents.ToList();
            }

            return examofstudentlist;
        }
        public ExamOfStudent GetById(int id)
        {
            ExamOfStudent eos = new ExamOfStudent();
            using (context = new BasExamContext())
            {
                eos = context.ExamOfStudents.Find(id);
            }

            return eos;
        }


    }
}
