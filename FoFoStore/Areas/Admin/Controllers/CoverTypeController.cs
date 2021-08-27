using FoFoStore.Utility;
using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace FoFoStore.Areas.Admin.Controllers
{
    //1-add area 
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        //2-add IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        //depency injectiion 
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType   = new CoverType();
            if (id == null)
            {
                //thiis is for create
                return View(coverType);
            }
            //this is for edit 
            // using GetValueOrDefault bec id could be nulll
            var parameter = new DynamicParameters();
            parameter.Add(@"Id", id);
            coverType = _unitOfWork.sP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add(@"Name",coverType.Name);
                //double secuirty with the validation in script in upsert cshtml

                if (coverType.Id == 0)
                {
                    _unitOfWork.sP_Call.Execute(SD.Proc_CoverType_Create,parameter);
                   
                }
                else
                {
                    parameter.Add("@Id", coverType.Id);
                    _unitOfWork.sP_Call.Execute(SD.Proc_CoverType_Update, parameter);
                }
                _unitOfWork.Save();
                //بتقدر تستخدم  Index مكان 
                //nameof(Index)بس عشان ما يحصل غلط ةانت بتكتبه اندكس
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }


        //using datatable in js (api call)
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllObj = _unitOfWork.sP_Call.List<CoverType>(SD.Proc_CoverType_GetAll,null);
            return Json(new { data =AllObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add(@"Id",id);
            var objFromDb = _unitOfWork.sP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get,parameter);
            if(objFromDb ==null)
            {
                return Json(new { success = false,message="Error While deleting" });
            }
            _unitOfWork.sP_Call.Execute(SD.Proc_CoverType_Delete,parameter);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
