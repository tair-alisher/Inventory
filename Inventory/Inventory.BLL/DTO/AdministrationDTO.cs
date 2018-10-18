using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class AdministrationDTO
    {
        public int AdministrationId { get; set; }
        public string AdministrationName { get; set; }
        public int DivisionId { get; set; }

        public DivisionDTO Division { get; set; }
        public ICollection<DepartmentDTO> Departments { get; set; }
    }
}
