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
    public class QuestionOfStudentRepo : IRepository<QuestionOfStudent>
    {
        BasExamContext context;
        bool result = false;

        public bool Add(QuestionOfStudent item)
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

        public bool Delete(int id)
        {
            QuestionOfStudent ques = null;
            using (context = new BasExamContext())
            {
                ques = context.QuestionOfStudents.Find(id);
                context.Entry(ques).State = EntityState.Deleted;
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

        public bool Edit(QuestionOfStudent item)
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

        public List<QuestionOfStudent> GetAll()
        {
            List<QuestionOfStudent> quesList = null;
            using (context = new BasExamContext())
            {
                quesList = context.QuestionOfStudents.ToList();
            }
            return quesList;
        }

        public QuestionOfStudent GetById(int id)
        {
            QuestionOfStudent ques = null;
            using (context = new BasExamContext())
            {
                ques = context.QuestionOfStudents.Find(id);
            }

            return ques;
        }
    }
}
