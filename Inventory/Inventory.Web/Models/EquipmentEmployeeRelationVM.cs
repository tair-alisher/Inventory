using Inventory.BLL.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class EquipmentEmployeeRelationVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Сотрудник")]
        public int EmployeeId { get; set; }

        [Display(Name = "Оборудование")]
        public Guid EquipmentId { get; set; }

        [Display(Name = "Когда сотрудник был закреплен за оборудованием")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Когда сотрудник был откреплен от оборудования")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Является текущим владельцем")]
        public bool IsOwner { get; set; }

        public EmployeeDTO Employee { get; set; }
        public EquipmentDTO Equipment { get; set; }
    }
}