using Inventory.DAL.EF;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories
{
    public class AccountWorker : IAccountWorker
    {
        private AccountContext context;
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;

        public AccountWorker(string connectionString)
        {
            context = new AccountContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<IdentityUser>(context));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
        }

        public ApplicationUserManager ApplicationUserManager
        {
            get
            {
                return userManager;
            }
        }

        public ApplicationRoleManager ApplicationRoleManager
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
                    context.Dispose();
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
