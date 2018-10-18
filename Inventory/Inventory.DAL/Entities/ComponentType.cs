using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("ComponentTypes")]
    public class ComponentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Component> Components { get; set; }

        public ComponentType()
        {
            Components = new List<Component>();
        }
    }
}
