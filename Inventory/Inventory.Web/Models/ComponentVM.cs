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
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string ModelName { get; set; }

        [Display(Name = "Наименование")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [StringLength(300, ErrorMessage = "Длина строки не должна превышать 300 символов")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Инвентаризационный номер")]
        public string InventNumber { get; set; }

        [Display(Name = "Поставщик")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string Supplier { get; set; }

        public ComponentTypeDTO ComponentType { get; set; }
        public ICollection<EquipmentComponentRelationDTO> EquipmentComponentRelations { get; set; }
    }
}