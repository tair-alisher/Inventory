using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.Web.Models
{
    public class ComponentTypeVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ComponentDTO> Components { get; set; }
    }
}