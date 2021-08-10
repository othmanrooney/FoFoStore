
using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using FoFoStore.DAL.Data;
using FoFoStore.DAL.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FoFoStore.DAL.Repository;

namespace FoFoStore.DAL.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var objFromDb = _db.categories.FirstOrDefault(s=>s.Id==category.Id);
            objFromDb.Name = category.Name;
            if(objFromDb != null)
            {
                _db.SaveChanges();
            }
            
            throw new NotImplementedException();
        }
    }
}
