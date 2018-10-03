using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogEntities;

namespace Inventory.DAL.Entities
{
    public class History
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid PlaceId{ get; set; }
        public Guid StatusId{ get; set; }
        public string Comments{ get; set; }

        public virtual RepairPlace RepairPlace { get; set; }
        public virtual TypeStatus TypeStatus { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
