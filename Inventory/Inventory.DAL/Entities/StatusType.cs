using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("StatusTypes")]
    public class StatusType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
