using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentEmployeeRelationService : IService<EquipmentEmployeeRelationDTO>
    {
        void Create(Guid equipmentId, int employeeId);
    }
}
