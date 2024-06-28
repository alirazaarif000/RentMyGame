using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using RMG.DAL.Repository.IRepository;
using RMG.Models.ViewModels;
using RMG.Utility;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IUserService _registrationService;
        public LoginController(IUserService registrationService)
        {
            _registrationService = registrationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _registrationService.LoginUserAsync(model, SD.Role_Admin);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Genre");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Only administrators can log in.");
                }
            }

            return View(model);
        }
    }
}
