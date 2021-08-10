using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FoFoStore.DAL.Data.Repository.IRepository
{
    //we dont know the type of the object
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        //list of gategory based on the number of REq return from DB
        IEnumerable<T> GetAll(
            //we dont know what is the parameter just we make it generic
                Expression<Func<T, bool>> Filter = null,
                //order by + output result
                Func<IQueryable<T>,IOrderedQueryable<T>> OrderBy = null,
                string IncludeProperties = null // forien key (eager key)
            );
         T GetFirstOrDefault(//return one object that why delete IEnumerable
               //we dont know what is the parameter just we make it generic
               Expression<Func<T, bool>> Filter = null,
               string IncludeProperties = null // forien key (eager key)
           );
        //return Entity 
        void Add(T Entity);
        // بقدر اعمل فنكشن واحد بس للتخصيص
        void Remove(int id);
        void Remove(T enityt);
        void RemoveRange(IEnumerable<T>entity);
    }
}
