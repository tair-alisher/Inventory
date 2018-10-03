using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class EquipmentComponent
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid ComponentId { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public bool IsActual { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Component Component { get; set; }
    }
}
