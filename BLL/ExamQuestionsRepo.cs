using BLL.Interfaces;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ExamQuestionsRepo : IRepository<ExamQuestion>
    {
        BasExamContext context;
        bool result = false;
        public bool Add(ExamQuestion item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(ExamQuestion item)
        {
            throw new NotImplementedException();
        }

        public List<ExamQuestion> GetAll()
        {
            context = new BasExamContext();
            return context.ExamQuestions.ToList();
        }

        public ExamQuestion GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
