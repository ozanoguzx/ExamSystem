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
   public class PresantationRepo:IRepository<Presentation>
    {
        BasExamContext context;
        bool result = false;

        public bool Add(Presentation item)
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
                Presentation trn = context.Presentations.Find(id);
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

        public bool Edit(Presentation item)
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

        public List<Presentation> GetAll()
        {
            List<Presentation> presantationlist;
            using (context = new BasExamContext())
            {
                presantationlist = context.Presentations.ToList();
            }

            return presantationlist;
        }
        public Presentation GetById(int id)
        {
            Presentation prn = new Presentation();
            using (context = new BasExamContext())
            {
                prn = context.Presentations.Find(id);
            }

            return prn;
        }










    }
}
