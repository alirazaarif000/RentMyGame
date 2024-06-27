using Microsoft.AspNetCore.Identity;
using RMG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository.IRepository
{
    public interface IUserRegistrationService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterVM model,ClaimsPrincipal user);
        Task<SignInResult> LoginUserAsync(LoginVM model, string Role);
    }
}
