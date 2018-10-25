using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class EquipmentTypeVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string Name { get; set; }

        public ICollection<EquipmentDTO> Equipments { get; set; }
    }
}