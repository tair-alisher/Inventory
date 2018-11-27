using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Inventory.DAL.EF
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<IdentityContext>(null);
        }
    }
}
