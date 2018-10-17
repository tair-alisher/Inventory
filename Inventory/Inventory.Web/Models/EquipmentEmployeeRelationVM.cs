using Inventory.BLL.DTO;
using System;

namespace Inventory.Web.Models
{
    public class EquipmentEmployeeRelationVM
    {
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public bool IsOwner { get; set; }

        public EmployeeDTO Employee { get; set; }
        public EquipmentDTO Equipment { get; set; }
    }
}