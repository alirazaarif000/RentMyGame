using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMG.BLL;
using RMG.DAL;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Models.ViewModels;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationUserBll _applicationUserBll;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public UsersController(ApplicationUserBll applicationUserBll, ApplicationDbContext context, IUserService userService)
        {
            _applicationUserBll = applicationUserBll;
            _context = context;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            RegisterVM registerVM = _applicationUserBll.BindRoleDropdown().Data;
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model, User);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> users = _applicationUserBll.GetAllApplicationUser().Data;
            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            foreach (var user in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = users });
        }

        [HttpDelete]
        public IActionResult Delete(string id) 
        {
            Result<object> result = _applicationUserBll.DeleteApplicationUser(id);
            TempData["success"] = result.Message;
            return Json(result);
        }

    }
}
