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
        private const string DEFAULT_ROLE = "user";

        private IAccountWorker worker { get; set; }
        public AccountService(IAccountWorker worker)
        {
            this.worker = worker;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            ApplicationUser user = await worker.UserManager.FindByEmailAsync(userDTO.UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDTO.UserName, Email = userDTO.Email };
                var result = await worker.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    await worker.UserManager.AddToRoleAsync(user.Id, (userDTO.Role ?? DEFAULT_ROLE));
                    await worker.SaveAsync();
                }
                else
                {
                    if (result.Errors.Contains($"Name {user.UserName} is already taken."))
                        throw new UserAlreadyExistsException();
                    else if (result.Errors.Count() > 0)
                        throw new System.Exception("Something went wrong.");
                }
            }
        }

        public async Task AuthenticateUser(UserDTO userDTO)
        {
            var user = await worker.UserManager.FindAsync(userDTO.UserName, userDTO.Password);
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
