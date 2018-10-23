using System;
using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class EquipmentTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<EquipmentDTO> Equipments { get; set; }
    }
}
