using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class OwnerInfoVM
    {
        public int EmployeeId { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Кабинет")]
        public string Room { get; set; }

        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Отдел")]
        public string Department { get; set; }

        [Display(Name = "Орган управления")]
        public string Administration { get; set; }

        [Display(Name = "Регион")]
        public string Division { get; set; }

        [Display(Name = "Когда сотрудник был закреплен за оборудованием")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Когда сотрудник был откреплен от оборудования")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Является текущим владельцем")]
        public bool IsActual { get; set; }
    }
}