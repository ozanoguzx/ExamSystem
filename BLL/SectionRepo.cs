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
   public class SectionRepo:IRepository<Section>
    {
        BasExamContext context;
        bool result = false;

        public bool Add(Section item)
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
                Section sec = context.Sections.Find(id);
                context.Entry(sec).State = System.Data.Entity.EntityState.Deleted;

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

        public bool Edit(Section item)
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

        public List<Section> GetAll()
        {
            List<Section> Sectionlist;
            using (context = new BasExamContext())
            {
                Sectionlist = context.Sections.ToList();
            }

            return Sectionlist;
        }
        public Section GetById(int id)
        {
            Section sec = new Section();
            using (context = new BasExamContext())
            {
                sec = context.Sections.Find(id);
            }

            return sec;
        }
    }
}
