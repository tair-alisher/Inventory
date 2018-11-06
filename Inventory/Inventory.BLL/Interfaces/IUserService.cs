using Inventory.BLL.DTO;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
    }
}
