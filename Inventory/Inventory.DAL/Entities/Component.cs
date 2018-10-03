using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class Component
    {
        public Guid Id { get; set; }
        public Guid ComponentTypeId { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int InventNumber { get; set; }
        public string Supplier { get; set; }
        
        public virtual ComponentType ComponentType { get; set; }

        public virtual ICollection<EquipmentComponent> EquipmentComponents { get; set; }

        public Component()
        {
            EquipmentComponents = new List<EquipmentComponent>();
        }
    }
}
