using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

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
            ApplicationUser user = await worker.UserManager.FindByEmailAsync(userDTO.UserName);
            IdentityResult result;
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDTO.UserName, Email = userDTO.Email };
                var result = await worker.UserManager.CreateAsync(user, userDTO.Password);
                await worker.UserManager.addToRoleAsync(user.Id, userDTO.Role);
                await worker.SaveAsync();
            }

            return result.Succeeded;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = worker.UserManager.Users.ToList();

            return BLLUserMapper.EntityToDto(users);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = worker.RoleManager.Roles.ToList();

            return BLLRoleMapper.EntityToDto(roles);
        }
    }
}
