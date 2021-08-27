using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoFoStore.Areas.Admin.Controllers
{
    //1-add area 
    [Area("Admin")]
    public class CompanyController : Controller
    {
        //2-add IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        //depency injectiion 
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
            {
                //thiis is for create
                return View(company);
            }
            //this is for edit 
            // using GetValueOrDefault bec id could be nulll
            company = _unitOfWork.company.Get(id.GetValueOrDefault());
            if(company == null)
            {
                return NotFound();
            }
            return View(company);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                //double secuirty with the validation in script in upsert cshtml

                if (company.Id == 0)
                {
                    _unitOfWork.company.Add(company);
                   
                }
                else
                {
                    _unitOfWork.company.Update(company);
                }
                _unitOfWork.Save();
                //بتقدر تستخدم  Index مكان 
                //nameof(Index)بس عشان ما يحصل غلط ةانت بتكتبه اندكس
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }


        //using datatable in js (api call)
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllObj = _unitOfWork.company.GetAll();
            return Json(new { data =AllObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.company.Get(id);
            if(objFromDb ==null)
            {
                return Json(new { success = false,message="Error While deleting" });
            }
            _unitOfWork.company.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
