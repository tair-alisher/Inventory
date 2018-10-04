using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogEntities;

namespace Inventory.DAL.Entities
{
    public class EquipmentEmployees
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid EqupmentId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsOwner { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
