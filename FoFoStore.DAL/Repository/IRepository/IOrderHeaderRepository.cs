using FoFoStore.DAL.Data.Repository.IRepository;
using FoFoStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoFoStore.DAL.Repository.IRepository
{
    public interface IOrderHeaderRepository :IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
