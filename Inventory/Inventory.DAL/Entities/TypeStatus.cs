using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Entities
{
    public class TypeStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<History> Histories { get; set; }

        public TypeStatus()
        {
            Histories = new List<History>();
        }
    }
}
