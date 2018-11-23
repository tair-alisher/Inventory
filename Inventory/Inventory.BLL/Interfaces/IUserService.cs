using Inventory.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        Task<string> GetUserRole(string userId);
        Task ChangeUserRole(ChangeRoleDTO changeRoleDTO);
        Task DeleteUser(string userId);
    }
}
