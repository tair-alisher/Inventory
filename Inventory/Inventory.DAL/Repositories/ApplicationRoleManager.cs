using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Inventory.DAL.Repositories
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store) { }
    }
}
