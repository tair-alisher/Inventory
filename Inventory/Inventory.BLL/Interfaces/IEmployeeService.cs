using Inventory.BLL.DTO;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEmployeeService : IPartialService<EmployeeDTO>
    {
        IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname);
        IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname);
        IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname, string mname);
    }
}
