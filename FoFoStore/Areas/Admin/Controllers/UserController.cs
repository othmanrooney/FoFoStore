using FoFoStore.DAL.Data;
using FoFoStore.DAL.Repository.IRepository;
using FoFoStore.Models;
using FoFoStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoFoStore.Areas.Admin.Controllers
{   //other way to using applicationdbContext without Repository 
    //1-add area 
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        //2-add IUnitOfWork
        //private readonly IUnitOfWork _unitOfWork;
        //depency injectiion 
        public UserController(ApplicationDbContext db)
        {
            _db=db;
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

            var UserList = _db.applicationUsers.Include(u=>u.company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in UserList) 
            {
                var roleId = userRole.FirstOrDefault(u=>u.UserId==user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u=>u.Id==roleId).Name;
                if(user.company ==null)
                {
                    user.company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = UserList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            //if it is lock we will lock them , if its unlock we will lock 
            var objFromDb = _db.applicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb==null)
            {
                return Json(new {success=false , message="Error While Locking/Unlocking" });

            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user currently locked will unlock
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful. " });
        }

        #endregion
    }
}
