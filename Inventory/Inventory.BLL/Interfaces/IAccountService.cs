using Inventory.BLL.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.BLL.Interfaces
{
    public interface IAccountService
    {
        Task CreateUser(UserDTO userDTO);
        Task<ClaimsIdentity> AuthenticateUser(UserDTO userDTO);
        Task<UserDTO> GetUser(string id);
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<RoleDTO> GetAllRoles();
        Task UpdateEmail(UserDTO userDTO);
        Task UpdatePassword(ChangePasswordDTO changePasswordDTO);
    }
}
