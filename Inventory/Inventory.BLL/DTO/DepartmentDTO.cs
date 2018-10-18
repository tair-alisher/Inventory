using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string AdministrationId { get; set; }

        public AdministrationDTO Administration { get; set; }
        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}
