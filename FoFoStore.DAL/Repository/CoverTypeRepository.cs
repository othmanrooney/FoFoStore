
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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepositry
    {
        private readonly ApplicationDbContext _db;
        public CoverTypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(CoverType coverType)
        {
            var objFromDb = _db.coverTypes.FirstOrDefault(s=>s.Id== coverType.Id);
           
            if(objFromDb != null)
            {
                objFromDb.Name = coverType.Name;
                
            }
            
      
        }
    }
}
