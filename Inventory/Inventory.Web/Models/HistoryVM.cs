using Inventory.BLL.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class HistoryVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Оборудование")]
        public Guid EquipmentId { get; set; }

        [Display(Name = "Сотрудник")]
        public int EmployeeId { get; set; }

        [Display(Name = "Место ремонта")]
        public Guid RepairPlaceId { get; set; }

        [Display(Name = "Статус")]
        public Guid StatusTypeId { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime ChangeDate { get; set; } 

        [Display(Name = "Комментарий")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string Comments { get; set; }

        public RepairPlaceDTO RepairPlace { get; set; }
        public StatusTypeDTO StatusType { get; set; }
        public EquipmentDTO Equipment { get; set; }
        public CatalogEntities.Employee Employee { get; set; }
    }
}