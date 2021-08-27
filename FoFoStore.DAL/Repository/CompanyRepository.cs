
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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var objFromDb = _db.companies.FirstOrDefault(s=>s.Id== company.Id);
           
            if(objFromDb != null)
            {
                objFromDb.Name = company.Name;
                
            }
            
      
        }
    }
}
