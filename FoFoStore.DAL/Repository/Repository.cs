using FoFoStore.DAL.Data;
using FoFoStore.DAL.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace FoFoStore.DAL.Repository
{
    //Generic Repository
    public class Repository<T> : IRepository<T> where T : class
    {
        //we need the DBContext to Modify DB
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        //instance from dependency injection and constructor
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T Entity)
        {
            dbSet.Add(Entity);
        }
        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> Filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string IncludeProperties = null)
        {
            IQueryable<T> Query = dbSet;
            if(Filter != null)
            {
                Query = Query.Where(Filter);
            }
            if(IncludeProperties != null)
            {
                //لو كان عندي جاتيجوري جوا كاتيجوري ثاني راح يكون فورين كي للبرايمري كي الاول
                foreach (var IncludeProp in IncludeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(IncludeProp);
                }
            }
            if(OrderBy != null)
            {
                return OrderBy(Query).ToList();
            }
            return Query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> Filter = null, string IncludeProperties = null)
        {
            IQueryable<T> Query = dbSet;
            if (Filter != null)
            {
                Query = Query.Where(Filter);
            }
            if (IncludeProperties != null)
            {
                //لو كان عندي جاتيجوري جوا كاتيجوري ثاني راح يكون فورين كي للبرايمري كي الاول
                foreach (var IncludeProp in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(IncludeProp);
                }
            }
            
            return Query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T Entity = dbSet.Find(id);
            Remove(Entity);
        }

        public void Remove(T enityt)
        {
            dbSet.Remove(enityt);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
