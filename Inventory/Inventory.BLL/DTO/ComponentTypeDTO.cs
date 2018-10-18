using System;
using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class ComponentTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ComponentDTO> Components { get; set; }
    }
}
