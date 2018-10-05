using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.Web.Models
{
    public class ComponentVM
    {
        public Guid Id { get; set; }
        public Guid ComponentTypeId { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string InventNumber { get; set; }
        public string Supplier { get; set; }

        public ComponentTypeDTO ComponentType { get; set; }
        public ICollection<EquipmentComponentDTO> EquipmentComponents { get; set; }
    }
}