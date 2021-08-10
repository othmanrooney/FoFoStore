﻿using FoFoStore.DAL.Data;
using FoFoStore.DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoFoStore.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            //شان تقدر تعمل اكسيز علىيه
            category = new CategoryRepository(_db);
            sP_Call = new SP_Call(_db);
        }
        public ICategoryRepository category { get; private set; }

    

        public ISP_Call sP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        //لما نستخدم اي ريبوزيتوري ما بكون وبولا فنكشن حفظ 
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}