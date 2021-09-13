using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using FoFoStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoFoStore.Areas.Admin.Controllers
{
    //1-add area 
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
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

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                //thiis is for create
                return View(category);
            }
            //this is for edit 
            // using GetValueOrDefault bec id could be nulll
            category = _unitOfWork.category.Get(id.GetValueOrDefault());
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                //double secuirty with the validation in script in upsert cshtml

                if (category.Id == 0)
                {
                    _unitOfWork.category.Add(category);
                   
                }
                else
                {
                    _unitOfWork.category.Update(category);
                }
                _unitOfWork.Save();
                //بتقدر تستخدم  Index مكان 
                //nameof(Index)بس عشان ما يحصل غلط ةانت بتكتبه اندكس
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        //using datatable in js (api call)
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllObj = _unitOfWork.category.GetAll();
            return Json(new { data =AllObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.category.Get(id);
            if(objFromDb ==null)
            {
                return Json(new { success = false,message="Error While deleting" });
            }
            _unitOfWork.category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
