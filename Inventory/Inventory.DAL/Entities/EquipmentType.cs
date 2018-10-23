using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("EquipmentTypes")]
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
