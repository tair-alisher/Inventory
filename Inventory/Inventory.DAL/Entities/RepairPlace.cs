using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.DAL.Entities
{
    [Table("RepairPlaces")]
    public class RepairPlace
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
