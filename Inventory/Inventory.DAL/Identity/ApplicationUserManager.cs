using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Inventory.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }
    }
}
