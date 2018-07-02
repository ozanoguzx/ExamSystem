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
    public class StudentRepo : IRepository<Student>
    {
        BasExamContext context = new BasExamContext();
        bool result = false;

        public bool Add(Student item)
        {
            using (context = new BasExamContext())
            {
                context.Entry(item).State = EntityState.Added;
                 //context.StudentS.Add(item);
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
                Student std = context.Students.Find(id);
                context.Entry(std).State = System.Data.Entity.EntityState.Deleted;

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

        public bool Edit(Student item)
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

        public List<Student> GetAll()
        {
            List<Student> StudentList;
            //using (context = new BasExamContext())
            //{
            //    
            //}
            StudentList = context.Students.ToList();
            return StudentList;
        }
        public Student GetById(int id)
        {
            Student std = new Student();
            using (context = new BasExamContext())
            {
                std = context.Students.Find(id);
            }

            return std;
        }
    }
}
