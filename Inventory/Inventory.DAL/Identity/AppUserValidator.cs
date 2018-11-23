using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DAL.Identity
{
    public class AppUserValidator : UserValidator<ApplicationUser>
    {
        private readonly ApplicationUserManager _userManager;
        public AppUserValidator(ApplicationUserManager manager) : base(manager)
        {
            AllowOnlyAlphanumericUserNames = false;
            _userManager = manager;

        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser != null)
            {
                var errors = result.Errors.ToList();
                errors.Add("Пользователь с таким логином уже существует.");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
