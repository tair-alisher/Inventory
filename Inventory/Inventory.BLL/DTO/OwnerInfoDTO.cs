using System;

namespace Inventory.BLL.DTO
{
    public class OwnerInfoDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActual { get; set; }
    }
}
