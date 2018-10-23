using System;
using System.ComponentModel.DataAnnotations.Schema;
using CatalogEntities;

namespace Inventory.DAL.Entities
{
    [Table("History")]
    public class History
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public int EmployeeId { get; set; }
        public Guid RepairPlaceId{ get; set; }
        public Guid StatusTypeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Comments { get; set; }

        public virtual RepairPlace RepairPlace { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
