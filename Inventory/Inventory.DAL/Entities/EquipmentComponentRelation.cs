using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("EquipmentComponent")]
    public class EquipmentComponentRelation
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid ComponentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActual { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Component Component { get; set; }
    }
}
