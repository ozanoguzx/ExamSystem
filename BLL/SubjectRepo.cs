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
    public class SubjectRepo : IRepository<Subject>
    {
        BasExamContext context;
        bool result = false;


        

        public bool Add(Subject item)
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
            using (context = new BasExamContext())
            {
                Subject trn = context.Subjects.Find(id);
                context.Entry(trn).State = System.Data.Entity.EntityState.Deleted;

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

        public bool Edit(Subject item)
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

        public List<Subject> GetAll()
        {
            List<Subject> Subjectlist = null;
            using (context = new BasExamContext())
            {
                try
                {
                    Subjectlist = context.Subjects.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return Subjectlist;
        }

        public List<Subject> GetDropdown()
        {
            List<Subject> Subjectlist = null;

            using (context = new BasExamContext())
            {
                try
                {
                    Subjectlist = context.Subjects.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return Subjectlist;

        }

        public Subject GetById(int id)
        {
            Subject sub = new Subject();
            using (context = new BasExamContext())
            {
                sub = context.Subjects.Find(id);
            }

            return sub;
        }
    }
}
