using FoFoStore.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoFoStore.Areas.Admin.Controllers
{
    //1-add area 
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //2-add IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        //depency injectiion 
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        //using datatable in js (api call)
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllObj = _unitOfWork.category.GetAll();
            return Json(new { data =AllObj });
        }
        #endregion
    }
}
