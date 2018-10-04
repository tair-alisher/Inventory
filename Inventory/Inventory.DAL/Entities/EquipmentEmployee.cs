using System;
using System.ComponentModel.DataAnnotations.Schema;
using CatalogEntities;

namespace Inventory.DAL.Entities
{
    [Table("EquipmentEmployee")]
    public class EquipmentEmployee
    {
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public Guid EqupmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsOwner { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
