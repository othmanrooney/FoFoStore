using System;
using System.Collections.Generic;
using System.Text;

namespace FoFoStore.DAL.Repository.IRepository
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository category { get; }
        ICoverTypeRepositry CoverType { get; }
        IProductRepository product { get; }
        ICompanyRepository company { get; }
        IApplicationUserRepository applicationUser { get; }
        IShoppingCartRepository shoppingCart { get; }
        IOrderHeaderRepository orderHeader { get; }
        IOrderDetailsRepository orderDetails { get; }
        ISP_Call sP_Call { get; }
        void Save();
    }
}
