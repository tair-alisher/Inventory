using System;

namespace Inventory.BLL.DTO
{
    public class HistoryDTO
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public int EmployeeId { get; set; }
        public Guid RepairPlaceId { get; set; }
        public Guid StatusTypeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Comments { get; set; }

        public RepairPlaceDTO RepairPlace { get; set; }
        public StatusTypeDTO StatusType { get; set; }
        public EquipmentDTO Equipment { get; set; }
        public CatalogEntities.Employee Employee { get; set; }
    }
}
