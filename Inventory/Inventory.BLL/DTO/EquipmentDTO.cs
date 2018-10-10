using System;
using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class EquipmentDTO
    {
        public Guid Id { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public string InventNumber { get; set; }
        public string QRCode { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; }

        public EquipmentTypeDTO EquipmentType { get; set; }

        public ICollection<EquipmentComponentRelationDTO> EquipmentComponentRelations { get; set; }
        public ICollection<HistoryDTO> History { get; set; }
        public ICollection<EquipmentEmployeeRelationDTO> EquipmentEmployeeRelations { get; set; }
    }
}
