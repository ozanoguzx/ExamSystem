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
   public class TrainerRepo:IRepository<Trainer> 
    {
        BasExamContext context;
        bool result = false;

        public bool Add(Trainer item)
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
                Trainer trn = context.Trainers.Find(id);
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

        public bool Edit(Trainer item)
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

        public List<Trainer> GetAll()
        {
            List<Trainer> trainerlist;
            using (context = new BasExamContext())
            {
                trainerlist = context.Trainers.ToList();
            }

            return trainerlist;
        }
        public Trainer GetById(int id)
        {
            Trainer trn = new Trainer();
            using (context = new BasExamContext())
            {
                trn = context.Trainers.Find(id);
            }

            return trn;
        }


    }
}
