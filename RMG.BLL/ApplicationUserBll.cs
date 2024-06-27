using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class ApplicationUserBll
    {
        private readonly IUnitOfWork _uow;
        private readonly ApplicationDbContext _context;
        public ApplicationUserBll(IUnitOfWork uow, ApplicationDbContext context)
        {
            _uow = uow;
            _context = context;
        }
        public Result<List<ApplicationUser>> GetAllApplicationUser()
        {
            try
            {
                List<ApplicationUser> applicationUsers = _uow.ApplicationUser.GetAll(IncludeProperties: "Subscription").ToList();
                return new Result<List<ApplicationUser>>
                {
                    Status = true,
                    Data = applicationUsers,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ApplicationUser>> { Status = false, Message = ex.Message };
            }
        }
        public Result<ApplicationUser> GetApplicationUser(string? id)
        {
            try
            {
                ApplicationUser applicationUser = _uow.ApplicationUser.Get(u => u.Id == id, IncludeProperties: "Subscription");
                return new Result<ApplicationUser>
                {
                    Status = true,
                    Data = applicationUser,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<ApplicationUser> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddApplicationUser(ApplicationUser applicationUser)
        {
            try
            {
                _uow.ApplicationUser.Add(applicationUser);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> UpdateApplicationUser(ApplicationUser applicationUser)
        {
            try
            {
                _uow.ApplicationUser.Update(applicationUser);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = true, Data = null, StatusCode = 200 };
            }
        }

        public Result<object> DeleteApplicationUser(string id)
        {
            try
            {
                var ApplicationUserToBeDeleted = GetApplicationUser(id).Data;
                if (ApplicationUserToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting ApplicationUser" };
                }
                else
                {
                    _uow.ApplicationUser.Remove(ApplicationUserToBeDeleted);
                    _uow.Save();
                    return new Result<object> { Status = true, Data = null, StatusCode = 200, Message = "Deleted Successfully" };
                }

            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<RegisterVM> BindRoleDropdown()
        {
            try
            {
                RegisterVM registerVM = new()
                {
                    RoleList = _context.Roles.Select(x => x.Name).Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    })
                };
                return new Result<RegisterVM>
                {
                    Status = true,
                    Data = registerVM,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<RegisterVM> { Status = false, Message = ex.Message };

            }
        }
    }
}
