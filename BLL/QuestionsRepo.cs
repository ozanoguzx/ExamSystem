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
    public class QuestionsRepo : IRepository<Question>
    {
        BasExamContext context;
        bool result = false;
        public bool Add(Question item)
        {
            using (context = new BasExamContext())
            {
                context.Entry(item).State = EntityState.Added;
                try
                {
                    result = context.SaveChanges() > 0;
                }
                catch 
                {
                    return result;
                }
            }

            return result;
        }

        public bool Delete(int id)
        {
            Question question = new Question();
            using (context = new BasExamContext())
            {
                question = context.Questions.Find(id);
                context.Entry(question).State = EntityState.Deleted;
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

        public bool Edit(Question item)
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

        public List<Question> GetAll()
        {
            List<Question> questionList = null;
            using (context = new BasExamContext())
            {
                questionList = context.Questions.ToList();
            }

            return questionList;
        }

        public Question GetById(int id)
        {
            Question question = null;

            using (context = new BasExamContext())
            {
                question = context.Questions.Find(id);
            }
            return question;
        }

        public Question GetByIDs(long id)
        {
            Question question = null;

            using (context = new BasExamContext())
            {
                question = context.Questions.Find(id);
            }
            return question;
        }
    }
}
