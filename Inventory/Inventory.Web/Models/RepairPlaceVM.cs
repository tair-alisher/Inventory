using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class RepairPlaceVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Наименование")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string Name { get; set; }
    }
}