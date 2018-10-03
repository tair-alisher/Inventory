using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class RepairPlace
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<History> Histories { get; set; }

        public RepairPlace()
        {
            Histories = new List<History>();
        }
    }
}
