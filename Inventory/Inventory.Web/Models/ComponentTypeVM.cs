using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class ComponentTypeVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        public ICollection<ComponentDTO> Components { get; set; }
    }
}