using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Inventory.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IAccountWorker worker { get; set; }
        public AccountService(IAccountWorker worker)
        {
            this.worker = worker;
        }

        public async Task<bool> CreateUser(UserDTO userDTO, string role = null)
        {
            IdentityUser user = new IdentityUser { UserName = userDTO.UserName };
            IdentityResult result = await worker
                .ApplicationUserManager
                .CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
                await worker.ApplicationUserManager.AddToRoleAsync(user.Id, (role ?? "user"));

            return result.Succeeded;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = worker.ApplicationUserManager.Users.ToList();

            return BLLUserMapper.EntityToDto(users);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = worker.ApplicationRoleManager.Roles.ToList();

            return BLLRoleMapper.EntityToDto(roles);
        }
    }
}
