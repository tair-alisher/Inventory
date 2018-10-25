using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class StatusTypeVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Наименование")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я ]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string Name { get; set; }
    }
}