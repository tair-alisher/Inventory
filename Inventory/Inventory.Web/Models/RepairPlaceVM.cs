using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class RepairPlaceVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
    }
}