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
   public class ClassroomRepo:IRepository<Classroom>
    {
        BasExamContext context;
        bool result = false;

        public bool Add(Classroom item)
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
                Classroom cls = context.Classrooms.Find(id);
                context.Entry(cls).State = System.Data.Entity.EntityState.Deleted;

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

        public bool Edit(Classroom item)
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

        public List<Classroom> GetAll()
        {
            List<Classroom> Classroomlist;
            using (context = new BasExamContext())
            {
                Classroomlist = context.Classrooms.ToList();
            }

            return Classroomlist;
        }
        public Classroom GetById(int id)
        {
            Classroom clr = new Classroom();
            using (context = new BasExamContext())
            {
                clr = context.Classrooms.Find(id);
            }

            return clr;
        }

    }
}
