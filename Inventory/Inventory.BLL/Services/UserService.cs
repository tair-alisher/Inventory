using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.BLL.Services
{
    public class UserService : IUserService
    {
        private const string DEFAULT_ROLE = "user";

        private IAccountWorker worker { get; set; }

        public UserService(IAccountWorker worker)
        {
            this.worker = worker;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            List<ApplicationUser> users = worker.UserManager.Users.ToList();

            return BLLUserMapper.EntityToDto(users);
        }

        public async Task<string> GetUserRole(string userId)
        {
            IList<string> roles = await worker.UserManager.GetRolesAsync(userId);
            string roleName = roles.FirstOrDefault();

            return roleName;
        }
    }
}
