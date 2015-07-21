using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.DataAccess
{
    public interface IRepository<T> where T : class 
    {
        List<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        List<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetByID(object ID);

        string GetMessage();
        string GetMessage(string Code);
        void AddMassage(string Code, string Message);

        bool Add(T item);
        bool Add(List<T> items);

        bool Update(T item);
        bool Update(List<T> items);

        bool Delete(object ID);
        bool Delete(List<T> items);
        bool CheckLogin(string LoginName, string Password);

    }
}
