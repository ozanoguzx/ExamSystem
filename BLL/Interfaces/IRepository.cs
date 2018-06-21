using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRepository <T>
    {
        List<T> GetAll();
        T GetById(int id);
        bool Add(T item);
        bool Edit(T item);
        bool Delete(int id);          
    }
}
