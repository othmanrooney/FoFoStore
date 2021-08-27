using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using FoFoStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FoFoStore.Areas.Admin.Controllers
{
    //1-add area 
    [Area("Admin")]
    public class ProductController : Controller
    {
        //2-add IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;//we will upload image on server / wwwroot
        //depency injectiion 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                //the purpose of this is to used drop downlist
                CategoryList = _unitOfWork.category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                
            }
                ;
            if (id == null)
            {
                //thiis is for create
                return View(productVM);
            }
            //this is for edit 
            // using GetValueOrDefault bec id could be nulll
            productVM.Product = _unitOfWork.product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnviroment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count>0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath,@"images\products");
                    var extenstiion = Path.GetExtension(files[0].FileName);

                    if(productVM.Product.ImageUrl !=null)
                    {
                        //this is an edit and we need to remove old image

                        var imagePath = Path.Combine(webRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using(var fileStreams = new FileStream(Path.Combine(uploads,fileName+extenstiion),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\"+fileName+extenstiion;
                }
                else
                {
                    //update when do not change the image 
                    if(productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                //double secuirty with the validation in script in upsert cshtml

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.product.Add(productVM.Product);

                }
                else
                {
                    _unitOfWork.product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                //بتقدر تستخدم  Index مكان 
                //nameof(Index)بس عشان ما يحصل غلط ةانت بتكتبه اندكس
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = _unitOfWork.category.GetAll().Select(i => new SelectListItem
                {
                    Text=i.Name,
                    Value=i.Id.ToString()
                });
                productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if(productVM.Product.Id!=0)
                {
                    productVM.Product = _unitOfWork.product.Get(productVM.Product.Id);
                }    
            }
            return View(productVM);
        }


        //using datatable in js (api call)
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllObj = _unitOfWork.product.GetAll(IncludeProperties:"category,coverType");
            return Json(new { data =AllObj });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.product.Get(id);
            if(objFromDb ==null)
            {
                return Json(new { success = false,message="Error While deleting" });
            }
            string webRootPath = _webHostEnviroment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
