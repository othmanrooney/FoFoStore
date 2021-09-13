
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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails Obj)
        {
            _db.Update(Obj)
      
        }
    }
}
