using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Inventory.DAL.EF
{
    public class AccountContext : IdentityDbContext<IdentityUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public AccountContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<AccountContext>(null);
        }
    }
}
