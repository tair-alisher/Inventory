using Inventory.DAL.EF;
using Inventory.DAL.Entities;
using Inventory.DAL.Identity;
using Inventory.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories
{
    public class AccountWorker : IAccountWorker
    {
        private IdentityContext context;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        PasswordValidator passwordValidator = new PasswordValidator
        {
            RequiredLength = 8,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true
        };

        public AccountWorker(string connectionString)
        {
            context = new IdentityContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            userManager.PasswordValidator = passwordValidator;
            userManager.UserValidator = new AppUserValidator(userManager);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return roleManager;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //userManager.Dispose();
                    //roleManager.Dispose();
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
