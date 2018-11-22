using Inventory.BLL.DTO;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEmployeeService : IPartialService<EmployeeDTO>
    {
        IEnumerable<OwnerInfoDTO> ValidateNameAndGetEmployeesByName(string inputName);
    }
}
