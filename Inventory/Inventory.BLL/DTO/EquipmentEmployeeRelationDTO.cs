using System;

namespace Inventory.BLL.DTO
{
    public class EquipmentEmployeeRelationDTO
    {
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public bool IsOwner { get; set; }

        public CatalogEntities.Employee Employee { get; set; }
        public EquipmentDTO Equipment { get; set; }
    }
}
