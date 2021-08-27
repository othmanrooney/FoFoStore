
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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        //public void Update(ApplicationUser applicationUser)
        //{
        //    var objFromDb = _db.applicationUsers.FirstOrDefault(s=>s.Id== applicationUser.Id);
           
        //    if(objFromDb != null)
        //    {
        //        objFromDb.Name = applicationUser.Name;
                
        //    }
            
      
        //}
    }
}
