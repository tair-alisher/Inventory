using Inventory.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.BLL.Interfaces
{
    public interface IAccountService
    {
        Task CreateUser(UserDTO userDTO);
        Task AuthenticateUser(UserDTO userDTO);
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<RoleDTO> GetAllRoles();
    }
}
