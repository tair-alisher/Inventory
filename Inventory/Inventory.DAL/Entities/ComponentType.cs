using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
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
