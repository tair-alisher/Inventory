using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class DivisionDTO
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        public ICollection<AdministrationDTO> Administrations { get; set; }
    }
}
