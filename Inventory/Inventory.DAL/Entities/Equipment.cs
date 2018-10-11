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

        public virtual ICollection<EquipmentComponentRelation> EquipmentComponentRelations { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<EquipmentEmployeeRelation> EquipmentEmployeeRelations { get; set; }

        public Equipment()
        {
            EquipmentComponentRelations = new List<EquipmentComponentRelation>();
            History = new List<History>();
            EquipmentEmployeeRelations = new List<EquipmentEmployeeRelation>();
        }
    }
}
