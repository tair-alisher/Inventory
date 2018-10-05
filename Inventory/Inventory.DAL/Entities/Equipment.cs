using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("Equipments")]
    public class Equipment
    {
        public Guid Id { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public string InventNumber { get; set; }
        public string QRCode { get; set; }
        public decimal Price { get; set; }      
        public string Supplier { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }

        public virtual ICollection<EquipmentComponent> EquipmentComponent { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<EquipmentEmployee> EquipmentEmployee { get; set; }

        public Equipment()
        {
            EquipmentComponent = new List<EquipmentComponent>();
            History = new List<History>();
            EquipmentEmployee = new List<EquipmentEmployee>();
        }
    }
}
