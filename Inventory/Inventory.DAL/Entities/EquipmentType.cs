using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class EquipmentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }

        public EquipmentType()
        {
            Equipments = new List<Equipment>();
        }
    }
}
