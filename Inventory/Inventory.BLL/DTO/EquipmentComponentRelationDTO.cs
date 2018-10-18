using System;

namespace Inventory.BLL.DTO
{
    public class EquipmentComponentRelationDTO
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid ComponentId { get; set; }
        public bool IsActual { get; set; }

        public EquipmentDTO Equipment { get; set; }
        public ComponentDTO Component { get; set; }
    }
}
