using Inventory.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CreateUser(UserDTO userDTO, string role = null);
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<RoleDTO> GetAllRoles();
    }
}
