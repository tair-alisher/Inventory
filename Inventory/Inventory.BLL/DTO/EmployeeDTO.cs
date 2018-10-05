namespace Inventory.BLL.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeRoom { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }

        public PositionDTO Position { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}
