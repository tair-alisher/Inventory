using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models
{
    public class EquipmentVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Тип оборудования")]
        public Guid EquipmentTypeId { get; set; }
        [Display(Name = "Инвентаризационный номер")]
        public string InventNumber { get; set; }
        [Display(Name = "QR-код")]
        public string QRCode { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Источник поступления")]
        public string Supplier { get; set; }

        public EquipmentTypeDTO EquipmentType { get; set; }

        public ICollection<EquipmentComponentRelationDTO> EquipmentComponentRelations { get; set; }
        public ICollection<HistoryDTO> History { get; set; }
        public ICollection<EquipmentEmployeeRelationDTO> EquipmentEmployeeRelations { get; set; }
    }
}