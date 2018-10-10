using System;
using System.ComponentModel.DataAnnotations.Schema;
using CatalogEntities;

namespace Inventory.DAL.Entities
{
    [Table("EquipmentEmployee")]
    public class EquipmentEmployeeRelation
    {
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsOwner { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
