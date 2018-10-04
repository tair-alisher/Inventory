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

        public virtual ICollection<EquipmentComponent> EquipmentComponents { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<EquipmentEmployee> EquipmentEmployees { get; set; }

        public Equipment()
        {
            EquipmentComponents = new List<EquipmentComponent>();
            Histories = new List<History>();
            EquipmentEmployees = new List<EquipmentEmployee>();
        }
    }
}
