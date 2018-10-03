using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public int InventNumber { get; set; }
        public string QRCode { get; set; }
        public double Price { get; set; }      
        public string Supplier { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }

        public virtual ICollection<EquipmentComponent> EquipmentComponents { get; set; }

        public Equipment()
        {
            EquipmentComponents = new List<EquipmentComponent>();
        }
    }
}
