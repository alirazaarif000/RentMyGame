using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using RMG.DAL.Repository.IRepository;
using RMG.Models.ViewModels;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IUserRegistrationService _registrationService;
        public LoginController(IUserRegistrationService registrationService)
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
                var result = await _registrationService.LoginUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Genre");
                }

                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }
    }
}
