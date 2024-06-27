using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Models.ViewModels;
using RMG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;

        public UserRegistrationService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterVM model, ClaimsPrincipal User)
        {
            var user = CreateUser();
            user.SubscriptionId = null;
            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!String.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }

                if (!User.IsInRole(SD.Role_Admin))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            return result;
        }
        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public async Task<SignInResult> LoginUserAsync(LoginVM loginVM, string Role)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (Role == SD.Role_Admin)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (user != null)
                    {
                        var isAdmin = await _userManager.IsInRoleAsync(user, SD.Role_Admin);
                        if (!isAdmin)
                        {
                            await _signInManager.SignOutAsync();
                            return SignInResult.Failed;
                        }
                    }
                }
            }

            return result;
        }
    }
}
