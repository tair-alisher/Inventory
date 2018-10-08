using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentService : IService<EquipmentDTO>
    {
        EmployeeDTO GetEquipmentOwner(Guid id);
        Guid AddAndGetId(EquipmentDTO equipment);
    }
}
