using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class PositionDTO
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }

        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}
