using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("Components")]
    public class Component
    {
        public Guid Id { get; set; }
        public Guid ComponentTypeId { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string InventNumber { get; set; }
        public string Supplier { get; set; }
        
        public virtual ComponentType ComponentType { get; set; }

        public virtual ICollection<EquipmentComponentRelation> EquipmentComponentRelations { get; set; }

        public Component()
        {
            EquipmentComponentRelations = new List<EquipmentComponentRelation>();
        }
    }
}
