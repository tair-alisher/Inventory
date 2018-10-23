using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class ComponentVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Тип комплектующего")]
        public Guid ComponentTypeId { get; set; }

        [Display(Name = "Наименование модели")]
        public string ModelName { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Инвентаризационный номер")]
        public string InventNumber { get; set; }

        [Display(Name = "Поставщик")]
        public string Supplier { get; set; }

        public ComponentTypeDTO ComponentType { get; set; }
        public ICollection<EquipmentComponentRelationDTO> EquipmentComponentRelations { get; set; }
    }
}